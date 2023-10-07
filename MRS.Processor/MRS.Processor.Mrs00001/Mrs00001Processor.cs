using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisIcdGroup;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using Inventec.Common.FlexCellExport;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisExecuteRoom;
using System.Threading;
using MOS.MANAGER.HisExpMestMedicine;
using System.Data;
using MOS.MANAGER.HisTreatmentEndType;
using IcdVn;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Manager;
using SDA.MANAGER.Core.SdaDistrict.Get;
using SDA.MANAGER.Core.SdaProvince.Get;
using SDA.MANAGER.Core.SdaCommune.Get;
using MOS.MANAGER.HisMediOrg;

namespace MRS.Processor.Mrs00001
{
    public class Mrs00001Processor : AbstractProcessor
    {
        private Mrs00001Filter filter;
        List<Mrs00001RDO> listRdo = new List<Mrs00001RDO>();
        List<SERE_SERV> ListSereServ = new List<SERE_SERV>();
        List<SALE_MEDICINE> ListSaleExpMestMedicine = new List<SALE_MEDICINE>();
        System.Data.DataTable listData = new System.Data.DataTable();
        System.Data.DataTable listParent = new System.Data.DataTable();
        System.Data.DataTable listGrandParent = new System.Data.DataTable();
        System.Data.DataTable listIcdVn = new System.Data.DataTable();
        List<DEPARTMENT_IN> listDepartmentIn = new List<DEPARTMENT_IN>();
        List<HIS_PATIENT_CASE> ListPatientCase = new List<HIS_PATIENT_CASE>();
        List<HIS_TREATMENT_END_TYPE> ListTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<long> KccDepartmentIds = new List<long>();

        List<V_SDA_DISTRICT> listDistrict = new List<V_SDA_DISTRICT>();
        List<V_SDA_COMMUNE> listCommune = new List<V_SDA_COMMUNE>();
        List<V_SDA_PROVINCE> listProvince = new List<V_SDA_PROVINCE>();
        List<TREATMENT> listHisTreatmentIdRemove = new List<TREATMENT>();
        List<Mrs00001RDO> listRdoOfKCC_KKB = new List<Mrs00001RDO>();
        List<HIS_EXECUTE_ROOM> listExecuteRoom = new List<HIS_EXECUTE_ROOM>();

        Dictionary<string, HIS_MEDI_ORG> dicMediOrg = new Dictionary<string, HIS_MEDI_ORG>();

        Dictionary<long, HIS_SERVICE> dicService = new Dictionary<long, HIS_SERVICE>();


        CommonParam paramGet = new CommonParam();

        long patientTypeBhyt;

        long patientTypeFree;

        List<HIS_PATIENT_TYPE> listPatientType;

        internal const string TickSymbol = "x";

        internal const string SeparateItemInStringSymbol = ",";

        public Mrs00001Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00001Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00001Filter)reportFilter;

            try
            {

                FilterTimeType(filter);
                dicMediOrg = (new HisMediOrgManager().Get(new HisMediOrgFilterQuery()) ?? new List<HIS_MEDI_ORG>()).Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).GroupBy(o => o.MEDI_ORG_CODE).ToDictionary(p => p.Key, q => q.First());
                listProvince = new SdaProvinceManager(new CommonParam()).Get<List<V_SDA_PROVINCE>>(new SdaProvinceViewFilterQuery()) ?? new List<V_SDA_PROVINCE>();
                listDistrict = new SdaDistrictManager(new CommonParam()).Get<List<V_SDA_DISTRICT>>(new SdaDistrictViewFilterQuery()) ?? new List<V_SDA_DISTRICT>();
                listCommune = new SdaCommuneManager(new CommonParam()).Get<List<V_SDA_COMMUNE>>(new SdaCommuneViewFilterQuery()) ?? new List<V_SDA_COMMUNE>();
                List<HIS_ICD> listIcd = new HisIcdManager().Get(new HisIcdFilterQuery());
                listData = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15));
                listParent = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 16));
                listGrandParent = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 17));
                listIcdVn = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 18));
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0 && listParent != null && listParent.Rows != null && listParent.Rows.Count > 0 && listGrandParent.Rows != null && listGrandParent.Rows.Count > 0)
                {
                    return true;
                }

                patientTypeBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                LogSystem.Info("bhyt" + patientTypeBhyt);
                patientTypeFree = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;
                LogSystem.Info("free" + patientTypeFree);
                listPatientType = HisPatientTypeCFG.PATIENT_TYPEs ?? new List<HIS_PATIENT_TYPE>();

                //kham
                if (filter.ICD_IDs != null)
                {
                    listIcd = listIcd.Where(p => filter.ICD_IDs.Contains(p.ID)).ToList();
                }
                listRdo = new ManagerSql().GetServiceReq(filter, listProvince, listDistrict, listCommune, listIcd);

                Inventec.Common.Logging.LogSystem.Info("data service_req" + listRdo.Count);

                if (filter.DEPARTMENT_CODE__OUTPATIENTs != null)
                {
                    List<string> KCCDepartmentCodes = filter.DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
                    var KccDepartments = HisDepartmentCFG.DEPARTMENTs.Where(o => KCCDepartmentCodes.Contains(o.DEPARTMENT_CODE)).ToList() ?? new List<HIS_DEPARTMENT>();
                    KccDepartmentIds = KccDepartments.Select(o => o.ID).ToList();
                    //Bo cac ho so thoa man thoi gian bao cao nhung vao dieu tri ngoai tru, noi tru o khoa kham benh, phong cap cuu.
                    listHisTreatmentIdRemove = new ManagerSql().GetByIdRemove(filter, KccDepartmentIds, listProvince, listDistrict, listCommune, listIcd) ?? new List<TREATMENT>();

                    //Them cac ho so dieu tri noi tru, ngoai tru tai phong cap cuu, khoa kham benh ra khoi khoa trong thoi gian bao cao:
                    listRdoOfKCC_KKB = new ManagerSql().GetServiceReqOutOfKCC_KKB(filter, KccDepartmentIds, listProvince, listDistrict, listCommune, listIcd) ?? new List<Mrs00001RDO>();
                    if (listRdoOfKCC_KKB != null && listRdoOfKCC_KKB.Count > 0)
                    {
                        listRdo.AddRange(listRdoOfKCC_KKB);
                        ListSereServ.AddRange(new ManagerSql().GetSereServOfKCC_KKB(listRdoOfKCC_KKB, this.filter, listProvince, listDistrict, listCommune, listIcd));
                    }
                }


                //dv
                ListSereServ.AddRange(new ManagerSql().GetSereServ(filter, listProvince, listDistrict, listCommune, listIcd));
                ListSereServ = ListSereServ.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                Inventec.Common.Logging.LogSystem.Info("ListSereServ" + ListSereServ.Count);

                //Gộp dịch vụ khám theo phòng thực hiện
                listRdo = listRdo.OrderBy(q => q.INTRUCTION_TIME).GroupBy(o => new { o.TREATMENT_ID, o.EXECUTE_ROOM_ID }).Select(p => p.First()).ToList();
                Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);

                ListSaleExpMestMedicine = new ManagerSql().GetSaleExpMestMedicine(filter, listProvince, listDistrict, listCommune, listIcd) ?? new List<SALE_MEDICINE>();
                //Inventec.Common.Logging.LogSystem.Info("ListMisuServiceReq" + ListMisuServiceReq.Count);
                //lay thong tin khoa nhap vien
                listDepartmentIn = new ManagerSql().GetDepartmentIn(filter, listProvince, listDistrict, listCommune, listIcd) ?? new List<DEPARTMENT_IN>();

                string sql = "SELECT * FROM HIS_PATIENT_CASE WHERE IS_ACTIVE = 1";
                ListPatientCase = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CASE>(sql) ?? new List<HIS_PATIENT_CASE>();
                HisTreatmentEndTypeFilterQuery TreatmentEndTypeFilter = new HisTreatmentEndTypeFilterQuery();
                ListTreatmentEndType = new HisTreatmentEndTypeManager().Get(TreatmentEndTypeFilter);
                HisExecuteRoomFilterQuery ExecuteRoomFilter = new HisExecuteRoomFilterQuery();
                listExecuteRoom = new HisExecuteRoomManager().Get(ExecuteRoomFilter);

                //get danh sách dịch vụ
                dicService = GetService();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterTimeType( Mrs00001Filter filter)
        {
            filter.INTRUCTION_TIME_FROM = filter.INTRUCTION_TIME_FROM ?? filter.START_TIME_FROM ?? filter.FINISH_TIME_FROM ?? filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM ?? filter.FEE_LOCK_TIME_FROM;
            filter.INTRUCTION_TIME_TO = filter.INTRUCTION_TIME_TO ?? filter.START_TIME_TO ?? filter.FINISH_TIME_TO ?? filter.IN_TIME_TO ?? filter.OUT_TIME_TO ?? filter.FEE_LOCK_TIME_TO;
        }


        private Dictionary<long, HIS_SERVICE> GetService()
        {
            List<HIS_SERVICE> result = new List<HIS_SERVICE>();
            try
            {
                result = new HisServiceManager().Get(new HisServiceFilterQuery()) ?? new List<HIS_SERVICE>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<HIS_SERVICE>();
            }
            return result.GroupBy(o => o.ID).ToDictionary(p => p.Key, q => q.First());

        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0 && listParent != null && listParent.Rows != null && listParent.Rows.Count > 0 && listGrandParent.Rows != null && listGrandParent.Rows.Count > 0)
                {
                    return true;
                }
                if (IsNotNullOrEmpty(listRdo))
                {
                    List<Thread> listThread = new List<Thread>();
                    int count = (int)listRdo.Count / 4;
                    for (int i = 0; i <= 4; i++)
                    {
                        var listRdoSub = listRdo.Skip(count * i).Take(i == 4 ? i : count).ToList();
                        Thread thread = new Thread(processorRdo);
                        thread.Start(listRdoSub);
                        listThread.Add(thread);
                    }
                    foreach (var item in listThread)
                    {
                        item.Join();
                    }
                    listRdo = listRdo.Where(o => o.HAS_COUNT == true).ToList();
                    Inventec.Common.Logging.LogSystem.Info("listRdoPrint" + listRdo.Count);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        //private bool IsExam(long requestRoomId)
        //{
        //    return dicRoom.ContainsKey(requestRoomId) && dicRoom[requestRoomId].IS_EXAM == 1;
        //}

        private void processorRdo(object listRdoSub)
        {
            try
            {
                var groupByTreatment = (listRdoSub as List<Mrs00001RDO>).GroupBy(o => o.TREATMENT_ID).ToList();

                foreach (var group in groupByTreatment)
                {
                    var sereServs = ListSereServ.Where(o => o.TDL_TREATMENT_ID == group.Key).ToList();
                    var saleExpMestMedicines = ListSaleExpMestMedicine.Where(o => o.TREATMENT_ID == group.Key).ToList();
                    if (filter.DEPARTMENT_CODE__OUTPATIENTs != null)
                    {
                        var treatmentIdRemove = listHisTreatmentIdRemove.FirstOrDefault(o => o.ID == group.Key);
                        var treatmentIdAdd = listRdoOfKCC_KKB.FirstOrDefault(o => o.TDL_TREATMENT_CODE == group.First().TDL_TREATMENT_CODE);
                        if (treatmentIdRemove != null && treatmentIdAdd == null)
                        {
                            continue;
                        }
                    }
                    //thong tin gan theo ho so dieu tri
                    List<string> Codes = new List<string>();
                    if (group.First() != null && group.First().ICD_SUB_CODE != null)
                    {
                        Codes = group.First().ICD_SUB_CODE.Split(';').Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
                    }

                    List<string> Names = new List<string>();
                    if (group.First() != null && group.First().ICD_TEXT != null)
                    {
                        Names = group.First().ICD_TEXT.Split(';').Where(o => !string.IsNullOrWhiteSpace(o)).ToList();
                    }


                    ///
                    foreach (var item in group)
                    {
                        var sereServOfRooms = sereServs.Where(o => item.EXECUTE_ROOM_ID == o.TDL_REQUEST_ROOM_ID).ToList();
                        var saleMedicineOfRooms = saleExpMestMedicines.Where(o => item.EXECUTE_ROOM_ID == o.REQUEST_ROOM_ID).ToList();
                        var endRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.END_ROOM_ID && o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                        //Nếu tích gộp các phòng khám sẽ lấy lượt khám ở phòng xử lí cuối cùng, thông tin bác sĩ khám, ICD và chỉ định điều trị ở các phòng lại với nhau

                        if (filter.IS_MERGE_EXAM_ROOM == true)
                        {
                            //chi lay cac y lenh hoan thanh
                            if (item.SERVICE_REQ_STT_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                            {
                                continue;
                            }

                            //benh nhan nhap vien thi chi lay y lenh kham o phong nhap vien
                            if (item.IN_ROOM_ID != item.EXECUTE_ROOM_ID && (item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU))
                            {
                                // neu khong co thong tin phong nhap vien thi kiem tra thong tin phong kham dau tien
                                if (item.IN_ROOM_ID == null && item.TDL_FIRST_EXAM_ROOM_ID != item.EXECUTE_ROOM_ID)
                                    continue;
                            }
                            //benh nhan kham da ket thuc dieu tri thi chi lay y lenh kham o phong ket thuc dieu tri
                            if (item.END_ROOM_ID != item.EXECUTE_ROOM_ID && item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && item.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            {
                                //neu phong ket thuc la phong tiep don thi lay phong dau tien

                                if (endRoom != null && item.EXECUTE_ROOM_ID != item.TDL_FIRST_EXAM_ROOM_ID)
                                    continue;
                            }
                            if (group.ToList().Exists(o => o.HAS_COUNT == true))
                            {
                                continue;
                            }
                            item.ICD_CODE = string.Join("/", group.Select(p => (p.ICD_CODE ?? "") + ": " + (p.ICD_NAME ?? "")).Distinct().ToList());
                            item.ICD_NAME = "";
                            item.ICD_SUB_CODE = item.ICD_SUB_CODE;
                            item.ICD_TEXT = item.ICD_TEXT;
                            item.EXECUTE_USERNAME = string.Join(";", group.Where(o => o.EXECUTE_USERNAME != null).Select(p => (p.EXECUTE_USERNAME)).Distinct().ToList());
                            if (item.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                sereServOfRooms = sereServs.Where(o => IsExam(o.TDL_REQUEST_ROOM_ID)).ToList();
                                saleMedicineOfRooms = saleExpMestMedicines.Where(o => IsExam(o.REQUEST_ROOM_ID)).ToList();
                            }
                            else
                            {
                                sereServOfRooms = sereServs.Where(o => o.SERVICE_REQ_ID != item.ID).ToList();
                                saleMedicineOfRooms = saleExpMestMedicines;
                            }
                        }

                        ProcessSereServ(item, sereServOfRooms);
                        item.THUOC_BAN = ProcessSaleMedicine(saleMedicineOfRooms);
                        //thong tin gan theo y lenh kham
                        ProcessAddInfor(item, sereServs, patientTypeBhyt, patientTypeFree, listPatientType, filter, ListPatientCase, ListTreatmentEndType, listDepartmentIn, Codes, Names);
                        item.HAS_COUNT = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessAddInfor(Mrs00001RDO item, List<SERE_SERV> sereServs, long patientTypeBhyt, long patientTypeFree, List<HIS_PATIENT_TYPE> listPatientType, Mrs00001Filter filter, List<HIS_PATIENT_CASE> listPatientCase, List<HIS_TREATMENT_END_TYPE> listTreatmentEndType, List<DEPARTMENT_IN> listDepartmentIn, List<string> Codes, List<string> Names)
        {
            try
            {
                //cấp độ nơi đăng ký ban đầu
                var mediOrg = dicMediOrg.ContainsKey(item.TDL_HEIN_MEDI_ORG_CODE ?? "") ? dicMediOrg[item.TDL_HEIN_MEDI_ORG_CODE ?? ""] : null;
                if (mediOrg != null)
                {
                    item.TDL_HEIN_MEDI_ORG_GRADE = mediOrg.PROVINCE_CODE != null && mediOrg.PROVINCE_CODE.Length > 2 ? mediOrg.PROVINCE_CODE.Substring(2) : "";
                }
                item.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                if (item.PATIENT_CASE_ID.HasValue)
                {
                    var paCase = listPatientCase.FirstOrDefault(o => o.ID == item.PATIENT_CASE_ID);
                    if (paCase != null)
                    {
                        item.PATIENT_CASE_NAME = paCase.PATIENT_CASE_NAME;
                    }
                }

                item.VIR_ADDRESS = item.TDL_PATIENT_ADDRESS;

                item.PATIENT_TYPE_NAME = (listPatientType.FirstOrDefault(o => o.ID == item.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                var departmentIn = listDepartmentIn.FirstOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID);
                if (departmentIn != null)
                {
                    item.DEPARTMENT_IN_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentIn.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                }
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM();
                var exroom = listExecuteRoom.FirstOrDefault(o => o.ROOM_ID == item.EXECUTE_ROOM_ID) ?? new HIS_EXECUTE_ROOM();
                item.EXECUTE_ROOM_NAME = room.ROOM_NAME;
                item.EXECUTE_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                this.TickPatientType(item, patientTypeBhyt, patientTypeFree);
                this.ProcessHeinCard(item, item.TDL_HEIN_CARD_NUMBER, item.TDL_HEIN_MEDI_ORG_NAME, patientTypeBhyt, patientTypeFree);
                this.TickEmergency(item, exroom.IS_EMERGENCY);
                this.TickSpeciality(item, exroom.IS_SPECIALITY);

                this.TickTreatmentEndType(item, listTreatmentEndType);
                this.TickChild(item);
                long? tranPatiFormId = 0;
                tranPatiFormId = item.TRAN_PATI_FORM_ID;
                if (item.END_ROOM_ID == item.EXECUTE_ROOM_ID)
                {
                    this.TickTranPatiForm(item, tranPatiFormId);
                }

                this.TickTreatmentType(item);

                this.Weekday(item);

                item.ICD_NAME_TEXT = ProcessIcdText(item);
                item.ICD_SUB = ProcessIcdSub(item, Codes, Names);
                item.TRANSFER_IN_ICD_CODE = ProcessTransferInIcdCode(item);
                item.TRANSFER_IN_ICD_NAME = ProcessTransferInIcdName(item);
                item.ICD_NAMEs = (!String.IsNullOrWhiteSpace(item.ICD_NAME) ? item.ICD_NAME : item.ICD_TEXT);
                item.ICDVN_CODE = ProcessIcdVn(item.ICD_CODE);
                item.ICDVN_CAUSE_CODE = ProcessIcdVn(item.ICD_CAUSE_CODE);
                item.FILTER_DATE = item.FILTER_TIME - item.FILTER_TIME % 1000000;
                //thêm tên dịch vụ khám
                if (!string.IsNullOrWhiteSpace(item.TDL_SERVICE_IDS))
                {

                    string[] sv = item.TDL_SERVICE_IDS.Split(',');

                    long svId = 0;
                    if (sv.Length == 1)
                    {
                        long.TryParse(sv[0], out svId);
                    }
                    if (dicService.ContainsKey(svId))
                    {
                        item.EXAM_SERVICE_CODE = dicService[svId].SERVICE_CODE;
                        item.EXAM_SERVICE_NAME = dicService[svId].SERVICE_NAME;
                    }
                }
                if (item.START_TIME.HasValue && item.FINISH_TIME.HasValue)
                {
                    item.EXECUTE_TIME = Inventec.Common.DateTime.Calculation.DifferenceTime(item.START_TIME.Value, item.FINISH_TIME.Value, Inventec.Common.DateTime.Calculation.UnitDifferenceTime.MINUTE);
                }
                else if (item.FINISH_TIME.HasValue && !item.START_TIME.HasValue)
                {
                    LogSystem.Error("Yeu cau kham co thoi gian ket thuc nhung khong co thoi gian bat dau.Id=" + item.ID + ".");
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private bool IsExam(long requestRoomId)
        {
            return HisRoomCFG.HisRooms.Exists(o => o.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.ID == requestRoomId);
        }

        private string ProcessIcdVn(string IcdCode)
        {
            try
            {
                return new IcdVnIcd(IcdCode).ICDVN_CODE;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

        private void TickPatientType(Mrs00001RDO item, long patientTypeIdBhyt, long patientTypeFree)
        {
            try
            {
                if (item.TDL_PATIENT_TYPE_ID != null)
                {
                    if (item.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                    {
                        item.PATIENT_TYPE_GROUP_HEIN_TICK = TickSymbol;
                    }
                    else if (patientTypeFree == item.TDL_PATIENT_TYPE_ID)
                    {
                        item.PATIENT_TYPE_GROUP_FREE_TICK = TickSymbol;
                    }
                    else
                    {
                        item.PATIENT_TYPE_GROUP_SERVICE_TICK = TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void TickEmergency(Mrs00001RDO item, short? isEmergency)
        {
            try
            {
                if (isEmergency.HasValue && isEmergency.Value == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)// IMSys.DbConfig.HIS_RS.HIS_EXECUTE_ROOM.IS_EMERGENCY__TRUE)
                {
                    item.IS_EMERGENCY_TICK = TickSymbol;

                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void TickSpeciality(Mrs00001RDO item, short? isSpeciality)
        {
            try
            {
                if (isSpeciality.HasValue && isSpeciality.Value == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)// IMSys.DbConfig.HIS_RS.HIS_EXECUTE_ROOM.IS_SPECIALITY__TRUE)
                {
                    item.IS_SPECIALITY_TICK = TickSymbol;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void TickTreatmentEndType(Mrs00001RDO item, List<HIS_TREATMENT_END_TYPE> listTreatmentEndType)
        {
            try
            {
                if (item != null)
                {
                    if (listTreatmentEndType != null)
                    {
                        var treatmentEndType = listTreatmentEndType.FirstOrDefault(o => o.ID == item.TREATMENT_END_TYPE_ID);
                        if (treatmentEndType != null)
                        {
                            item.TREATMENT_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME;
                            item.TREATMENT_END_TYPE_CODE = treatmentEndType.TREATMENT_END_TYPE_CODE;
                        }
                    }
                    if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV == item.TREATMENT_END_TYPE_ID)
                    {
                        item.EXSR_FINISH_TYPE_HOME_TICK = TickSymbol;
                    }
                    if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN == item.TREATMENT_END_TYPE_ID)
                    {
                        item.APPOINTMENT_TICK = TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void TickTranPatiForm(Mrs00001RDO item, long? tranPatiFormId)
        {
            try
            {
                if (tranPatiFormId.HasValue && tranPatiFormId > 0)
                {
                    item.EXSR_FINISH_TYPE_TRANSPORT_TICK = TickSymbol;
                    item.NEXT_MEDI_ORG_CODE = item.MEDI_ORG_CODE;
                    item.NEXT_MEDI_ORG_NAME = item.MEDI_ORG_NAME;
                    if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__EQUAL == tranPatiFormId.Value)
                    {
                        item.EXSR_FINISH_TYPE_TRANSPORT_EQUAL_TICK = TickSymbol;
                    }
                    else if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__UP_DOWN == tranPatiFormId.Value)
                    {
                        item.EXSR_FINISH_TYPE_TRANSPORT_DOWN_TICK = TickSymbol;
                    }
                    else if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT == tranPatiFormId.Value || MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT == tranPatiFormId.Value)
                    {
                        item.EXSR_FINISH_TYPE_TRANSPORT_UP_TICK = TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        //benh nhan kham nhieu phong cho nhap vien dtri phong nao thi tick hien thi tai phong do
        private void TickTreatmentType(Mrs00001RDO item)
        {
            try
            {
                if (item.TDL_TREATMENT_TYPE_ID.HasValue && item.TDL_TREATMENT_TYPE_ID > 0)
                {
                    if (IsRequestIn(item, item.TDL_TREATMENT_TYPE_ID, item.CLINICAL_IN_TIME, item.IN_ROOM_ID))
                    {
                        item.EXSR_FINISH_TYPE_HOSPITALIZED_IN_TICK = TickSymbol;
                    }
                    else if (IsRequestOut(item, item.TDL_TREATMENT_TYPE_ID, item.CLINICAL_IN_TIME, item.IN_ROOM_ID))
                    {
                        item.EXSR_FINISH_TYPE_HOSPITALIZED_OUT_TICK = TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }


        private void Weekday(Mrs00001RDO item)
        {
            try
            {
                var dayOfWeek = (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.FILTER_TIME) ?? new DateTime()).DayOfWeek;
                item.WEEK_DAY = dayOfWeek == DayOfWeek.Sunday ? 1 : dayOfWeek == DayOfWeek.Monday ? 2 : dayOfWeek == DayOfWeek.Tuesday ? 3 : dayOfWeek == DayOfWeek.Wednesday ? 4 : dayOfWeek == DayOfWeek.Thursday ? 5 : dayOfWeek == DayOfWeek.Friday ? 6 : dayOfWeek == DayOfWeek.Saturday ? 7 : 0;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private bool IsRequestIn(Mrs00001RDO item, long? treatmentTypeId, long? clinicalInTime, long? inRoomId)
        {
            if (treatmentTypeId.HasValue && treatmentTypeId > 0)
            {
                return IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU == treatmentTypeId.Value && clinicalInTime != null && inRoomId == item.EXECUTE_ROOM_ID;
            }
            else
            {
                return false;
            }
        }

        private bool IsRequestOut(Mrs00001RDO item, long? treatmentTypeId, long? clinicalInTime, long? inRoomId)
        {
            if (treatmentTypeId.HasValue && treatmentTypeId > 0)
            {
                return IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU == treatmentTypeId.Value && clinicalInTime != null && inRoomId == item.EXECUTE_ROOM_ID;
            }
            else
            {
                return false;
            }
        }

        private string ProcessIcdText(Mrs00001RDO item)
        {
            string icdText = String.Empty;
            try
            {
                icdText = item.TRANSFER_IN_ICD_NAME;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return icdText;
        }

        private string ProcessIcdSub(Mrs00001RDO item, List<string> Codes, List<string> Names)
        {
            string icdText = String.Empty;
            try
            {
                if (Codes.Count == Names.Count)
                {
                    for (int i = 0; i < Codes.Count; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(Codes[i]))
                        {
                            Codes[i] = Codes[i] + ": " + Names[i];
                        }
                    }
                    icdText = string.Join("/", Codes.Distinct().ToList());
                }
                else
                {
                    icdText = (item.ICD_SUB_CODE ?? "") + ":" + (item.ICD_TEXT ?? "");
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return icdText;
        }

        private string ProcessTransferInIcdName(Mrs00001RDO item)
        {
            string icdText = String.Empty;
            try
            {
                icdText = item.TRANSFER_IN_ICD_NAME;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return icdText;
        }

        private string ProcessTransferInIcdCode(Mrs00001RDO item)
        {
            string icdText = String.Empty;
            try
            {
                icdText = item.TRANSFER_IN_ICD_CODE;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return icdText;
        }

        private void ProcessHeinCard(Mrs00001RDO item, string HeinCardNumber, string HeinMediOrgName, long patientTypeBhyt, long patientTypeFree)
        {
            try
            {
                if (item.TDL_PATIENT_TYPE_ID == patientTypeBhyt)
                {
                    item.IS_BHYT = "X";

                    if (HeinCardNumber != null)
                    {
                        item.HEIN_CARD_NUMBER = GenerateHeinCardSeparate(HeinCardNumber);
                        item.MEDI_ORG_NAME = HeinMediOrgName;
                    }
                }
                else
                {
                    item.IS_NOBHYT = "X";
                    if (item.TDL_PATIENT_TYPE_ID == patientTypeFree)
                    {
                        item.IS_FREE = "X";
                    }
                    else
                    {
                        item.IS_THUPHI = "X";
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private void TickChild(Mrs00001RDO item)
        {
            try
            {
                if (item.TDL_PATIENT_DOB != null)
                {
                    var tuoi = Inventec.Common.DateTime.Calculation.Age(item.TDL_PATIENT_DOB ?? 0);
                    if (tuoi < 16)
                    {
                        item.IS_CHILD = TickSymbol;
                        if (tuoi < 6)
                        {
                            item.IS_CHILD_6 = TickSymbol;
                        }
                        if (item.TDL_HEIN_CARD_NUMBER != null)
                        {
                            if (item.TDL_HEIN_CARD_NUMBER.Substring(0, 2) == "TE")
                            {
                                item.IS_CHILD_BHYT = TickSymbol;
                                if (tuoi < 6)
                                {
                                    item.IS_CHILD_6_BHYT = TickSymbol;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        public static string GenerateHeinCardSeparate(string heinCardNumber)
        {
            string result = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(heinCardNumber) && heinCardNumber.Length == 15)
                {
                    string separateSymbol = "-";
                    result = new StringBuilder().Append(heinCardNumber.Substring(0, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(2, 1)).Append(separateSymbol).Append(heinCardNumber.Substring(3, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(5, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(7, 3)).Append(separateSymbol).Append(heinCardNumber.Substring(10, 5)).ToString();
                }
                else
                {
                    result = heinCardNumber;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = heinCardNumber;
            }
            return result;
        }

        private void ProcessSereServ(Mrs00001RDO item, List<SERE_SERV> sereServ)
        {
            if (sereServ != null)
            {
                var misuSereServ = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList();
                if (misuSereServ.Count > 0)
                {
                    item.HAS_MISU_SERVICE_REQ_TICK = TickSymbol;
                    item.MISU_SERVICE_TYPE_NAMEs = string.Join(";", misuSereServ.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME + ": " + o.Sum(s => s.AMOUNT)).ToList());
                }

                var mediSereServ = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                if (mediSereServ.Count > 0)
                {
                    item.MEDICINE_TYPE_NAMEs = string.Join(";", mediSereServ.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME + ": " + o.Sum(s => s.AMOUNT) + " ").ToList());
                }

                item.AN = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN);
                item.CDHA = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA);
                item.G = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                item.GPBL = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL);
                item.THUOC = StrMediMate(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC);
                item.VT = StrMediMate(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT);
                item.KHAC = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC);
                item.MAU = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU);
                item.NS = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS);
                item.PHCN = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN);
                item.PT = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                item.SA = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA);
                item.TDCN = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN);
                item.TT = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                item.XN = StrService(sereServ, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN);

                var serviceNames = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).ToList();
                if (serviceNames.Count > 0)
                {
                    item.SERVICE_NAMEs = string.Join(";", serviceNames.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME + ": " + o.Sum(s => s.AMOUNT)).ToList());
                }
            }
        }

        private string StrService(List<SERE_SERV> sereServ, long serviceType)
        {
            string result = "";
            try
            {
                var sss = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == serviceType).ToList();
                if (sss.Count > 0)
                {
                    result = string.Join(";", sss.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME).ToList()) + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string StrMediMate(List<SERE_SERV> sereServ, long serviceType)
        {
            string result = "";
            try
            {
                var sss = sereServ.Where(o => o.TDL_SERVICE_TYPE_ID == serviceType).ToList();
                if (sss.Count > 0)
                {
                    result = string.Join(";", sss.GroupBy(q => q.SERVICE_ID).Select(o => o.First().TDL_SERVICE_NAME + ": " + o.Sum(s => s.AMOUNT)).ToList()) + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        private string ProcessSaleMedicine(List<SALE_MEDICINE> saleMedicine)
        {
            string result = "";
            try
            {
                if (saleMedicine != null && saleMedicine.Count > 0)
                {
                    result = string.Join(";", saleMedicine.GroupBy(q => q.MEDICINE_TYPE_ID).Select(o => o.First().MEDICINE_TYPE_NAME + ": " + o.Sum(s => s.AMOUNT)).ToList()) + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((filter.IN_TIME_FROM ?? filter.FEE_LOCK_TIME_FROM ??filter.INTRUCTION_TIME_FROM ??filter.FINISH_TIME_FROM ??filter.OUT_TIME_FROM??0)));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((filter.IN_TIME_TO ?? filter.FEE_LOCK_TIME_TO ?? filter.INTRUCTION_TIME_TO ?? filter.FINISH_TIME_TO ?? filter.OUT_TIME_TO ?? 0)));

            if (this.filter.EXAM_ROOM_ID != null)
            {
                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.filter.EXAM_ROOM_ID) ?? new V_HIS_ROOM();
                dicSingleTag.Add("ROOM_NAME", room.ROOM_NAME);
                dicSingleTag.Add("DEPARTMENT_NAME", room.DEPARTMENT_NAME);
            }
            dicSingleTag.Add("CURRENT_MEDI_ORG_CODE", (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == branch_id) ?? new HIS_BRANCH()).HEIN_MEDI_ORG_CODE);
            dicSingleTag.Add("CURRENT_PROVINCE_CODE", (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == branch_id) ?? new HIS_BRANCH()).PROVINCE_CODE);
            objectTag.AddObjectData(store, "IcdVn", listIcdVn ?? new DataTable());

            if (listData != null && listData.Rows != null && listData.Rows.Count > 0 && listParent != null && listParent.Rows != null && listParent.Rows.Count > 0 && listGrandParent.Rows != null && listGrandParent.Rows.Count > 0)
            {
                objectTag.AddObjectData(store, "Report", listData);
                objectTag.AddObjectData(store, "ParentReport", listParent);
                objectTag.AddRelationship(store, "ParentReport", "Report", new string[] { "EXECUTE_ROOM_ID", "OUT_DATE" }, new string[] { "EXECUTE_ROOM_ID", "OUT_DATE" });
                objectTag.AddObjectData(store, "GrandParentReport", listGrandParent);
                objectTag.AddRelationship(store, "GrandParentReport", "ParentReport", "EXECUTE_ROOM_ID", "EXECUTE_ROOM_ID");
                return;
            }

            if (filter.IS_FINISH == true)
            {
                listRdo = listRdo.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList();
            }
            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "Parent", listRdo.GroupBy(o => o.EXECUTE_ROOM_ID).Select(p => p.FirstOrDefault() ?? new Mrs00001RDO()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "EXECUTE_ROOM_ID", "EXECUTE_ROOM_ID");
            List<Mrs00001RDO> grandParent = null;
            try
            {
                grandParent = listRdo.GroupBy(o => o.EXECUTE_ROOM_ID).Select(p => p.FirstOrDefault() ?? new Mrs00001RDO()).ToList();
            }
            catch (Exception)
            { }

            objectTag.AddObjectData(store, "GrandParentReport", grandParent ?? new List<Mrs00001RDO>());
            objectTag.AddObjectData(store, "ParentReport", listRdo.OrderBy(p => p.INTRUCTION_DATE).GroupBy(o => new { o.EXECUTE_ROOM_ID, o.INTRUCTION_DATE }).Select(p => p.FirstOrDefault() ?? new Mrs00001RDO()).ToList());
            objectTag.AddRelationship(store, "GrandParentReport", "ParentReport", "EXECUTE_ROOM_ID", "EXECUTE_ROOM_ID");
            objectTag.AddRelationship(store, "ParentReport", "Report", new string[] { "EXECUTE_ROOM_ID", "INTRUCTION_DATE" }, new string[] { "EXECUTE_ROOM_ID", "INTRUCTION_DATE" });
            objectTag.AddObjectData(store, "ListSereServ", ListSereServ);
        }
    }
}
