using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisService;
using Inventec.Common.FlexCellExport;
using Inventec.Common.DateTime;
using Inventec.Common.Number;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MRS.Processor.Mrs00720;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Logging;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;

namespace MRS.Processor.Mrs00720
{
    public class Mrs00720Processor : AbstractProcessor
    {
        Mrs00720Filter castFilter = null;
        List<Mrs00720RDO> ListRdoTransacNoCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTransacCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoSereServNoCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoSereServCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTreatmentNoCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTreatmentCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTranNoKTCNoCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTranKTCCancel = new List<Mrs00720RDO>();

        List<Mrs00720RDO> ListRdoTranNoKTCCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTranKTCNoCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTreaDepaNoCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTreaDepaCancel = new List<Mrs00720RDO>();

        List<Mrs00720RDO> ListRdoTreaRoomNoCancel = new List<Mrs00720RDO>();
        List<Mrs00720RDO> ListRdoTreaRoomCancel = new List<Mrs00720RDO>();

        List<HIS_TRANSACTION> listTransactionNoCancel = new List<HIS_TRANSACTION>();
        List<V_HIS_TREATMENT_FEE> ListTreatmentFeeNoCancel = new List<V_HIS_TREATMENT_FEE>();
        List<ManagerSql.SERESERV> ListSereServNoCancel = new List<ManagerSql.SERESERV>();
        List<ManagerSql.SERESERV> ListBillGoodsNoCancel = new List<ManagerSql.SERESERV>();
        List<HIS_TRANSACTION> listTransactionCancel = new List<HIS_TRANSACTION>();
        List<V_HIS_TREATMENT_FEE> ListTreatmentFeeCancel = new List<V_HIS_TREATMENT_FEE>();
        List<ManagerSql.SERESERV> ListSereServCancel = new List<ManagerSql.SERESERV>();
        List<ManagerSql.SERESERV> ListBillGoodsCancel = new List<ManagerSql.SERESERV>();

        List<HIS_ACCOUNT_BOOK> listAccountBook = new List<HIS_ACCOUNT_BOOK>();

        Dictionary<long, HIS_SERVICE> dicSv = new Dictionary<long, HIS_SERVICE>();
        Dictionary<long, HIS_SERVICE> dicPar = new Dictionary<long, HIS_SERVICE>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicCate = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        List<HIS_PATIENT_CLASSIFY> listClassify = new List<HIS_PATIENT_CLASSIFY>();
        List<HIS_CASHIER_ROOM> listCashierRoom = new List<HIS_CASHIER_ROOM>();

        Dictionary<long, HIS_NONE_MEDI_SERVICE> dicNoneMediSv = new Dictionary<long, HIS_NONE_MEDI_SERVICE>();


        //thông tin lô thuốc, vật tư
        Dictionary<long, HIS_MEDICINE> dicMedicine = new Dictionary<long, HIS_MEDICINE>();
        Dictionary<long, HIS_MATERIAL> dicMaterial = new Dictionary<long, HIS_MATERIAL>();

        //Danh sách hồ sơ không phải thanh toán
        List<V_HIS_TREATMENT_FEE> ListTreatmentFeeNoBill = new List<V_HIS_TREATMENT_FEE>();
        List<ManagerSql.SERESERV> ListSereServNoBill = new List<ManagerSql.SERESERV>();

        CommonParam paramGet = new CommonParam();
        string title = "";

        public Mrs00720Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00720Filter);
        }

        protected override bool GetData()
        {

            var result = true;
            castFilter = (Mrs00720Filter)reportFilter;
            try
            {
                bool isCancel = false;
                listTransactionNoCancel = new MRS.Processor.Mrs00720.ManagerSql().GetTransaction(castFilter, isCancel);
                ListTreatmentFeeNoCancel = new MRS.Processor.Mrs00720.ManagerSql().GetTreatmentFee(castFilter, isCancel);
                ListSereServNoCancel = new MRS.Processor.Mrs00720.ManagerSql().GetDataSereServ(castFilter, isCancel);
                ListBillGoodsNoCancel = new MRS.Processor.Mrs00720.ManagerSql().GetDataBillGoods(castFilter, isCancel);

                //xử lý khi lọc các điều kiện sereServ cho transaction và treatment
                ProcessFilterSereServ(ref listTransactionNoCancel, ref ListTreatmentFeeNoCancel, ListSereServNoCancel, castFilter);

                ListTreatmentFeeNoBill = new MRS.Processor.Mrs00720.ManagerSql().GetTreatmentFeeNoBill(castFilter);
                ListSereServNoBill = new MRS.Processor.Mrs00720.ManagerSql().GetDataSereServNoBill(castFilter);

                //xử lý khi lọc các điều kiện sereServ cho transaction và treatment
                var listTransactionNoBill =  new List<HIS_TRANSACTION>();
                ProcessFilterSereServ(ref listTransactionNoBill, ref ListTreatmentFeeNoBill, ListSereServNoBill, castFilter);

                isCancel = true;
                listTransactionCancel = new MRS.Processor.Mrs00720.ManagerSql().GetTransaction(castFilter, isCancel);
                ListTreatmentFeeCancel = new MRS.Processor.Mrs00720.ManagerSql().GetTreatmentFee(castFilter, isCancel);
                ListSereServCancel = new MRS.Processor.Mrs00720.ManagerSql().GetDataSereServ(castFilter, isCancel);
                ListBillGoodsCancel = new MRS.Processor.Mrs00720.ManagerSql().GetDataBillGoods(castFilter, isCancel);

                //xử lý khi lọc các điều kiện sereServ cho transaction và treatment
                ProcessFilterSereServ(ref listTransactionCancel, ref ListTreatmentFeeCancel, ListSereServCancel, castFilter);


                //sổ thu chi
                GetAccountBook();

                //dịch vụ
                GetService();

                //nhóm báo cáo
                GetServiceRetyCat();

                //đối tượng chi tiết
                GetClassify();

                //phòng thu
                GetCashierRoom();

                //lô thuốc, vât tư
                GetMediMate();

                //dịch vụ khác
                GetNoneMediService();

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterSereServ(ref List<HIS_TRANSACTION> listTransaction, ref List<V_HIS_TREATMENT_FEE> ListTreatmentFee, List<ManagerSql.SERESERV> ListSereServ, Mrs00720Filter filter)
        {
            if (filter.SS_PATIENT_TYPE_IDs != null || filter.DEPARTMENT_ID != null || filter.DEPARTMENT_IDs != null || filter.ROOM_IDs != null)
            {
                if (ListSereServ == null)
                {
                    ListSereServ = new List<ManagerSql.SERESERV>();
                }
                var transactionIds = ListSereServ.Select(o => o.BILL_ID).Distinct().ToList();
                if (listTransaction == null)
                {
                    listTransaction = new List<HIS_TRANSACTION>();
                }
                listTransaction = listTransaction.Where(o => transactionIds.Contains(o.ID)).ToList();
                var treatmentIds = ListSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                if (ListTreatmentFee == null)
                {
                    ListTreatmentFee = new List<V_HIS_TREATMENT_FEE>();
                }
                ListTreatmentFee = ListTreatmentFee.Where(o => treatmentIds.Contains(o.ID)).ToList();
            }


        }

        private void GetCashierRoom()
        {
            string query = "select * from his_cashier_room";
            listCashierRoom = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_CASHIER_ROOM>(query);
        }

        private void GetMediMate()
        {
            try
            {
                List<long> medicineIds = new List<long>();
                if (ListSereServNoCancel != null)
                {
                    medicineIds.AddRange(ListSereServNoCancel.Select(o => o.MEDICINE_ID??0).ToList());
                }
                if (ListSereServCancel != null)
                {
                    medicineIds.AddRange(ListSereServCancel.Select(o => o.MEDICINE_ID??0).ToList());
                }

                medicineIds = medicineIds.Distinct().ToList();

                if (medicineIds != null && medicineIds.Count > 0)
                {
                    List<HIS_MEDICINE> Medicines = new List<HIS_MEDICINE>();
                    var skip = 0;
                    while (medicineIds.Count - skip > 0)
                    {
                        var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMedicineFilterQuery Medicinefilter = new HisMedicineFilterQuery();
                        Medicinefilter.IDs = limit;
                        var MedicineSub = new HisMedicineManager().Get(Medicinefilter);
                        Medicines.AddRange(MedicineSub);
                    }
                    dicMedicine = Medicines.ToDictionary(o => o.ID, o => o);
                }

                List<long> materialIds = new List<long>();
                if (ListSereServNoCancel != null)
                {
                    materialIds.AddRange(ListSereServNoCancel.Select(o => o.MATERIAL_ID??0).ToList());
                }
                if (ListSereServCancel != null)
                {
                    materialIds.AddRange(ListSereServCancel.Select(o => o.MATERIAL_ID??0).ToList());
                }
               
                materialIds = materialIds.Distinct().ToList();

                if (materialIds != null && materialIds.Count > 0)
                {
                    List<HIS_MATERIAL> Materials = new List<HIS_MATERIAL>();
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialFilterQuery Materialfilter = new HisMaterialFilterQuery();
                        Materialfilter.IDs = limit;
                        var MaterialSub = new HisMaterialManager().Get(Materialfilter);
                        Materials.AddRange(MaterialSub);
                    }
                    dicMaterial = Materials.ToDictionary(o => o.ID, o => o);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        private void GetClassify()
        {
            string query = "select * from his_patient_classify";
            listClassify = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>(query);
        }

        private void GetService()
        {
            var listService = new HisServiceManager().Get(new HisServiceFilterQuery());
            
            if (listService != null)
            {
                dicSv = listService.ToDictionary(o => o.ID, p => p);
                dicPar = listService.ToDictionary(o => o.ID, p => listService.FirstOrDefault(q => q.ID == p.PARENT_ID) ?? new HIS_SERVICE());
            }
        }

        private void GetServiceRetyCat()
        {
            HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
            serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = this.reportType.REPORT_TYPE_CODE;
            var serviceRetyCats = new HisServiceRetyCatManager(new CommonParam()).GetView(serviceRetyCatFilter);
            if (serviceRetyCats != null)
            {
                foreach (var item in serviceRetyCats)
                {
                    if (!dicCate.ContainsKey(item.SERVICE_ID))
                    {
                        dicCate.Add(item.SERVICE_ID, item);
                    }
                }
            }
        }

        private void GetAccountBook()
        {
            listAccountBook = new HisAccountBookManager().Get(new HisAccountBookFilterQuery()) ?? new List<HIS_ACCOUNT_BOOK>();
        }

        private void GetNoneMediService()
        {
            var listNoneMediService = new ManagerSql().GetNoneMediService();

            if (listNoneMediService != null)
            {
                dicNoneMediSv = listNoneMediService.ToDictionary(o => o.ID, p => p);
            }
        }

        protected override bool ProcessData()
        {
            var result = true;

            try
            {
                var listRepayNoCancel = listTransactionNoCancel.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();
                ListRdoTransacNoCancel = ProcessTransaction(listTransactionNoCancel, ListTreatmentFeeNoCancel, ListSereServNoCancel, listRepayNoCancel, ListBillGoodsNoCancel);
                ListRdoSereServNoCancel = ListRdoTransacNoCancel.Where(o => o.TRANSACTION_TYPE == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                ListRdoTreatmentNoCancel = ProcessTreatment(listTransactionNoCancel, ListTreatmentFeeNoCancel, ListSereServNoCancel,ListBillGoodsNoCancel);
                if (castFilter.IS_SPLIT_INTO_ROOMS == true)
                {
                    ListRdoTreaRoomNoCancel = ProcessGroupByReqRoom(listTransactionNoCancel, ListTreatmentFeeNoCancel, ListSereServNoCancel);
                }
                else
                {
                    ListRdoTreaDepaNoCancel = ProcessGroupByReqDepartment(listTransactionNoCancel, ListTreatmentFeeNoCancel, ListSereServNoCancel);
                }
                

                var ListTreaKTCNoCancelId = ListSereServNoCancel.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                //bệnh nhân dùng dịch vụ kỹ thuật thường, không hủy
                var ListTranNoKTCNoCancel = listTransactionNoCancel.Where(o => o.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER
               && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT /*&& !ListTreaKTCNoCancelId.Contains(o.TREATMENT_ID ?? 0)*/).ToList();
                var ListTreaNoKTCNoCancel = ListTreatmentFeeNoCancel.Where(o => 1 == 1/*&&!ListTreaKTCNoCancelId.Contains(o.ID)*/).ToList();
                ListRdoTranNoKTCNoCancel = ProcessTransaction(ListTranNoKTCNoCancel, ListTreaNoKTCNoCancel, ListSereServNoCancel, listRepayNoCancel,  ListBillGoodsNoCancel);

                //bệnh nhân không phải thanh toán
                ListRdoTranNoKTCNoCancel.AddRange(ProcessTreatment(new List<HIS_TRANSACTION>(), ListTreatmentFeeNoBill, ListSereServNoBill, new List<ManagerSql.SERESERV>()));

                //bệnh nhân dùng dịch vụ kỹ thuật cao, không hủy
                var ListTranKTCNoCancel = listTransactionNoCancel.Where(o => o.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER
               && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && ListTreaKTCNoCancelId.Contains(o.TREATMENT_ID ?? 0)).ToList();
                var ListTreaKTCNoCancel = ListTreatmentFeeNoCancel.Where(o => ListTreaKTCNoCancelId.Contains(o.ID)).ToList();
                ListRdoTranKTCNoCancel = ProcessTransaction(ListTranKTCNoCancel, ListTreaKTCNoCancel, ListSereServNoCancel, listRepayNoCancel, ListBillGoodsNoCancel);


                var listRepayCancel = listTransactionCancel.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();
                ListRdoTransacCancel = ProcessTransaction(listTransactionCancel, ListTreatmentFeeCancel, ListSereServCancel, listRepayCancel, ListBillGoodsCancel);
                ListRdoSereServCancel = ListRdoTransacCancel.Where(o => o.TRANSACTION_TYPE == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                ListRdoTreatmentCancel = ProcessTreatment(listTransactionCancel, ListTreatmentFeeCancel, ListSereServCancel,  ListBillGoodsCancel);
                if (castFilter.IS_SPLIT_INTO_ROOMS == true)
                {
                    ListRdoTreaRoomNoCancel = ProcessGroupByReqRoom(listTransactionCancel, ListTreatmentFeeCancel, ListSereServCancel);
                }
                else
                {
                    ListRdoTreaDepaCancel = ProcessGroupByReqDepartment(listTransactionCancel, ListTreatmentFeeCancel, ListSereServCancel);
                }
                var ListTreaKTCCancelId = ListSereServCancel.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC).Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                //bệnh nhân dùng dịch vụ kỹ thuật thường, hủy
                var ListTranNoKTCCancel = listTransactionCancel.Where(o => o.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER
               && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT /*&& !ListTreaKTCCancelId.Contains(o.TREATMENT_ID ?? 0)*/).ToList();
                var ListTreaNoKTCCancel = ListTreatmentFeeCancel.Where(o => 1 == 1/*!ListTreaKTCCancelId.Contains(o.ID)*/).ToList();
                ListRdoTranNoKTCCancel = ProcessTransaction(ListTranNoKTCCancel, ListTreaNoKTCCancel, ListSereServCancel, listRepayCancel,ListBillGoodsCancel);

                //bệnh nhân dùng dịch vụ kỹ thuật cao, hủy
                var ListTranKTCCancel = listTransactionCancel.Where(o => o.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER
               && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && ListTreaKTCCancelId.Contains(o.TREATMENT_ID ?? 0)).ToList();
                var ListTreaKTCCancel = ListTreatmentFeeCancel.Where(o => ListTreaKTCCancelId.Contains(o.ID)).ToList();
                ListRdoTranKTCCancel = ProcessTransaction(ListTranKTCCancel, ListTreaKTCCancel, ListSereServCancel, listRepayCancel, ListBillGoodsCancel);


            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        //theo hóa đơn
        private List<Mrs00720RDO> ProcessTransaction(List<HIS_TRANSACTION> ListTransaction, List<V_HIS_TREATMENT_FEE> ListTreatmentFee, List<ManagerSql.SERESERV> ListSereServ, List<HIS_TRANSACTION> ListRepay,List<ManagerSql.SERESERV> ListBillGoods)
        {
            List<Mrs00720RDO> result = new List<Mrs00720RDO>();
            if (IsNotNullOrEmpty(ListTransaction))
            {
                foreach (var item in ListTransaction)
                {
                    var treatment = ListTreatmentFee != null ? ListTreatmentFee.FirstOrDefault(o => o.ID == item.TREATMENT_ID) : null;
                    var sereServSub = ListSereServ != null ? ListSereServ.Where(o => o.BILL_ID == item.ID).ToList() : null;
                    var cashierRoom = listCashierRoom.FirstOrDefault(p => p.ID == item.CASHIER_ROOM_ID);

                    var billGoodsSubAll = ListBillGoods != null ? ListBillGoods.Where(o => o.TDL_TREATMENT_ID == item.TREATMENT_ID).ToList() : null;

                    Mrs00720RDO rdo = new Mrs00720RDO();
                    //rdo.IS_TRANSACTION_CANCEL = item.IS_CANCEL;
                    rdo.TRANSACTION_CODE = item.TRANSACTION_CODE;
                    rdo.BILL_CODE = item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU ? "" : item.ID.ToString();
                    var tranSub = ListTransaction != null ? ListTransaction.Where(o => o.TREATMENT_ID == item.ID).ToList() : null;
                    if (treatment != null)
                    {
                        AddInforTrea(rdo, treatment);

                    }
                    if (sereServSub != null && sereServSub.Count > 0)
                    {
                        AddInforSS(rdo, sereServSub);
                    }
                    if (billGoodsSubAll != null && billGoodsSubAll.Count > 0)
                    {
                        AddInforBG(rdo, billGoodsSubAll);
                    }

                    if (cashierRoom != null)
                    {
                        rdo.CASHIER_ROOM_CODE = cashierRoom.CASHIER_ROOM_CODE;
                        rdo.CASHIER_ROOM_NAME = cashierRoom.CASHIER_ROOM_NAME;
                    }
                    rdo.TRANSACTION_TYPE = item.TRANSACTION_TYPE_ID;

                    rdo.TRANSACTION_TIME = item.TRANSACTION_TIME;
                    rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.TRANSACTION_TIME);
                    rdo.TRANSACTION_TIME_STR1 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TRANSACTION_TIME);
                    var accountBook = listAccountBook.FirstOrDefault(o => o.ID == item.ACCOUNT_BOOK_ID);
                    if (accountBook != null)
                    {
                        rdo.BILL_SYMBOL = accountBook.SYMBOL_CODE;
                    }

                    //Số biên lai theo tạm ứng
                    rdo.NUM_ORDER = item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU ? "" : item.NUM_ORDER.ToString();


                    //Tiền tạm ứng
                    rdo.DEPOSIT_AMOUNT = item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU ? item.AMOUNT : 0;


                    //Tiền kết chuyển
                    rdo.KC_AMOUNT = item.KC_AMOUNT ?? 0;


                    //Tiền BH chi trả
                    //rdo.TOTAL_HEIN_PRICE = castFilter.PATIENT_TYPE_ID == 1 ? ListTransaction.Where(p => p.ID_TT == item).First().TIEN_BH ?? 0 : 0;

                    if (rdo.DEPOSIT_AMOUNT == 0)
                    {
                        //rdo.TOTAL_PATIENT_PRICE = item.AMOUNT;
                        if (item.TREATMENT_ID != null)
                        {
                            var repay = ListRepay.Where(p => p.TREATMENT_ID == item.TREATMENT_ID && p.ID > item.ID && IsDateDiffOK(p.TRANSACTION_TIME, item.TRANSACTION_TIME)).OrderBy(q => q.ID).FirstOrDefault();
                            if (repay != null)
                            {
                                rdo.CHI_RA = repay.AMOUNT;
                                rdo.REPAY_NUM_ORDER = repay.NUM_ORDER.ToString();
                                rdo.KC_AMOUNT = (item.KC_AMOUNT ?? 0) + repay.AMOUNT;
                            }
                            else
                            {
                                rdo.THU_THEM = item.AMOUNT - (item.KC_AMOUNT ?? 0);
                            }
                            //tiền dịch vụ thanh toán khác
                            if (billGoodsSubAll != null && billGoodsSubAll.Count > 0)
                            {
                                rdo.SALE_SERVICE_PRICE = billGoodsSubAll.Sum(s => s.PRICE??0);
                            }
                        }
                        //if (rdo.KC_AMOUNT > 0 && rdo.KC_AMOUNT > rdo.TOTAL_PATIENT_PRICE)
                        //{
                        //    rdo.CHI_RA = rdo.KC_AMOUNT - rdo.TOTAL_PATIENT_PRICE;
                        //}

                    }
                    else
                    {
                        rdo.TOTAL_PATIENT_PRICE = 0;

                    }

                    rdo.CASHIER_NAME = item.CASHIER_USERNAME;
                    rdo.TT_AMOUNT = item.AMOUNT;//So tien da thanh toan
                    result.Add(rdo);
                }
            }
            return result;
        }

        private bool IsDateDiffOK(long repayTime, long BillTime)
        {
            try
            {

                if (repayTime != null && BillTime != null)
                {
                    System.DateTime? dateBefore = System.DateTime.ParseExact(BillTime.ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    System.DateTime? dateAfter = System.DateTime.ParseExact(repayTime.ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    if (dateBefore != null && dateAfter != null && dateAfter >= dateBefore)
                    {
                        TimeSpan difference = dateAfter.Value - dateBefore.Value;
                        return (double)difference.TotalSeconds < (double)60;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }

        }

        //theo hồ sơ điều trị
        private List<Mrs00720RDO> ProcessTreatment(List<HIS_TRANSACTION> ListTransaction, List<V_HIS_TREATMENT_FEE> ListTreatmentFee, List<ManagerSql.SERESERV> ListSereServ,List<ManagerSql.SERESERV> ListBillGoods)
        {
            List<Mrs00720RDO> result = new List<Mrs00720RDO>();
            if (ListTreatmentFee != null && ListTreatmentFee.Count > 0)
            {
                foreach (var item in ListTreatmentFee)
                {
                    var sereServSub = ListSereServ != null ? ListSereServ.Where(o => o.TDL_TREATMENT_ID == item.ID).ToList() : null;

                    var tranSub = ListTransaction != null ? ListTransaction.Where(o => o.TREATMENT_ID == item.ID).ToList() : null;

                    var billGoodsSubAll = ListBillGoods != null ? ListBillGoods.Where(o => o.TDL_TREATMENT_ID == item.ID).ToList() : null;
                    Mrs00720RDO rdo = new Mrs00720RDO();


                    AddInforTrea(rdo, item);
                    if (tranSub != null)
                    {
                        rdo.NUM_ORDER = string.Join(";", tranSub.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(o => o.NUM_ORDER.ToString()).Distinct().ToList());
                        rdo.TRANSACTION_TIME_STR = string.Join(";", tranSub.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(o => Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.TRANSACTION_TIME)).Distinct().ToList());
                        rdo.TT_AMOUNT = tranSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(p => p.AMOUNT);
                        rdo.KC_AMOUNT = tranSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(p => p.KC_AMOUNT ?? 0);
                        if (string.IsNullOrWhiteSpace(rdo.CASHIER_NAME))
                        {
                            rdo.CASHIER_NAME = string.Join("; ",tranSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(p => p.CASHIER_USERNAME).Distinct().ToList());
                        }

                        if (string.IsNullOrWhiteSpace(rdo.CASHIER_NAME))
                        {
                            rdo.CASHIER_NAME = string.Join("; ", tranSub.Select(p => p.CASHIER_USERNAME).Distinct().ToList());
                        }
                    }

                    if (sereServSub != null && sereServSub.Count > 0)
                    {
                        AddInforSS(rdo, sereServSub);
                        rdo.PRICE = sereServSub.Sum(p => p.PRICE ?? 0);
                    }

                    if (billGoodsSubAll != null && billGoodsSubAll.Count > 0)
                    {
                        AddInforBG(rdo, billGoodsSubAll);
                    }
                    //tiền dịch vụ thanh toán khác
                    if (billGoodsSubAll != null && billGoodsSubAll.Count > 0)
                    {
                        rdo.SALE_SERVICE_PRICE = billGoodsSubAll.Sum(s => s.PRICE??0);
                    }
                    rdo.DEPOSIT_AMOUNT = item.TOTAL_DEPOSIT_AMOUNT ?? 0;
                    rdo.REPAY_AMOUNT = item.TOTAL_REPAY_AMOUNT ?? 0;
                    result.Add(rdo);
                }
            }
            return result;
        }
        

        //nhóm theo khoa chỉ định
        private List<Mrs00720RDO> ProcessGroupByReqDepartment(List<HIS_TRANSACTION> ListTransaction, List<V_HIS_TREATMENT_FEE> ListTreatmentFee, List<ManagerSql.SERESERV> ListSereServ)
        {
            List<Mrs00720RDO> result = new List<Mrs00720RDO>();
            if (ListSereServ != null && ListSereServ.Count > 0)
            {
                var group = ListSereServ.GroupBy(p => new { p.TDL_TREATMENT_ID, p.REQUEST_DEPARTMENT_ID }).ToList();
                foreach (var item in group)
                {
                    List<ManagerSql.SERESERV> listSub = item.ToList<ManagerSql.SERESERV>();

                    if (listSub != null && listSub.Count > 0)
                    {
                        Mrs00720RDO rdo = new Mrs00720RDO();

                        var treatment = ListTreatmentFee != null ? ListTreatmentFee.FirstOrDefault(o => o.ID == item.First().TDL_TREATMENT_ID) : null;
                        var tranSub = ListTransaction != null ? ListTransaction.Where(o => item.Select(p => p.BILL_ID).Contains(o.ID)).ToList() : null;
                        if (treatment != null)
                        {
                            AddInforTrea(rdo, treatment);
                            rdo.DEPOSIT_AMOUNT = treatment.TOTAL_DEPOSIT_AMOUNT ?? 0;
                            rdo.REPAY_AMOUNT = treatment.TOTAL_REPAY_AMOUNT ?? 0;
                        }
                        AddInforSS(rdo, listSub);
                        if (tranSub != null && tranSub.Count > 0)
                        {
                            rdo.TRANSACTION_CODE = string.Join(";", tranSub.Select(o => o.TRANSACTION_CODE).Distinct().ToList());
                            rdo.TRANSACTION_TIME = tranSub[0].TRANSACTION_TIME;
                            rdo.TRANSACTION_TIME_STR = string.Join(";", tranSub.Select(o => Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.TRANSACTION_TIME)).Distinct().ToList());
                            rdo.TT_AMOUNT = tranSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(p => p.AMOUNT);
                            rdo.KC_AMOUNT = tranSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(p => p.KC_AMOUNT ?? 0);
                        }

                        result.Add(rdo);
                    }
                }
            }
            return result;
        }

        //nhóm theo phòng chỉ định
        private List<Mrs00720RDO> ProcessGroupByReqRoom(List<HIS_TRANSACTION> ListTransaction, List<V_HIS_TREATMENT_FEE> ListTreatmentFee, List<ManagerSql.SERESERV> ListSereServ)
        {
            List<Mrs00720RDO> result = new List<Mrs00720RDO>();
            if (ListSereServ != null && ListSereServ.Count > 0)
            {
                var group = ListSereServ.GroupBy(p => new { p.TDL_TREATMENT_ID, p.REQUEST_ROOM_ID }).ToList();
                foreach (var item in group)
                {
                    List<ManagerSql.SERESERV> listSub = item.ToList<ManagerSql.SERESERV>();

                    if (listSub != null && listSub.Count > 0)
                    {
                        Mrs00720RDO rdo = new Mrs00720RDO();

                        var treatment = ListTreatmentFee != null ? ListTreatmentFee.FirstOrDefault(o => o.ID == item.First().TDL_TREATMENT_ID) : null;
                        var tranSub = ListTransaction != null ? ListTransaction.Where(o => item.Select(p => p.BILL_ID).Contains(o.ID)).ToList() : null;
                        if (treatment != null)
                        {
                            AddInforTrea(rdo, treatment);
                            rdo.DEPOSIT_AMOUNT = treatment.TOTAL_DEPOSIT_AMOUNT ?? 0;
                            rdo.REPAY_AMOUNT = treatment.TOTAL_REPAY_AMOUNT ?? 0;
                        }
                        AddInforSS(rdo, listSub);
                        if (tranSub != null && tranSub.Count > 0)
                        {
                            rdo.TRANSACTION_CODE = string.Join(";", tranSub.Select(o => o.TRANSACTION_CODE).Distinct().ToList());
                            rdo.TRANSACTION_TIME = tranSub[0].TRANSACTION_TIME;
                            rdo.TRANSACTION_TIME_STR = string.Join(";", tranSub.Select(o => Inventec.Common.DateTime.Convert.TimeNumberToTimeString(o.TRANSACTION_TIME)).Distinct().ToList());
                            rdo.TT_AMOUNT = tranSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(p => p.AMOUNT);
                            rdo.KC_AMOUNT = tranSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(p => p.KC_AMOUNT ?? 0);
                        }

                        result.Add(rdo);
                    }
                }
            }
            return result;
        }

        private void AddInforBill(Mrs00720RDO rdo, HIS_TRANSACTION bill)
        {
            if (bill != null)
            {
                rdo.NUM_ORDER = bill.NUM_ORDER.ToString();
                rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(bill.TRANSACTION_TIME);
                
            }
        }

        private void AddInforTrea(Mrs00720RDO rdo, V_HIS_TREATMENT_FEE treatment)
        {
            if (string.IsNullOrWhiteSpace(rdo.CASHIER_NAME))
            {
                rdo.CASHIER_NAME = treatment.FEE_LOCK_USERNAME;
            }
            rdo.PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
            rdo.PATIENT_DOB_YEAR = (treatment.TDL_PATIENT_DOB).ToString().Substring(0, 4);
            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
            {
                rdo.FEMALE_DOB = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
            }
            else if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
            {
                rdo.MALE_DOB = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
            }
            var patientClassfy = listClassify.FirstOrDefault(p => p.ID == (treatment.TDL_PATIENT_CLASSIFY_ID ?? 0));
            if (patientClassfy != null)
            {
                rdo.PATIENT_CLASSIFY_CODE = patientClassfy.PATIENT_CLASSIFY_CODE;
                rdo.PATIENT_CLASSIFY_NAME = patientClassfy.PATIENT_CLASSIFY_NAME;
            }
            rdo.PATIENT_GENDER = treatment.TDL_PATIENT_GENDER_NAME;
            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
            rdo.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
            rdo.MEDI_ORG_NAME = treatment.TDL_HEIN_MEDI_ORG_NAME;
            rdo.MEDI_ORG_CODE = treatment.TDL_HEIN_MEDI_ORG_CODE;
            rdo.ICD_CODE = treatment.ICD_CODE;
            rdo.ICD_SUB_CODE = treatment.ICD_SUB_CODE;
            rdo.ICD_TEXT = treatment.ICD_TEXT;
            rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
            rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
            rdo.TOTAL_DAY_IN_TREATMENT = treatment.TREATMENT_DAY_COUNT ?? 0;
            rdo.PATIENT_TYPE_ID = treatment.TDL_PATIENT_TYPE_ID ?? 0;
            var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID);
            if (patientType != null)
            {
                rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
            }
            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.LAST_DEPARTMENT_ID);
            if (department != null)
            {
                rdo.DEPARTMENT_CODE = department.DEPARTMENT_CODE;

                rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
            }
            rdo.TOTAL_TREATMENT_DEPO_AMOUNT = treatment.TOTAL_DEPOSIT_AMOUNT ?? 0;

            //rdo.TOTAL_PRICE = treatment.TOTAL_PATIENT_PRICE ?? 0;
        }

        private void AddInforSS(Mrs00720RDO rdo, List<ManagerSql.SERESERV> sereServSub)
        {
            var requestDepartments = HisDepartmentCFG.DEPARTMENTs.Where(o => sereServSub.Select(p => p.REQUEST_DEPARTMENT_ID).Contains(o.ID)).ToList();
            if (requestDepartments != null)
            {
                rdo.REQUEST_DEPARTMENT_NAME = string.Join(";", requestDepartments.Select(o => o.DEPARTMENT_NAME).ToList());
                rdo.REQUEST_DEPARTMENT_CODE = string.Join(";", requestDepartments.Select(o => o.DEPARTMENT_CODE).ToList());

            }
            var requestRooms = HisRoomCFG.HisRooms.Where(o => sereServSub.Select(p => p.REQUEST_ROOM_ID).Contains(o.ID)).ToList();
            if (requestRooms != null)
            {
                rdo.REQUEST_ROOM_NAME = string.Join(";", requestRooms.Select(o => o.ROOM_NAME).ToList());
                rdo.REQUEST_ROOM_CODE = string.Join(";", requestRooms.Select(o => o.ROOM_CODE).ToList());

            }

            rdo.REQUEST_USERNAME = string.Join(";", sereServSub.Select(o => o.REQUEST_USERNAME).Distinct().ToList());
            rdo.REQUEST_LOGINNAME = string.Join(";", sereServSub.Select(o => o.REQUEST_LOGINNAME).Distinct().ToList());
            rdo.TOTAL_PATIENT_PRICE = sereServSub.Sum(p => p.TDL_TOTAL_PATIENT_PRICE ?? 0);//So tien benh nhan phai thanh toan
            rdo.TOTAL_HEIN_PRICE = sereServSub.Sum(p => p.VIR_TOTAL_HEIN_PRICE ?? 0);//So tien bh thanh toan
            rdo.TOTAL_PATIENT_PRICE_BHYT = sereServSub.Sum(p => p.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0);//dct
            rdo.SERVICE_REQ_CODE = string.Join(";", sereServSub.Select(o => o.SERVICE_REQ_CODE).Distinct().ToList());
            var parents = dicPar.Where(o => sereServSub.Select(p => p.SERVICE_ID).Contains(o.Key)).Select(p => p.Value).ToList();
            rdo.PR_SERVICE_CODE = string.Join(";", parents.Select(o => o.SERVICE_CODE).Distinct().ToList());
            rdo.PR_SERVICE_NAME = string.Join(";", parents.Select(o => o.SERVICE_NAME).Distinct().ToList());

            rdo.DIC_SERVICE_PRICE = sereServSub.GroupBy(g => string.Format("{0}", dicSv.ContainsKey(g.SERVICE_ID) ? dicSv[g.SERVICE_ID].SERVICE_CODE : "NONE")).ToDictionary(p => p.Key, y => y.Sum(p => p.PRICE ?? 0));

            rdo.DIC_PARENT_PRICE = sereServSub.GroupBy(g => string.Format("{0}", dicPar.ContainsKey(g.SERVICE_ID) ? dicPar[g.SERVICE_ID].SERVICE_CODE : "NONE")).ToDictionary(p => p.Key, y => y.Sum(p => p.PRICE ?? 0));

            rdo.DIC_CATE_PRICE = sereServSub.GroupBy(g => string.Format("{0}", dicCate.ContainsKey(g.SERVICE_ID) ? dicCate[g.SERVICE_ID].CATEGORY_CODE : "NONE")).ToDictionary(p => p.Key, y => y.Sum(p => p.PRICE ?? 0));

            rdo.DIC_SERVICE_TYPE_PRICE = sereServSub.GroupBy(g => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == g.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, y => y.Sum(p => p.PRICE ?? 0));

            rdo.DIC_PARENT_COUNT = sereServSub.GroupBy(g => string.Format("{0}", dicPar.ContainsKey(g.SERVICE_ID) ? dicPar[g.SERVICE_ID].SERVICE_CODE : "NONE")).ToDictionary(p => p.Key, y => y.Sum(s => (decimal)1));

            #region Tiền bn trả
            rdo.TEST_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => s.PRICE ?? 0);

            rdo.CDHA_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(s => s.PRICE ?? 0);

            rdo.MEDICINE_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => s.PRICE ?? 0);
            rdo.BLOOD_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Sum(s => s.PRICE ?? 0);

            rdo.TTPT_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).Sum(s => s.PRICE ?? 0);

            rdo.VTYT_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => s.PRICE ?? 0);

            rdo.EXAM_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.PRICE ?? 0);

            rdo.BED_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(s => s.PRICE ?? 0);

            rdo.GPBL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL).Sum(s => s.PRICE ?? 0);

            rdo.TRANSFER_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC).Sum(s => s.PRICE ?? 0);
            rdo.MEDICINE_PRICE_NDM = sereServSub.Where(p => dicMedicine.ContainsKey(p.MEDICINE_ID??0) &&string.IsNullOrEmpty(dicMedicine[p.MEDICINE_ID??0].TDL_BID_NUMBER)).Sum(p => p.PRICE ?? 0);
            rdo.VTYT_PRICE_NDM = sereServSub.Where(p => dicMaterial.ContainsKey(p.MATERIAL_ID??0) && string.IsNullOrEmpty(dicMaterial[p.MATERIAL_ID??0].TDL_BID_NUMBER)).Sum(p => p.PRICE ?? 0);
            rdo.SV_PRICE_NDM = sereServSub.Where(p => p.TDL_HEIN_SERVICE_TYPE_ID == null).Sum(p => p.PRICE ?? 0);
            rdo.FOOD_PRICE = sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN).Sum(p => p.PRICE ?? 0);

            rdo.SIEUAM_PRICE = sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(p => p.PRICE ?? 0);
            rdo.NOISOI_PRICE = sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).Sum(p => p.PRICE ?? 0);
            rdo.TAKECARE_PRICE = sereServSub.Where(p => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == p.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE == "12").Sum(p => p.PRICE ?? 0);
            rdo.XHH_PRICE = sereServSub.Where(p => ((dicSv.ContainsKey(p.SERVICE_ID) ? dicSv[p.SERVICE_ID] : new HIS_SERVICE()).SERVICE_NAME ?? "").ToLower().Contains("xhh")).Sum(p => p.PRICE ?? 0);
            rdo.PACKAGE_PRICE = sereServSub.Sum(p => p.VIR_TOTAL_PRICE_NO_EXPEND ?? 0) + sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC || p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(p => p.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0);
            rdo.OTHER_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => s.PRICE ?? 0);
            #endregion

            #region Tổng chi phí
            rdo.TEST_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => s.SS_TOTAL_PRICE);

            rdo.CDHA_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(s => s.SS_TOTAL_PRICE);

            rdo.MEDICINE_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => s.SS_TOTAL_PRICE);
            rdo.BLOOD_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Sum(s => s.SS_TOTAL_PRICE);

            rdo.TTPT_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).Sum(s => s.SS_TOTAL_PRICE);

            rdo.VTYT_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => s.SS_TOTAL_PRICE);

            rdo.EXAM_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.SS_TOTAL_PRICE);

            rdo.BED_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(s => s.SS_TOTAL_PRICE);

            rdo.GPBL_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL).Sum(s => s.SS_TOTAL_PRICE);

            rdo.TRANSFER_TOTAL_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC).Sum(s => s.SS_TOTAL_PRICE);
            rdo.MEDICINE_TOTAL_PRICE_NDM = sereServSub.Where(p => dicMedicine.ContainsKey(p.MEDICINE_ID??0) &&string.IsNullOrEmpty(dicMedicine[p.MEDICINE_ID??0].TDL_BID_NUMBER)).Sum(p => p.SS_TOTAL_PRICE);
            rdo.VTYT_TOTAL_PRICE_NDM = sereServSub.Where(p => dicMaterial.ContainsKey(p.MATERIAL_ID??0) && string.IsNullOrEmpty(dicMaterial[p.MATERIAL_ID??0].TDL_BID_NUMBER)).Sum(p => p.SS_TOTAL_PRICE);
            rdo.SV_TOTAL_PRICE_NDM = sereServSub.Where(p => p.TDL_HEIN_SERVICE_TYPE_ID == null).Sum(p => p.SS_TOTAL_PRICE);
            rdo.FOOD_TOTAL_PRICE = sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN).Sum(p => p.SS_TOTAL_PRICE);

            rdo.SIEUAM_TOTAL_PRICE = sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(p => p.SS_TOTAL_PRICE);
            rdo.NOISOI_TOTAL_PRICE = sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).Sum(p => p.SS_TOTAL_PRICE);
            rdo.TAKECARE_TOTAL_PRICE = sereServSub.Where(p => (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == p.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE == "12").Sum(p => p.SS_TOTAL_PRICE);
            rdo.XHH_TOTAL_PRICE = sereServSub.Where(p => ((dicSv.ContainsKey(p.SERVICE_ID) ? dicSv[p.SERVICE_ID] : new HIS_SERVICE()).SERVICE_NAME ?? "").ToLower().Contains("xhh")).Sum(p => p.SS_TOTAL_PRICE);
            rdo.PACKAGE_TOTAL_PRICE = sereServSub.Sum(p => p.VIR_TOTAL_PRICE_NO_EXPEND ?? 0) + sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC || p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(p => p.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0);
            rdo.OTHER_TOTAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => s.SS_TOTAL_PRICE);
            #endregion

            rdo.SERVICE_REQUEST_CODEs = string.Join(";", sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && p.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT && p.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Select(p => p.SERVICE_REQ_CODE).Distinct().ToList());

            rdo.SERVICE_TYPE_ID = sereServSub[0].TDL_SERVICE_TYPE_ID;
            rdo.PRICE = sereServSub.Sum(p => p.PRICE ?? 0);
            rdo.TOTAL_OTHER_SOURCE_PRICE = sereServSub.Sum(p => (p.SS_AMOUNT ?? 0) * (p.TDL_OTHER_SOURCE_PRICE ?? 0)); ;

            rdo.TOTAL_PRICE = rdo.TOTAL_PATIENT_PRICE + rdo.TOTAL_HEIN_PRICE;
        }

        private void AddInforBG(Mrs00720RDO rdo, List<ManagerSql.SERESERV> billGoodsSub)
        {
            rdo.DIC_BILL_GOODS_PRICE = billGoodsSub.GroupBy(g => string.Format("{0}", dicNoneMediSv.ContainsKey(g.SERVICE_ID) ? dicNoneMediSv[g.SERVICE_ID].NONE_MEDI_SERVICE_CODE : "NONE")).ToDictionary(p => p.Key, y => y.Sum(p => p.PRICE ?? 0));
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
            dicSingleTag.Add("PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(ListRdoSereServNoCancel.Sum(p => p.TOTAL_PATIENT_PRICE)).ToString()));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
            objectTag.AddObjectData(store, "Report", ListRdoSereServNoCancel.OrderBy(p => p.PATIENT_CODE).ThenBy(a => a.TRANSACTION_CODE).ToList());
            objectTag.AddObjectData(store, "ReportCancel", ListRdoSereServCancel.OrderBy(p => p.PATIENT_CODE).ThenBy(a => a.TRANSACTION_CODE).ToList());
            objectTag.AddObjectData(store, "Parent", ListRdoSereServNoCancel.GroupBy(o => o.PATIENT_TYPE_NAME).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "ParentCancel", ListRdoSereServCancel.GroupBy(o => o.PATIENT_TYPE_NAME).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Doctor", ListRdoSereServNoCancel.Where(p => !string.IsNullOrWhiteSpace(p.PR_SERVICE_CODE)).OrderBy(p => p.PR_SERVICE_CODE).ThenBy(p => p.REQUEST_LOGINNAME).Distinct().ToList());
            objectTag.AddObjectData(store, "DoctorCancel", ListRdoSereServCancel.Where(p => !string.IsNullOrWhiteSpace(p.PR_SERVICE_CODE)).OrderBy(p => p.PR_SERVICE_CODE).ThenBy(p => p.REQUEST_LOGINNAME).Distinct().ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "PATIENT_TYPE_NAME", "PATIENT_TYPE_NAME");
            objectTag.AddRelationship(store, "ParentCancel", "ReportCancel", "PATIENT_TYPE_NAME", "PATIENT_TYPE_NAME");

            objectTag.AddObjectData(store, "Report0", ListRdoTransacNoCancel.Where(p => p.TRANSACTION_TYPE == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU || p.TRANSACTION_TYPE == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).OrderBy(p => p.PATIENT_CODE).ThenBy(a => a.TRANSACTION_CODE).ToList());

            objectTag.AddObjectData(store, "Report0Cancel", ListRdoTransacCancel.Where(p => p.TRANSACTION_TYPE == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU || p.TRANSACTION_TYPE == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).OrderBy(p => p.PATIENT_CODE).ThenBy(a => a.TRANSACTION_CODE).ToList());

            objectTag.AddObjectData(store, "Report1", ListRdoTreatmentNoCancel.OrderBy(p => p.TREATMENT_CODE).ToList());

            objectTag.AddObjectData(store, "Report1Cancel", ListRdoTreatmentCancel.OrderBy(p => p.TREATMENT_CODE).ToList());

            objectTag.AddObjectData(store, "Report2", ListRdoTranNoKTCNoCancel.OrderBy(p => p.TRANSACTION_TIME_STR).ToList());
            objectTag.AddObjectData(store, "Report3", ListRdoTranKTCNoCancel.OrderBy(p => p.TRANSACTION_TIME_STR).ToList());

            objectTag.AddObjectData(store, "Report2Cancel", ListRdoTranNoKTCCancel.OrderBy(p => p.TRANSACTION_TIME_STR).ToList());
            objectTag.AddObjectData(store, "Report3Cancel", ListRdoTranKTCCancel.OrderBy(p => p.TRANSACTION_TIME_STR).ToList());

            objectTag.AddObjectData(store, "Parent1", ListRdoTreatmentNoCancel.GroupBy(o => o.PATIENT_TYPE_NAME).Select(p => p.First()).ToList());

            objectTag.AddObjectData(store, "Parent1Cancel", ListRdoTreatmentCancel.GroupBy(o => o.PATIENT_TYPE_NAME).Select(p => p.First()).ToList());

            objectTag.AddRelationship(store, "Parent1", "Report1", "PATIENT_TYPE_NAME", "PATIENT_TYPE_NAME");

            objectTag.AddRelationship(store, "Parent1Cancel", "Report1Cancel", "PATIENT_TYPE_NAME", "PATIENT_TYPE_NAME");

            objectTag.AddObjectData(store, "ReportTreatment", ListRdoTreaDepaNoCancel.OrderBy(p => p.TRANSACTION_TIME).ThenBy(p => p.REQUEST_DEPARTMENT_CODE).ThenBy(p => p.PATIENT_CODE).ToList());

            objectTag.AddObjectData(store, "ReportTreatmentCancel", ListRdoTreaDepaCancel.OrderBy(p => p.TRANSACTION_TIME).ThenBy(p => p.REQUEST_DEPARTMENT_CODE).ThenBy(p => p.PATIENT_CODE).ToList());

            objectTag.AddObjectData(store, "ReportGroup", ListRdoSereServNoCancel.OrderBy(p => p.TRANSACTION_TIME).ThenBy(p => p.REQUEST_DEPARTMENT_CODE).ThenBy(P => P.PATIENT_CODE).ToList());

            objectTag.AddObjectData(store, "ReportGroupCancel", ListRdoSereServCancel.OrderBy(p => p.TRANSACTION_TIME).ThenBy(p => p.REQUEST_DEPARTMENT_CODE).ThenBy(P => P.PATIENT_CODE).ToList());
            var service = new ManagerSql().GetService();
            service = service.OrderBy(p => p.SERVICE_CODE).ToList();
            for (int i = 0; i < service.Count(); i++)
            {
                dicSingleTag.Add("PARENT_SERVICE_NAME__" + i, service[i].SERVICE_NAME);
                dicSingleTag.Add("PARENT_SERVICE_CODE__" + i, service[i].SERVICE_CODE);
            }

            objectTag.AddObjectData(store, "ParentService", service);
            List<PATIENT_CLASSIFY> listPatientClassify = new List<PATIENT_CLASSIFY>();
            if (castFilter.TDL_PATIENT_CLASSIFY_IDs != null)
            {
                listPatientClassify = new ManagerSql().GetClassify(castFilter.TDL_PATIENT_CLASSIFY_IDs) ?? new List<PATIENT_CLASSIFY>();
                dicSingleTag.Add("PATIENT_CLASSIFY_NAME", string.Join(",", listPatientClassify.Select(p => p.PATIENT_CLASSIFY_NAME).ToList()));
            }

            List<CASHIER_USER> listUser = new List<CASHIER_USER>();
            if (castFilter.CASHIER_LOGINNAMEs != null)
            {
                listUser = new ManagerSql().GetCashierUser(castFilter.CASHIER_LOGINNAMEs) ?? new List<CASHIER_USER>();
                dicSingleTag.Add("CASHIER_USER", string.Join(",", listUser.Select(p => p.USERNAME).ToList()));
            }
        }
    }
}
