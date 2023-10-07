using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisAccountBook;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00361
{
    class Mrs00361Processor : AbstractProcessor
    {
        Mrs00361Filter castFilter = null;

        private const string REPORT_TYPE_CAT__KHAM_BENH = "350KB";
        private const string REPORT_TYPE_CAT__KHAM_SUC_KHOE = "350KSK";
        private const string REPORT_TYPE_CAT__HUYET_HOC = "350HH";
        private const string REPORT_TYPE_CAT__HOA_SINH = "350KHS";
        private const string REPORT_TYPE_CAT__NUOC_TIEU = "350NT";
        private const string REPORT_TYPE_CAT__HIV_HBSag = "350HIV";
        private const string REPORT_TYPE_CAT__SIEU_AM = "350SA";
        private const string REPORT_TYPE_CAT__X_QUANG = "350XQ";
        private const string REPORT_TYPE_CAT__LUU_HUYET = "350REG";
        private const string REPORT_TYPE_CAT__DIEN_TIM = "350ECG";
        private const string REPORT_TYPE_CAT__THU_THUAT = "350TT";
        private const string REPORT_TYPE_CAT__PHAU_THUAT = "350PT";

        public Mrs00361Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TRANSACTION> listBill = null;
        List<V_HIS_TRANSACTION> listDeposit = null;
        //List<V_HIS_TREATMENT> listTreatment = null; 
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = null;

        Dictionary<long, List<V_HIS_SERE_SERV_BILL>> dicSereServBill = new Dictionary<long, List<V_HIS_SERE_SERV_BILL>>();
        Dictionary<long, V_HIS_SERE_SERV> dicSereServ = new Dictionary<long, V_HIS_SERE_SERV>();
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatyAlterTreatIn = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__KB = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__KSK = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__HH = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__HS = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__NT = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__HIV = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__SA = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__XQ = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__LH = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__DT = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__TT = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicServiceId__PT = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();

        List<Mrs00361RDO> listRdo = new List<Mrs00361RDO>();


        public override Type FilterType()
        {
            return typeof(Mrs00361Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00361Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu Mrs00361, Filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.castFilter), this.castFilter));

                HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                transactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT, IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU };
                transactionFilter.HAS_SALL_TYPE = false;
                var listTransaction = new HisTransactionManager(paramGet).GetView(transactionFilter);

                if (castFilter.CASHIER_LOGINNAME != null)
                {
                    listTransaction = listTransaction.Where(p => p.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME).ToList();
                }
                if (castFilter.LOGINNAME != null)
                {
                    listTransaction = listTransaction.Where(p => p.CASHIER_LOGINNAME == castFilter.LOGINNAME).ToList();
                }
                listBill = listTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();

                //Cac dich vu thanh toan vien phi cua benh nhan
                var listTreatmentId = listBill.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();
                var skip = 0;
                List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIds = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    long PATIENT_TYPE_ID__FEE = MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                    HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                    sereServFilter.TREATMENT_IDs = listIds;
                    sereServFilter.PATIENT_TYPE_ID = PATIENT_TYPE_ID__FEE;
                    List<V_HIS_SERE_SERV> ListSereServSub = new HisSereServManager(paramGet).GetView(sereServFilter);
                    ListSereServ.AddRange(ListSereServSub ?? new List<V_HIS_SERE_SERV>());
                }
                dicSereServ = ListSereServ.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                //

                listDeposit = listTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();

                HisServiceRetyCatViewFilterQuery serviceRetyFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyFilter.REPORT_TYPE_CODE__EXACT = "MRS00350";
                listServiceRetyCat = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00361");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listBill) && IsNotNullOrEmpty(listServiceRetyCat))
                {
                    CommonParam paramGet = new CommonParam();
                    foreach (var item in listServiceRetyCat)
                    {
                        switch (item.CATEGORY_CODE)
                        {
                            case REPORT_TYPE_CAT__KHAM_SUC_KHOE:
                                dicServiceId__KSK[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__KHAM_BENH:
                                dicServiceId__KB[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__HUYET_HOC:
                                dicServiceId__HH[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__HOA_SINH:
                                dicServiceId__HS[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__NUOC_TIEU:
                                dicServiceId__NT[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__HIV_HBSag:
                                dicServiceId__HIV[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__SIEU_AM:
                                dicServiceId__SA[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__X_QUANG:
                                dicServiceId__XQ[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__LUU_HUYET:
                                dicServiceId__LH[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__DIEN_TIM:
                                dicServiceId__DT[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__THU_THUAT:
                                dicServiceId__TT[item.SERVICE_ID] = item;
                                break;
                            case REPORT_TYPE_CAT__PHAU_THUAT:
                                dicServiceId__PT[item.SERVICE_ID] = item;
                                break;

                            default:
                                break;
                        }
                    }

                    int start = 0;
                    //var listTretmentId = listBill.Select(s => s.TREATMENT_ID).Distinct().ToList(); 
                    int count = listBill.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listSub = listBill.Skip(start).Take(limit).ToList();

                        HisSereServBillViewFilterQuery ssbFilter = new HisSereServBillViewFilterQuery();
                        ssbFilter.BILL_IDs = listSub.Select(s => s.ID).ToList();
                        var listSereServBill = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(paramGet).GetView(ssbFilter);

                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        patyAlterFilter.TREATMENT_IDs = listSub.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList();
                        patyAlterFilter.ORDER_DIRECTION = "DESC";
                        patyAlterFilter.ORDER_FIELD = "LOG_TIME";
                        var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter);

                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DOAGET trong qua trinh lay du lieu Mrs00361");
                        }
                        if (IsNotNullOrEmpty(listSereServBill))
                        {
                            foreach (var item in listSereServBill)
                            {

                                if (item.MEDICINE_ID.HasValue || item.MATERIAL_ID.HasValue)
                                    continue;
                                if (!dicSereServBill.ContainsKey(item.BILL_ID))
                                    dicSereServBill[item.BILL_ID] = new List<V_HIS_SERE_SERV_BILL>();
                                dicSereServBill[item.BILL_ID].Add(item);
                            }
                        }

                        if (IsNotNullOrEmpty(listPatientTypeAlter))
                        {
                            foreach (var item in listPatientTypeAlter)
                            {

                                if (dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID))
                                    continue;
                                dicCurrentPatyAlter[item.TREATMENT_ID] = item;
                            }
                            var Groups = listPatientTypeAlter.GroupBy(g => g.TREATMENT_ID).ToList();
                            foreach (var group in Groups)
                            {
                                var listGroup = group.ToList<V_HIS_PATIENT_TYPE_ALTER>().OrderBy(o => o.LOG_TIME).ToList();
                                foreach (var item in listGroup)
                                {
                                    if (item.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                    {
                                        if (!dicPatyAlterTreatIn.ContainsKey(item.TREATMENT_ID))
                                        {
                                            dicPatyAlterTreatIn[item.TREATMENT_ID] = item;
                                        }
                                    }
                                }
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    this.ProcessDataDetail();
                    this.ProcessRdo();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        void ProcessDataDetail()
        {

            List<long> ACCOUNT_BOOK_ID__KSKs = HisAccountBookCFG.ACCOUNT_BOOK_ID__KSKs;
            long PATIENT_TYPE_ID__BHYT = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
            foreach (var bill in listBill)
            {
                if (bill.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    continue;
                }

                if (!dicSereServBill.ContainsKey(bill.ID))
                {
                    Inventec.Common.Logging.LogSystem.Info("Giao dich thanh toan khong co dich vu nao: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => bill.TRANSACTION_CODE), bill.TRANSACTION_CODE));
                    continue;
                }
                if (!dicCurrentPatyAlter.ContainsKey(bill.TREATMENT_ID ?? 0))
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co thong tin doi tuong: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => bill.TREATMENT_CODE), bill.TREATMENT_CODE));
                    continue;
                }
                var patyAlter = dicCurrentPatyAlter[bill.TREATMENT_ID ?? 0];
                if (patyAlter.PATIENT_TYPE_ID == PATIENT_TYPE_ID__BHYT)
                {
                    continue;
                }
                if (dicPatyAlterTreatIn.ContainsKey(bill.TREATMENT_ID ?? 0) && dicPatyAlterTreatIn[bill.TREATMENT_ID ?? 0].LOG_TIME < bill.TRANSACTION_TIME)
                {
                    continue;
                }
                var hisSereServBills = dicSereServBill[bill.ID];
                Mrs00361RDO rdo = new Mrs00361RDO(bill);
                foreach (var item in hisSereServBills)
                {
                    if (dicServiceId__KSK.ContainsKey(item.SERVICE_ID) || ACCOUNT_BOOK_ID__KSKs.Contains(bill.ACCOUNT_BOOK_ID))
                    {
                        rdo.EXAM_AMOUNT_KSK += item.AMOUNT;
                        rdo.EXAM_PRICE_KSK += item.PRICE;
                    }
                    else if (dicServiceId__KB.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.EXAM_AMOUNT_KB += item.AMOUNT;
                        rdo.EXAM_PRICE_KB += item.PRICE;
                    }
                    else if (dicServiceId__HH.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.TEST_AMOUNT_HH += item.AMOUNT;
                        rdo.TEST_PRICE_HH += item.PRICE;
                    }
                    else if (dicServiceId__HS.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.TEST_AMOUNT_HS += item.AMOUNT;
                        rdo.TEST_PRICE_HS += item.PRICE;
                    }
                    else if (dicServiceId__NT.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.TEST_AMOUNT_NT += item.AMOUNT;
                        rdo.TEST_PRICE_NT += item.PRICE;
                    }
                    else if (dicServiceId__HIV.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.TEST_AMOUNT_HIV += item.AMOUNT;
                        rdo.TEST_PRICE_HIV += item.PRICE;
                    }
                    else if (dicServiceId__SA.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.DIIM_AMOUNT_SA += item.AMOUNT;
                        rdo.DIIM_PRICE_SA += item.PRICE;
                    }
                    else if (dicServiceId__XQ.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.DIIM_AMOUNT_XQ += item.AMOUNT;
                        rdo.DIIM_PRICE_XQ += item.PRICE;
                    }
                    else if (dicServiceId__LH.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.DIIM_AMOUNT_REG += item.AMOUNT;
                        rdo.DIIM_PRICE_REG += item.PRICE;
                    }
                    else if (dicServiceId__DT.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.DIIM_AMOUNT_ECG += item.AMOUNT;
                        rdo.DIIM_PRICE_ECG += item.PRICE;
                    }
                    else if (dicServiceId__TT.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.MISU_AMOUNT += item.AMOUNT;
                        rdo.MISU_PRICE += item.PRICE;
                    }
                    else if (dicServiceId__PT.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.SURG_AMOUNT += item.AMOUNT;
                        rdo.SURG_PRICE += item.PRICE;
                    }
                    else
                    {
                        rdo.OTHER_AMOUNT += item.AMOUNT;
                        rdo.OTHER_PRICE += item.PRICE;
                    }
                }
                listRdo.Add(rdo);
            }
            foreach (var deposit in listDeposit)
            {
                if (deposit.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    continue;
                }

                if (!dicCurrentPatyAlter.ContainsKey(deposit.TREATMENT_ID ?? 0))
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co thong tin doi tuong: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => deposit.TREATMENT_CODE), deposit.TREATMENT_CODE));
                    continue;
                }
                var patyAlter = dicCurrentPatyAlter[deposit.TREATMENT_ID ?? 0];
                if (patyAlter.PATIENT_TYPE_ID == PATIENT_TYPE_ID__BHYT)
                {
                    continue;
                }
                if (dicPatyAlterTreatIn.ContainsKey(deposit.TREATMENT_ID ?? 0) && dicPatyAlterTreatIn[deposit.TREATMENT_ID ?? 0].LOG_TIME < deposit.TRANSACTION_TIME)
                {
                    continue;
                }

                Mrs00361RDO rdo = new Mrs00361RDO(deposit);

                listRdo.Add(rdo);
            }
        }

        void ProcessRdo()
        {
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.GroupBy(g => g.FEE_LOCK_DATE).Select(s => new Mrs00361RDO() { FEE_LOCK_DATE = s.First().FEE_LOCK_DATE, FEE_LOCK_DATE_STR = s.First().FEE_LOCK_DATE_STR, EXAM_AMOUNT_KSK = s.Sum(s1 => s1.EXAM_AMOUNT_KSK), EXAM_AMOUNT_KB = s.Sum(s2 => s2.EXAM_AMOUNT_KB), TEST_AMOUNT_HH = s.Sum(s3 => s3.TEST_AMOUNT_HH), TEST_AMOUNT_HS = s.Sum(s4 => s4.TEST_AMOUNT_HS), TEST_AMOUNT_NT = s.Sum(s5 => s5.TEST_AMOUNT_NT), TEST_AMOUNT_HIV = s.Sum(s6 => s6.TEST_AMOUNT_HIV), DIIM_AMOUNT_SA = s.Sum(s7 => s7.DIIM_AMOUNT_SA), DIIM_AMOUNT_XQ = s.Sum(s8 => s8.DIIM_AMOUNT_XQ), DIIM_AMOUNT_REG = s.Sum(s9 => s9.DIIM_AMOUNT_REG), DIIM_AMOUNT_ECG = s.Sum(s10 => s10.DIIM_AMOUNT_ECG), MISU_AMOUNT = s.Sum(s11 => s11.MISU_AMOUNT), SURG_AMOUNT = s.Sum(s12 => s12.SURG_AMOUNT), OTHER_AMOUNT = s.Sum(s13 => s13.OTHER_AMOUNT), EXAM_PRICE_KSK = s.Sum(s1 => s1.EXAM_PRICE_KSK), EXAM_PRICE_KB = s.Sum(s2 => s2.EXAM_PRICE_KB), TEST_PRICE_HH = s.Sum(s3 => s3.TEST_PRICE_HH), TEST_PRICE_HS = s.Sum(s4 => s4.TEST_PRICE_HS), TEST_PRICE_NT = s.Sum(s5 => s5.TEST_PRICE_NT), TEST_PRICE_HIV = s.Sum(s6 => s6.TEST_PRICE_HIV), DIIM_PRICE_SA = s.Sum(s7 => s7.DIIM_PRICE_SA), DIIM_PRICE_XQ = s.Sum(s8 => s8.DIIM_PRICE_XQ), DIIM_PRICE_REG = s.Sum(s9 => s9.DIIM_PRICE_REG), DIIM_PRICE_ECG = s.Sum(s10 => s10.DIIM_PRICE_ECG), MISU_PRICE = s.Sum(s11 => s11.MISU_PRICE), SURG_PRICE = s.Sum(s12 => s12.SURG_PRICE), OTHER_PRICE = s.Sum(s13 => s13.OTHER_PRICE), DEPOSIT_AMOUNT = s.Sum(s13 => s13.DEPOSIT_AMOUNT) }).OrderBy(o => o.FEE_LOCK_DATE).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
