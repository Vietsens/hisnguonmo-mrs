using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00350
{
    class Mrs00350Processor : AbstractProcessor
    {
        Mrs00350Filter castFilter = null;

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

        public Mrs00350Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatment = null;
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = null;
        List<V_HIS_TRANSACTION> listTransaction = new List<V_HIS_TRANSACTION>();
        Dictionary<long, List<V_HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV>>();
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

        List<Mrs00350RDO> listRdo = new List<Mrs00350RDO>();

        public override Type FilterType()
        {
            return typeof(Mrs00350Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00350Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00350, Filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.castFilter), this.castFilter));

                HisTreatmentViewFilterQuery treatFilter = new HisTreatmentViewFilterQuery();
                treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatFilter);
                var listTreatmentId = listTreatment.Select(o => o.ID).Distinct().ToList();
                var skip = 0;
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIds = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                    transactionFilter.TREATMENT_IDs = listIds;
                    transactionFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                    transactionFilter.HAS_SALL_TYPE = false;
                    var listSub = new HisTransactionManager(paramGet).GetView(transactionFilter);
                    listTransaction.AddRange(listSub);
                    Inventec.Common.Logging.LogSystem.Info("listSub" + listSub.Count);
                }
                if (castFilter.CASHIER_LOGINNAME != null)
                {
                    listTransaction = listTransaction.Where(p => p.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME).ToList();
                }
                if (castFilter.LOGINNAME != null)
                {
                    listTransaction = listTransaction.Where(p => p.CASHIER_LOGINNAME == castFilter.LOGINNAME).ToList();
                }
                listTreatment = listTreatment.Where(o => listTransaction.Exists(p => p.TREATMENT_ID == o.ID)).ToList();
                HisServiceRetyCatViewFilterQuery serviceRetyFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyFilter.REPORT_TYPE_CODE__EXACT = "MRS00350";
                listServiceRetyCat = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00350");
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
                if (IsNotNullOrEmpty(listTreatment) && IsNotNullOrEmpty(listServiceRetyCat))
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
                    int count = listTreatment.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listSub = listTreatment.Skip(start).Take(limit).ToList();

                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                        ssFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        var listSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);

                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DOAGET trong qua trinh lay du lieu MRS00350");
                        }
                        if (IsNotNullOrEmpty(listSereServ))
                        {
                            foreach (var item in listSereServ)
                            {
                                if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.IS_NO_PAY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                {
                                    continue;
                                }
                                if (item.MEDICINE_ID.HasValue || item.MATERIAL_ID.HasValue)
                                    continue;
                                if (!dicSereServ.ContainsKey(item.TDL_TREATMENT_ID ?? 0))
                                    dicSereServ[item.TDL_TREATMENT_ID ?? 0] = new List<V_HIS_SERE_SERV>();
                                dicSereServ[item.TDL_TREATMENT_ID ?? 0].Add(item);
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
            Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listTreatment.Count), listTreatment.Count));
            foreach (var treatment in listTreatment)
            {
                if (treatment.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    continue;
                if (!treatment.FEE_LOCK_TIME.HasValue)
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri da khoa vien phi nhung khong co thoi gian khoa vien phi: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment.TREATMENT_CODE), treatment.TREATMENT_CODE));
                    continue;
                }
                if (!dicSereServ.ContainsKey(treatment.ID))
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co thong tin dich vu thuc hien: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment.TREATMENT_CODE), treatment.TREATMENT_CODE));
                    continue;
                }
                var hisSereServs = dicSereServ[treatment.ID];
                Mrs00350RDO rdo = new Mrs00350RDO(treatment);
                foreach (var item in hisSereServs)
                {
                    if (dicServiceId__KSK.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.EXAM_AMOUNT_KSK += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__KB.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.EXAM_AMOUNT_KB += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__HH.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.TEST_AMOUNT_HH += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__HS.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.TEST_AMOUNT_HS += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__NT.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.TEST_AMOUNT_NT += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__HIV.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.TEST_AMOUNT_HIV += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__SA.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.DIIM_AMOUNT_SA += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__XQ.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.DIIM_AMOUNT_XQ += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__LH.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.DIIM_AMOUNT_REG += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__DT.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.DIIM_AMOUNT_ECG += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__TT.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.MISU_AMOUNT += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else if (dicServiceId__PT.ContainsKey(item.SERVICE_ID))
                    {
                        rdo.SURG_AMOUNT += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                    else
                    {
                        rdo.OTHER_AMOUNT += item.VIR_TOTAL_PATIENT_PRICE ?? 0;
                    }
                }
                listRdo.Add(rdo);
            }
        }

        void ProcessRdo()
        {
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.GroupBy(g => g.FEE_LOCK_DATE).Select(s => new Mrs00350RDO() { FEE_LOCK_DATE = s.First().FEE_LOCK_DATE, FEE_LOCK_DATE_STR = s.First().FEE_LOCK_DATE_STR, EXAM_AMOUNT_KSK = s.Sum(s1 => s1.EXAM_AMOUNT_KSK), EXAM_AMOUNT_KB = s.Sum(s2 => s2.EXAM_AMOUNT_KB), TEST_AMOUNT_HH = s.Sum(s3 => s3.TEST_AMOUNT_HH), TEST_AMOUNT_HS = s.Sum(s4 => s4.TEST_AMOUNT_HS), TEST_AMOUNT_NT = s.Sum(s5 => s5.TEST_AMOUNT_NT), TEST_AMOUNT_HIV = s.Sum(s6 => s6.TEST_AMOUNT_HIV), DIIM_AMOUNT_SA = s.Sum(s7 => s7.DIIM_AMOUNT_SA), DIIM_AMOUNT_XQ = s.Sum(s8 => s8.DIIM_AMOUNT_XQ), DIIM_AMOUNT_REG = s.Sum(s9 => s9.DIIM_AMOUNT_REG), DIIM_AMOUNT_ECG = s.Sum(s10 => s10.DIIM_AMOUNT_ECG), MISU_AMOUNT = s.Sum(s11 => s11.MISU_AMOUNT), SURG_AMOUNT = s.Sum(s12 => s12.SURG_AMOUNT), OTHER_AMOUNT = s.Sum(s13 => s13.OTHER_AMOUNT) }).OrderBy(o => o.FEE_LOCK_DATE).ToList();
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
