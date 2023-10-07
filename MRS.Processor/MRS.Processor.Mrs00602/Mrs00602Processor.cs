using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;

using MRS.MANAGER.Config;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPtttGroup;
using MOS.MANAGER.HisSereServBill;
using MOS.DAO.Sql;
using MOS.MANAGER.HisBedRoom;

namespace MRS.Processor.Mrs00602
{
    public class Mrs00602Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        Mrs00602Filter filter = null;
        List<Mrs00602RDOTreatment> ListRdoTreatment = new List<Mrs00602RDOTreatment>();
        List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<V_HIS_MEDICINE_TYPE> ListHisMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> ListHisMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        List<HIS_BED_ROOM> ListBedRoom = new List<HIS_BED_ROOM>();
        List<HIS_DEPARTMENT> ListDeparment = new List<HIS_DEPARTMENT>();
        List<HIS_PTTT_GROUP> ListPtttGroup = new List<HIS_PTTT_GROUP>();
        System.Collections.Generic.Dictionary<string, Mrs00602RDOService> dicRdoService = new Dictionary<string, Mrs00602RDOService>();
        System.Collections.Generic.Dictionary<string, Mrs00602RDOService> dicRdoRouteService = new Dictionary<string, Mrs00602RDOService>();
        System.Collections.Generic.Dictionary<string, Mrs00602RDOMedicine> dicRdoMedicine = new Dictionary<string, Mrs00602RDOMedicine>();
        System.Collections.Generic.Dictionary<string, Mrs00602RDOMaterial> dicRdoMaterial = new Dictionary<string, Mrs00602RDOMaterial>();
        List<HIS_TREATMENT> ListHisTreatment = new List<HIS_TREATMENT>();
        Dictionary<long, string> DicParentCode = new Dictionary<long, string>();
        private const string RouteCodeA = "A";
        private const string RouteCodeB = "B";

        public Mrs00602Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00602Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00602Filter)reportFilter);
            var result = true;
            try
            {

                //get dữ liệu nhóm báo cáo
                GetServiceRety();

                //get dữ liệu loại thuốc vật tư
                GetMetyMaty();

                //get dữ liệu dịch vụ và dịch vụ cha
                GetService();

                //get dữ liệu buồng bệnh
                GetBedRoom();

                HisPtttGroupFilterQuery ptttGroupFilter = new HisPtttGroupFilterQuery();
                ptttGroupFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                ListPtttGroup = new HisPtttGroupManager(paramGet).Get(ptttGroupFilter);
                ListDeparment = new ManagerSql().GetDepaments();
                var listIdDepament = ListDeparment.Select(x => x.ID).Distinct().ToList();

                //sửa lại lấy thông tin hồ sơ điều trị trong 1 truy vấn
                ListHisTreatment = new ManagerSql().GetTreatment(filter);

                if (filter.IS_NOT_BHYT.HasValue && filter.IS_NOT_BHYT.Value)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => o.TDL_PATIENT_TYPE_ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
                }

                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => filter.TDL_PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                if (filter.IS_PAUSE_AND_ACTIVE == true)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
               
                var treatmentIds = ListHisTreatment.Select(o => o.ID).Distinct().ToList();
                if (treatmentIds.Count > 0)
                {

                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        List<HIS_TREATMENT> ListHisTreatmentLocal = new List<HIS_TREATMENT>();
                        List<HIS_TRANSACTION> ListHisBillLocal = new List<HIS_TRANSACTION>();
                        List<HIS_SERE_SERV_BILL> ListHisSereServBillLocal = new List<HIS_SERE_SERV_BILL>();
                        List<HIS_SERE_SERV> ListHisSereServLocal = new List<HIS_SERE_SERV>();
                        List<HIS_PATIENT_TYPE_ALTER> ListHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                        var lists = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        ListHisTreatmentLocal = ListHisTreatment.Where(o => lists.Contains(o.ID)).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //danh sách giao dịch
                        HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                        HisTransactionfilter.TREATMENT_IDs = lists;
                        HisTransactionfilter.IS_CANCEL = false;
                        //HisTransactionfilter.HAS_SALL_TYPE = false;
                        var listHisTransactionSub = new HisTransactionManager(paramGet).Get(HisTransactionfilter);
                        if (listHisTransactionSub != null)
                        {
                            ListHisBillLocal=listHisTransactionSub.Where(o=>o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                        }

                        //bảng kê dịch vụ
                        HisSereServFilterQuery HisSereServfilter = new HisSereServFilterQuery();
                        HisSereServfilter.TREATMENT_IDs = lists;
                        HisSereServfilter.HAS_EXECUTE = true;
                        var listHisSereServSub = new HisSereServManager(paramGet).Get(HisSereServfilter);
                        if (listHisSereServSub != null)
                        {
                            ListHisSereServLocal.AddRange(listHisSereServSub);
                        }

                        //chi tiết thanh toán
                        HisSereServBillFilterQuery HisSereServBillfilter = new HisSereServBillFilterQuery();
                        HisSereServBillfilter.TDL_TREATMENT_IDs = lists;
                        HisSereServBillfilter.IS_NOT_CANCEL = true;
                        var listHisSereServBillSub = new HisSereServBillManager(paramGet).Get(HisSereServBillfilter);
                        if (listHisSereServBillSub != null)
                        {
                            ListHisSereServBillLocal.AddRange(listHisSereServBillSub);
                        }

                        //lịch sử chuyển đổi đối tượng
                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterFilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterFilter.TREATMENT_IDs = lists;
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(paramGet).Get(HisPatientTypeAlterFilter);
                        if (listHisPatientTypeAlterSub != null)
                        {
                            ListHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                        }

                        //chỉ lấy dịch vụ có y lệnh
                        ListHisSereServLocal = ListHisSereServLocal.Where(p => p.SERVICE_REQ_ID.HasValue).ToList();

                        //chỉ lấy dịch vụ có thông tin hồ sơ điều trị
                        ListHisSereServLocal = ListHisSereServLocal.Where(x => x.TDL_TREATMENT_ID != null).ToList();
                        Inventec.Common.Logging.LogSystem.Info("Count Treatment: " + ListHisTreatmentLocal.Count);

                        //Lọc đối tượng thanh toán
                        if (filter.PATIENT_TYPE_IDs != null)
                        {
                            ListHisSereServLocal = ListHisSereServLocal.Where(o => filter.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID)).ToList();
                            //lọc chi tiết thanh toán liên quan
                            var ssId = ListHisSereServLocal.Select(o => o.ID).ToList();
                            ListHisSereServBillLocal = ListHisSereServBillLocal.Where(o => ssId.Contains(o.SERE_SERV_ID)).ToList();
                            //lọc giao dịch liên quan
                            var ssbId = ListHisSereServBillLocal.Select(o => o.BILL_ID).Distinct().ToList();
                            ListHisBillLocal = ListHisBillLocal.Where(o => ssbId.Contains(o.ID)).ToList();
                            //lọc hồ sơ điều trị liên quan
                            var treatIds = ListHisSereServLocal.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                            ListHisTreatmentLocal = ListHisTreatmentLocal.Where(o => treatIds.Contains(o.ID)).ToList();

                        }

                        //Lọc id dịch vụ
                        if (filter.SERVICE_IDs != null)
                        {
                            ListHisSereServLocal = ListHisSereServLocal.Where(o => filter.SERVICE_IDs.Contains(o.SERVICE_ID)).ToList();
                            //lọc chi tiết thanh toán liên quan
                            var ssId = ListHisSereServLocal.Select(o => o.ID).ToList();
                            ListHisSereServBillLocal = ListHisSereServBillLocal.Where(o => ssId.Contains(o.SERE_SERV_ID)).ToList();
                            //lọc giao dịch liên quan
                            var ssbId = ListHisSereServBillLocal.Select(o => o.BILL_ID).Distinct().ToList();
                            ListHisBillLocal = ListHisBillLocal.Where(o => ssbId.Contains(o.ID)).ToList();
                            //lọc hồ sơ điều trị liên quan
                            var treatIds = ListHisSereServLocal.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                            ListHisTreatmentLocal = ListHisTreatmentLocal.Where(o => treatIds.Contains(o.ID)).ToList();

                        }

                        //Lọc người chỉ định
                        if (filter.REQUEST_LOGINNAME != null)
                        {
                            ListHisSereServLocal = ListHisSereServLocal.Where(o => filter.REQUEST_LOGINNAME.Contains(o.TDL_REQUEST_LOGINNAME ?? "")).ToList();
                            //lọc chi tiết thanh toán liên quan
                            var ssId = ListHisSereServLocal.Select(o => o.ID).ToList();
                            ListHisSereServBillLocal = ListHisSereServBillLocal.Where(o => ssId.Contains(o.SERE_SERV_ID)).ToList();
                            //lọc giao dịch liên quan
                            var ssbId = ListHisSereServBillLocal.Select(o => o.BILL_ID).Distinct().ToList();
                            ListHisBillLocal = ListHisBillLocal.Where(o => ssbId.Contains(o.ID)).ToList();
                            //lọc hồ sơ điều trị liên quan
                            var treatIds = ListHisSereServLocal.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                            ListHisTreatmentLocal = ListHisTreatmentLocal.Where(o => treatIds.Contains(o.ID)).ToList();
                        }

                        //Lọc hình thức thanh toán
                        if (filter.PAY_FORM_IDs != null)
                        {
                            ListHisBillLocal = ListHisBillLocal.Where(o => filter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID)).ToList();
                            //lọc chi tiết thanh toán liên quan
                            var billId = ListHisBillLocal.Select(o => o.ID).ToList();
                            ListHisSereServBillLocal = ListHisSereServBillLocal.Where(o => billId.Contains(o.BILL_ID)).ToList();
                            //lọc dịch vụ liên quan
                            var ssId = ListHisSereServBillLocal.Select(o => o.SERE_SERV_ID).Distinct().ToList();
                            ListHisSereServLocal = ListHisSereServLocal.Where(o => ssId.Contains(o.ID)).ToList();
                            //lọc hồ sơ điều trị liên quan
                            var treatIds = ListHisSereServLocal.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                            ListHisTreatmentLocal = ListHisTreatmentLocal.Where(o => treatIds.Contains(o.ID)).ToList();
                        }

                        //Lọc chỉ lấy dịch vụ chỉ định ở buồng bệnh nội trú
                        if (filter.IS_NOT_FROM_BED == true)
                        {
                            var bedRoomIds = ListBedRoom.Select(o => o.ROOM_ID).ToList();
                            var treatmentTypeIdNts = ListHisTreatmentLocal.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p=>p.ID).ToList();
                            ListHisSereServLocal = ListHisSereServLocal.Where(o => !treatmentTypeIdNts.Contains(o.TDL_TREATMENT_ID??0) || !bedRoomIds.Contains(o.TDL_REQUEST_ROOM_ID)).ToList();
                            //lọc chi tiết thanh toán liên quan
                            var ssId = ListHisSereServLocal.Select(o => o.ID).ToList();
                            ListHisSereServBillLocal = ListHisSereServBillLocal.Where(o => ssId.Contains(o.SERE_SERV_ID)).ToList();
                            //lọc giao dịch liên quan
                            var ssbId = ListHisSereServBillLocal.Select(o => o.BILL_ID).Distinct().ToList();
                            ListHisBillLocal = ListHisBillLocal.Where(o => ssbId.Contains(o.ID)).ToList();
                            //lọc hồ sơ điều trị liên quan
                            var treatIds = ListHisSereServLocal.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                            ListHisTreatmentLocal = ListHisTreatmentLocal.Where(o => treatIds.Contains(o.ID)).ToList();
                        }
                        if (filter.IS_FROM_BED == true)
                        {
                            var bedRoomIds = ListBedRoom.Select(o => o.ROOM_ID).ToList();
                            var treatmentTypeIdNts = ListHisTreatmentLocal.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p => p.ID).ToList();
                            ListHisSereServLocal = ListHisSereServLocal.Where(o => treatmentTypeIdNts.Contains(o.TDL_TREATMENT_ID??0) && bedRoomIds.Contains(o.TDL_REQUEST_ROOM_ID)).ToList();
                            //lọc chi tiết thanh toán liên quan
                            var ssId = ListHisSereServLocal.Select(o => o.ID).ToList();
                            ListHisSereServBillLocal = ListHisSereServBillLocal.Where(o => ssId.Contains(o.SERE_SERV_ID)).ToList();
                            //lọc giao dịch liên quan
                            var ssbId = ListHisSereServBillLocal.Select(o => o.BILL_ID).Distinct().ToList();
                            ListHisBillLocal = ListHisBillLocal.Where(o => ssbId.Contains(o.ID)).ToList();
                            //lọc hồ sơ điều trị liên quan
                            var treatIds = ListHisSereServLocal.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                            ListHisTreatmentLocal = ListHisTreatmentLocal.Where(o => treatIds.Contains(o.ID)).ToList();

                        }

                        //Lọc theo diện thanh toán
                        if (filter.INPUT_DATA_ID_BILL_TREAT_TYPE != null)
                        {
                            //lọc giao dịch liên quan
                            Dictionary<long, long?> dicClinialInTime = ListHisTreatmentLocal.ToDictionary(o => o.ID, p => p.CLINICAL_IN_TIME);
                            ListHisBillLocal = ListHisBillLocal.Where(o => filter.INPUT_DATA_ID_BILL_TREAT_TYPE == 1 && !(dicClinialInTime.ContainsKey(o.TREATMENT_ID??0) && dicClinialInTime[o.TREATMENT_ID??0]<o.TRANSACTION_TIME)||filter.INPUT_DATA_ID_BILL_TREAT_TYPE == 2 && dicClinialInTime.ContainsKey(o.TREATMENT_ID??0) && dicClinialInTime[o.TREATMENT_ID??0]<o.TRANSACTION_TIME).ToList();

                            //lọc chi tiết thanh toán liên quan
                            var billId = ListHisBillLocal.Select(o => o.ID).ToList();
                            ListHisSereServBillLocal = ListHisSereServBillLocal.Where(o => billId.Contains(o.BILL_ID)).ToList();
                            var ssId = ListHisSereServBillLocal.Select(o => o.SERE_SERV_ID).ToList();
                            ListHisSereServLocal = ListHisSereServLocal.Where(o => ssId.Contains(o.ID)).ToList();

                            //lọc hồ sơ điều trị liên quan
                            var treatIds = ListHisSereServLocal.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                            ListHisTreatmentLocal = ListHisTreatmentLocal.Where(o => treatIds.Contains(o.ID)).ToList();
                        }    

                        //xử lý 
                        ProcessDataLocal(ListHisTreatmentLocal, ListHisSereServLocal, ListHisSereServBillLocal, ListHisBillLocal, ListHisPatientTypeAlter);
                    }

                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetBedRoom()
        {
            ListBedRoom = new HisBedRoomManager(paramGet).Get(new HisBedRoomFilterQuery());
        }

        private void GetService()
        {
            HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
            serviceFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            ListService = new HisServiceManager(paramGet).Get(serviceFilter);
            //danh sach dich vu cha
            DicParentCode = ListService.ToDictionary(o => o.ID, p => (ListService.FirstOrDefault(q => q.ID == p.PARENT_ID) ?? new HIS_SERVICE()).SERVICE_CODE ?? "NONE");
        }

        private void GetMetyMaty()
        {
            ListHisMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());

            ListHisMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
        }

        private void GetServiceRety()
        {
            HisServiceRetyCatViewFilterQuery ServiceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
            ServiceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00602";
            ListHisServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(ServiceRetyCatFilter);
        }

        private void ProcessDataLocal(List<HIS_TREATMENT> ListHisTreatmentLocal, List<HIS_SERE_SERV> ListHisSereServLocal, List<HIS_SERE_SERV_BILL> ListHisSereServBillLocal, List<HIS_TRANSACTION> ListHisTransactionLocal, List<HIS_PATIENT_TYPE_ALTER> ListHisPatientTypeAlter)
        {


            if (IsNotNullOrEmpty(ListHisTreatmentLocal))
            {
                foreach (var item in ListHisTreatmentLocal)
                {
                    Mrs00602RDOTreatment rdo = new Mrs00602RDOTreatment(item, ListHisSereServLocal, ListHisServiceRetyCat, ListHisTransactionLocal, ListHisSereServBillLocal, filter, ListHisPatientTypeAlter, DicParentCode);
                    ListRdoTreatment.Add(rdo);
                }
            }
            if (IsNotNullOrEmpty(ListHisSereServLocal))
            {
                foreach (var item in ListHisSereServLocal)
                {
                    var treatment = ListHisTreatmentLocal.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID);
                    if (treatment != null)
                    {
                        if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC)
                        {
                            Mrs00602RDOMedicine rdo = new Mrs00602RDOMedicine(item, treatment, ListHisMedicineType);
                            string key = string.Format("{0}_{1}_{2}", rdo.SERVICE_ID, rdo.TREAT_PATIENT_TYPE_ID, rdo.PRICE);
                            if (!dicRdoMedicine.ContainsKey(key))
                            {
                                dicRdoMedicine.Add(key, rdo);
                            }
                            else
                            {
                                dicRdoMedicine[key].AMOUNT_NOITRU += rdo.AMOUNT_NOITRU;
                                dicRdoMedicine[key].AMOUNT_NGOAITRU += rdo.AMOUNT_NGOAITRU;
                                dicRdoMedicine[key].TOTAL_PRICE += rdo.TOTAL_PRICE;
                            }
                        }
                        else if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT)
                        {
                            Mrs00602RDOMaterial rdo = new Mrs00602RDOMaterial(item, treatment, ListHisMaterialType);
                            string key = string.Format("{0}_{1}_{2}", rdo.SERVICE_ID, rdo.TREAT_PATIENT_TYPE_ID, rdo.PRICE);
                            if (!dicRdoMaterial.ContainsKey(key))
                            {
                                dicRdoMaterial.Add(key, rdo);
                            }
                            else
                            {
                                dicRdoMaterial[key].AMOUNT_NOITRU += rdo.AMOUNT_NOITRU;
                                dicRdoMaterial[key].AMOUNT_NGOAITRU += rdo.AMOUNT_NGOAITRU;
                                dicRdoMaterial[key].TOTAL_PRICE += rdo.TOTAL_PRICE;
                            }

                        }
                        else
                        {
                            Mrs00602RDOService rdo = new Mrs00602RDOService(item, ListHisServiceRetyCat, ListService, ListPtttGroup, this.branch_id, treatment, ListHisPatientTypeAlter);
                            if (filter.IS_SPLIT_ROUTE == true)
                            {
                                string key = string.Format("ROUTE_{0}_{1}_{2}_{3}", rdo.SERVICE_ID, rdo.TDL_HEIN_SERVICE_TYPE_ID ?? 0, rdo.ROUTE_CODE, rdo.PRICE);
                                if (!dicRdoRouteService.ContainsKey(key))
                                {
                                    dicRdoRouteService.Add(key, rdo);
                                }
                                else
                                {
                                    dicRdoRouteService[key].AMOUNT_NOITRU_ROUTE += rdo.AMOUNT_NOITRU_ROUTE;
                                    dicRdoRouteService[key].AMOUNT_NGOAITRU_ROUTE += rdo.AMOUNT_NGOAITRU_ROUTE;
                                    dicRdoRouteService[key].TOTAL_PRICE_ROUTE += rdo.TOTAL_PRICE_ROUTE;
                                }

                            }

                            {
                                string key = string.Format("{0}_{1}_{2}", rdo.SERVICE_ID, rdo.TREAT_PATIENT_TYPE_ID, rdo.PRICE);
                                if (!dicRdoService.ContainsKey(key))
                                {
                                    dicRdoService.Add(key, rdo);
                                }
                                else
                                {
                                    dicRdoService[key].AMOUNT_NOITRU += rdo.AMOUNT_NOITRU;
                                    dicRdoService[key].AMOUNT_NGOAITRU += rdo.AMOUNT_NGOAITRU;
                                    dicRdoService[key].TOTAL_PRICE += rdo.TOTAL_PRICE;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                //ProcessDataLocal(ListHisTreatmentLocal, ListHisSereServLocal, ListHisSereServBillLocal, ListHisTransactionLocal, ListHisPatientTypeAlter);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                //ListRdoTreatment.Clear();
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var InSide = ListRdoTreatment.Where(o => (o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY) && o.TOTAL_PRICE > 0).ToList();
            var OutSide = ListRdoTreatment.Where(o => o.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && o.TOTAL_PRICE > 0 && o.TREATMENT_TYPE_EXAM == false).ToList();

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.FEE_LOCK_TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.FEE_LOCK_TIME_TO));
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());
            dicSingleTag.Add("INSIDE_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(InSide.Sum(s => s.TOTAL_PRICE)).ToString()));
            dicSingleTag.Add("OUTSIDE_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(OutSide.Sum(s => s.TOTAL_PRICE)).ToString()));
            dicSingleTag.Add("SERV_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(dicRdoService.Values.Sum(s => s.TOTAL_PRICE)).ToString()));
            dicSingleTag.Add("SERV_PRICE_ROUTE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(dicRdoRouteService.Values.Sum(s => s.TOTAL_PRICE_ROUTE)).ToString()));
            dicSingleTag.Add("MEDI_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(dicRdoMedicine.Values.Sum(s => s.TOTAL_PRICE)).ToString()));
            dicSingleTag.Add("MATE_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(dicRdoMaterial.Values.Sum(s => s.TOTAL_PRICE)).ToString()));

            objectTag.AddObjectData(store,"PatientTypes", ListRdoTreatment);

            objectTag.SetUserFunction(store, "Element", new RDOElement());
            objectTag.AddObjectData(store, "ParentMaterial", dicRdoMaterial.Values.GroupBy(o =>  o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Material", dicRdoMaterial.Values.ToList());
            objectTag.AddRelationship(store, "ParentMaterial", "Material", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");

            objectTag.AddObjectData(store, "ParentMedicine", dicRdoMedicine.Values.GroupBy(o => o.TREAT_PATIENT_TYPE_ID ).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "Medicine", dicRdoMedicine.Values.ToList());
            objectTag.AddRelationship(store, "ParentMedicine", "Medicine", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");

            objectTag.AddObjectData(store, "ParentInSide", InSide.GroupBy(o =>  o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "InSide", InSide);
            objectTag.AddRelationship(store, "ParentInSide", "InSide", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");

            objectTag.AddObjectData(store, "ParentInSide1", InSide.Where(p => p.TOTAL_PATIENT_PRICE_SELF > 0).GroupBy(o => o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "InSide1", InSide.Where(p => p.TOTAL_PATIENT_PRICE_SELF > 0).ToList());
            objectTag.AddRelationship(store, "ParentInSide1", "InSide1", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");

            objectTag.AddObjectData(store, "ParentOutSide", OutSide.GroupBy(o => o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "OutSide", OutSide);
            objectTag.AddRelationship(store, "ParentOutSide", "OutSide", "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");


            objectTag.AddObjectData(store, "Service", dicRdoService.Values.GroupBy(o => string.Format("{0}_{1}", o.SERVICE_ID, o.PRICE)).Select(p => new Mrs00602RDOService()
            {
                SERVICE_CODE_DMBYT = p.First().SERVICE_CODE_DMBYT,
                SERVICE_TYPE_NAME = p.First().SERVICE_TYPE_NAME,
                PRICE = p.First().PRICE,
                AMOUNT_NGOAITRU = p.Sum(s=>s.AMOUNT_NGOAITRU),
                AMOUNT_NOITRU = p.Sum(s=>s.AMOUNT_NOITRU),
                TOTAL_PRICE = p.Sum(s=>s.TOTAL_PRICE)
            }).ToList());
            objectTag.AddObjectData(store, "ParentService", dicRdoService.Values.GroupBy(o => o.TREAT_PATIENT_TYPE_ID).Select(p => p.First()).ToList());

            var groupByServiceType = dicRdoService.Values.GroupBy(o => o.SERVICE_TYPE_CODE).ToDictionary(p => p.Key ?? "NONE", q => q.ToList<Mrs00602RDOService>());


            foreach (var item in HisServiceTypeCFG.HisServiceTypes)
            {
                if (groupByServiceType.ContainsKey(item.SERVICE_TYPE_CODE))
                {
                    string transactionServiceType = "Transaction" + item.SERVICE_TYPE_CODE;

                    objectTag.AddObjectData(store, item.SERVICE_TYPE_CODE, groupByServiceType[item.SERVICE_TYPE_CODE].OrderBy(P => P.SERVICE_TYPE_NAME).ToList());
                    objectTag.AddObjectData(store, transactionServiceType, groupByServiceType[item.SERVICE_TYPE_CODE].Where(p => !string.IsNullOrEmpty(p.TRANSACTION_CODE)).GroupBy(p => new { p.TRANSACTION_CODE, p.TREAT_PATIENT_TYPE_ID }).Select(p => p.First()).ToList());
                    objectTag.AddRelationship(store, "ParentService", item.SERVICE_TYPE_CODE, "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
                    objectTag.AddRelationship(store, transactionServiceType, item.SERVICE_TYPE_CODE, new string[] { "TRANSACTION_CODE", "TREAT_PATIENT_TYPE_ID" }, new string[] { "TRANSACTION_CODE", "TREAT_PATIENT_TYPE_ID" });
                }
                else
                {
                    string transactionServiceType = "Transaction" + item.SERVICE_TYPE_CODE;
                    objectTag.AddObjectData(store, item.SERVICE_TYPE_CODE, new List<Mrs00602RDOService>());
                    objectTag.AddObjectData(store, transactionServiceType, new List<Mrs00602RDOService>().GroupBy(p => new { p.TRANSACTION_CODE, p.TREAT_PATIENT_TYPE_ID }).Select(p => p.First()).ToList());
                    objectTag.AddRelationship(store, "ParentService", item.SERVICE_TYPE_CODE, "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
                    objectTag.AddRelationship(store, transactionServiceType, item.SERVICE_TYPE_CODE, new string[] { "TRANSACTION_CODE", "TREAT_PATIENT_TYPE_ID" }, new string[] { "TRANSACTION_CODE", "TREAT_PATIENT_TYPE_ID" });
                }
            }
            var groupByCategory = dicRdoService.Values.GroupBy(o => o.CATEGORY_CODE).ToDictionary(p => p.Key ?? "NONE", q => q.ToList<Mrs00602RDOService>());
            foreach (var item in ListHisServiceRetyCat.Select(o => o.CATEGORY_CODE).Distinct().ToList())
            {
                if (groupByCategory.ContainsKey(item))
                {
                    objectTag.AddObjectData(store, item, groupByCategory[item]);
                    objectTag.AddRelationship(store, "ParentService", item, "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
                }
                else
                {
                    objectTag.AddObjectData(store, item, new List<Mrs00602RDOService>());
                    objectTag.AddRelationship(store, "ParentService", item, "TREAT_PATIENT_TYPE_ID", "TREAT_PATIENT_TYPE_ID");
                }
            }
            AddGeneral(dicSingleTag, objectTag, store);
            AddA(dicSingleTag, objectTag, store);
            AddB(dicSingleTag, objectTag, store);

            MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SelectSheet;

        }

        private void AddA(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            objectTag.AddObjectData(store, "ReportA1", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "A" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH).ToList());
            objectTag.AddObjectData(store, "ReportA2", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "A" && (o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT)).ToList());
            objectTag.AddObjectData(store, "ReportA3", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "A" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN).ToList());
            objectTag.AddObjectData(store, "ReportA4", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "A" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA).ToList());
            objectTag.AddObjectData(store, "ReportA5", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "A" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT).ToList());
            objectTag.AddObjectData(store, "ReportA6", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "A" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU).ToList());
            objectTag.AddObjectData(store, "ReportA7", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "A" && o.TDL_HEIN_SERVICE_TYPE_ID == 0).ToList());
        }

        private void AddB(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            objectTag.AddObjectData(store, "ReportB1", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "B" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH).ToList());
            objectTag.AddObjectData(store, "ReportB2", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "B" && (o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT)).ToList());
            objectTag.AddObjectData(store, "ReportB3", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "B" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN).ToList());
            objectTag.AddObjectData(store, "ReportB4", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "B" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA).ToList());
            objectTag.AddObjectData(store, "ReportB5", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "B" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT).ToList());
            objectTag.AddObjectData(store, "ReportB6", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "B" && o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU).ToList());
            objectTag.AddObjectData(store, "ReportB7", dicRdoRouteService.Values.Where(o => o.ROUTE_CODE == "B" && o.TDL_HEIN_SERVICE_TYPE_ID == 0).ToList());
        }

        private void AddGeneral(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            objectTag.AddObjectData(store, "Report1", dicRdoRouteService.Values.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH).ToList());
            objectTag.AddObjectData(store, "Report2", dicRdoRouteService.Values.Where(o => (o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT)).ToList());
            objectTag.AddObjectData(store, "Report3", dicRdoRouteService.Values.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN).ToList());
            objectTag.AddObjectData(store, "Report4", dicRdoRouteService.Values.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA).ToList());
            objectTag.AddObjectData(store, "Report5", dicRdoRouteService.Values.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT).ToList());
            objectTag.AddObjectData(store, "Report6", dicRdoRouteService.Values.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU).ToList());
            objectTag.AddObjectData(store, "Report7", dicRdoRouteService.Values.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == 0).ToList());
        }

        private void SelectSheet(ref Inventec.Common.FlexCellExport.Store store, ref System.IO.MemoryStream resultStream)
        {

            resultStream.Position = 0;
            FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
            xls.Open(resultStream);
            try
            {

                if (!String.IsNullOrWhiteSpace(this.ReportTemplateCode))
                {
                    xls.ActiveSheetByName = this.ReportTemplateCode;
                }
                else
                {
                    xls.ActiveSheet = 1;
                    Inventec.Common.Logging.LogSystem.Error("Khong ton tai sheet co ten giong ma mau bao cao");
                }


                xls.Save(resultStream);
                //resultStream = result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                xls.ActiveSheet = 1;
            }
        }

    }
}
