using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00696
{
    class Mrs00696Processor : AbstractProcessor
    {
        string ReportType = "";
        Mrs00696Filter castFilter = null;
        List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_REPORT_TYPE_CAT> ListReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        Dictionary<long, HIS_SERVICE> DicService = new Dictionary<long, HIS_SERVICE>();
        List<D_HIS_SERE_SERV> ListSereServ = new List<D_HIS_SERE_SERV>();
        List<Mrs00696RDO> ListRdo = new List<Mrs00696RDO>();
        const string CT = "696_CT";
        const string DSA = "696_DSA";
        const string GPB = "696_GPB";
        const string G = "696_G";
        const string HHTM = "696_HHTM";
        const string K = "696_K";
        const string MRI = "696_MRI";
        const string NS = "696_NS";
        const string PTTT = "696_PTTT";
        const string SA = "696_SA";
        const string TDCN = "696_TDCN";
        const string VLTL = "696_VLTL";
        const string XN = "696_XN";
        const string XQ = "696_XQ";
        List<string> lstTypeCatCode = new List<string>() { CT, DSA, GPB, G, HHTM, K, MRI, NS, PTTT, SA, TDCN, VLTL, XN, XQ };

        public Mrs00696Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            ReportType = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00696Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00696Filter)reportFilter);
                CommonParam paramGet = new CommonParam();
                ListSereServ = new ManagerSql().GetSS(castFilter);

                HisServiceRetyCatViewFilterQuery retyCatFilter = new HisServiceRetyCatViewFilterQuery();
                retyCatFilter.REPORT_TYPE_CODE__EXACT = ReportType;
                retyCatFilter.IS_ACTIVE = 1;
                ListServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(retyCatFilter);

                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery();
                reportTypeCatFilter.REPORT_TYPE_CODE__EXACT = ReportType;
                reportTypeCatFilter.IS_ACTIVE = 1;
                ListReportTypeCat = new HisReportTypeCatManager(paramGet).Get(reportTypeCatFilter);

                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                var ListService = new HisServiceManager(paramGet).Get(serviceFilter);

                if (IsNotNullOrEmpty(ListService))
                {
                    DicService = ListService.ToDictionary(o => o.ID, o => o);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    ListRdo = new List<Mrs00696RDO>();

                    if (ListSereServ.Exists(o => o.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DV && !o.PRIMARY_PATIENT_TYPE_ID.HasValue))
                    {
                        var dicPaty = MANAGER.Config.HisServicePatyCFG.DicServicePaty;
                        var lstPaty = MANAGER.Config.HisServicePatyCFG.DATAs;
                    }

                    //List<Thread> threadData = new List<Thread>();
                    //var maxReq = ListSereServ.Count / 5 + 1;
                    //Inventec.Common.Logging.LogSystem.Info(maxReq + "/" + ListSereServ.Count);
                    //int skip = 0;
                    //while (ListSereServ.Count - skip > 0)
                    //{
                    //    var datas = ListSereServ.Skip(skip).Take(maxReq).ToList();
                    //    skip += maxReq;
                    //    Thread hein = new Thread(ProcessListData);
                    //    hein.Start(datas);
                    //    threadData.Add(hein);
                    //}

                    //for (int i = 0; i < threadData.Count; i++)
                    //{
                    //    threadData[i].Join();
                    //    Thread.Sleep(100);
                    //}
                    ProcessRdo(ListSereServ);

                    ProcessDataTotal();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessDataTotal()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.GroupBy(g => g.REQUEST_DEPARTMENT_ID).Select(s => new Mrs00696RDO()
                    {
                        REQUEST_DEPARTMENT_ID = s.First().REQUEST_DEPARTMENT_ID,
                        REQUEST_DEPARTMENT_CODE = s.First().REQUEST_DEPARTMENT_CODE,
                        REQUEST_DEPARTMENT_NAME = s.First().REQUEST_DEPARTMENT_NAME,
                        CT_AMOUNT = s.Sum(o => o.CT_AMOUNT),
                        CT_TOTAL_AMOUNT = s.Sum(o => o.CT_TOTAL_AMOUNT),
                        DSA_AMOUNT = s.Sum(o => o.DSA_AMOUNT),
                        DSA_TOTAL_AMOUNT = s.Sum(o => o.DSA_TOTAL_AMOUNT),
                        GPB_AMOUNT = s.Sum(o => o.GPB_AMOUNT),
                        GPB_TOTAL_AMOUNT = s.Sum(o => o.GPB_TOTAL_AMOUNT),
                        G_AMOUNT = s.Sum(o => o.G_AMOUNT),
                        G_TOTAL_AMOUNT = s.Sum(o => o.G_TOTAL_AMOUNT),
                        HHTM_AMOUNT = s.Sum(o => o.HHTM_AMOUNT),
                        HHTM_TOTAL_AMOUNT = s.Sum(o => o.HHTM_TOTAL_AMOUNT),
                        K_AMOUNT = s.Sum(o => o.K_AMOUNT),
                        K_TOTAL_AMOUNT = s.Sum(o => o.K_TOTAL_AMOUNT),
                        MRI_AMOUNT = s.Sum(o => o.MRI_AMOUNT),
                        MRI_TOTAL_AMOUNT = s.Sum(o => o.MRI_TOTAL_AMOUNT),
                        NS_AMOUNT = s.Sum(o => o.NS_AMOUNT),
                        NS_TOTAL_AMOUNT = s.Sum(o => o.NS_TOTAL_AMOUNT),
                        PTTT_AMOUNT = s.Sum(o => o.PTTT_AMOUNT),
                        PTTT_TOTAL_AMOUNT = s.Sum(o => o.PTTT_TOTAL_AMOUNT),
                        SA_AMOUNT = s.Sum(o => o.SA_AMOUNT),
                        SA_TOTAL_AMOUNT = s.Sum(o => o.SA_TOTAL_AMOUNT),
                        TDCN_AMOUNT = s.Sum(o => o.TDCN_AMOUNT),
                        TDCN_TOTAL_AMOUNT = s.Sum(o => o.TDCN_TOTAL_AMOUNT),
                        VLTL_AMOUNT = s.Sum(o => o.VLTL_AMOUNT),
                        VLTL_TOTAL_AMOUNT = s.Sum(o => o.VLTL_TOTAL_AMOUNT),
                        XN_AMOUNT = s.Sum(o => o.XN_AMOUNT),
                        XN_TOTAL_AMOUNT = s.Sum(o => o.XN_TOTAL_AMOUNT),
                        XQ_AMOUNT = s.Sum(o => o.XQ_AMOUNT),
                        XQ_TOTAL_AMOUNT = s.Sum(o => o.XQ_TOTAL_AMOUNT)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListData(object data)
        {
            try
            {
                if (data != null)
                {
                    ProcessRdo((List<D_HIS_SERE_SERV>)data);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessRdo(List<D_HIS_SERE_SERV> listSs)
        {
            try
            {
                if (IsNotNullOrEmpty(listSs) && IsNotNullOrEmpty(ListServiceRetyCat))
                {
                    var grCategory = ListServiceRetyCat.GroupBy(o => o.REPORT_TYPE_CAT_ID).ToList();
                    foreach (var grService in grCategory)
                    {
                        if (!IsNotNullOrEmpty(listSs)) break;

                        var retyCat = ListReportTypeCat.FirstOrDefault(o => o.ID == grService.First().REPORT_TYPE_CAT_ID);
                        if (retyCat == null || !lstTypeCatCode.Contains(retyCat.CATEGORY_CODE)) continue;

                        var lstSereServ = listSs.Where(o => grService.Select(s => s.SERVICE_ID).Contains(o.SERVICE_ID)).ToList();
                        if (IsNotNullOrEmpty(lstSereServ))
                        {
                            foreach (var item in lstSereServ)
                            {
                                Mrs00696RDO rdo = new Mrs00696RDO();

                                rdo.REQUEST_DEPARTMENT_ID = item.TDL_REQUEST_DEPARTMENT_ID;
                                rdo.REQUEST_DEPARTMENT_NAME = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                rdo.REQUEST_DEPARTMENT_CODE = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;

                                ProcessDataPrice(retyCat, item, rdo);
                                lock (ListRdo)
                                {
                                    ListRdo.Add(rdo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDataPrice(HIS_REPORT_TYPE_CAT retyCat, D_HIS_SERE_SERV sereServ, Mrs00696RDO rdo)
        {
            try
            {
                if (IsNotNull(sereServ) && IsNotNull(retyCat))
                {
                    decimal PRICE_FEE = 0;
                    decimal PRICE_SERVICE = 0;

                    decimal TotalPrice = sereServ.VIR_PRICE ?? 0;

                    if (sereServ.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DV && !sereServ.PRIMARY_PATIENT_TYPE_ID.HasValue)
                    {
                        List<V_HIS_SERVICE_PATY> patys = new List<V_HIS_SERVICE_PATY>();
                        if (MANAGER.Config.HisServicePatyCFG.DicServicePaty != null && MANAGER.Config.HisServicePatyCFG.DicServicePaty.ContainsKey(sereServ.SERVICE_ID))
                        {
                            patys = MANAGER.Config.HisServicePatyCFG.DicServicePaty[sereServ.SERVICE_ID];
                        }
                        else
                        {
                            patys = MANAGER.Config.HisServicePatyCFG.DATAs;
                        }

                        var currentPaty = MOS.ServicePaty.ServicePatyUtil.GetApplied(patys, this.branch_id, null, sereServ.TDL_REQUEST_ROOM_ID, sereServ.TDL_REQUEST_DEPARTMENT_ID, sereServ.TDL_INTRUCTION_TIME, sereServ.TREATMENT_IN_TIME ?? 0, sereServ.SERVICE_ID, sereServ.TREATMENT_TDL_PATIENT_TYPE_ID ?? 0, null);
                        if (currentPaty != null)
                        {
                            PRICE_FEE = currentPaty.PRICE * (1 + currentPaty.VAT_RATIO);
                        }
                    }
                    else
                    {
                        if (sereServ.HEIN_LIMIT_PRICE.HasValue && (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH))
                        {
                            PRICE_FEE = sereServ.HEIN_LIMIT_PRICE.Value;
                        }
                        else if (sereServ.LIMIT_PRICE.HasValue)
                        {
                            PRICE_FEE = sereServ.LIMIT_PRICE.Value;
                        }
                        else if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                            || sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                        {
                            PRICE_FEE = TotalPrice;
                        }
                    }

                    PRICE_SERVICE = TotalPrice - PRICE_FEE;

                    //Y nghia la co chech lech thi tach ra
                    if (PRICE_SERVICE == 0 && sereServ.HEIN_LIMIT_PRICE.HasValue && TotalPrice > sereServ.HEIN_LIMIT_PRICE)
                    {
                        //khi có chênh lệch thì phần chênh lệch chỉ dồn sang khi là dịch vụ khám hoặc giường.
                        if (sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || sereServ.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                        {
                            PRICE_FEE = sereServ.HEIN_LIMIT_PRICE.Value;
                            PRICE_SERVICE = TotalPrice - PRICE_FEE;
                        }
                    }

                    switch (retyCat.CATEGORY_CODE)
                    {
                        case CT:
                            rdo.CT_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.CT_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case DSA:
                            rdo.DSA_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.DSA_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case GPB:
                            rdo.GPB_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.GPB_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case G:
                            rdo.G_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.G_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case HHTM:
                            rdo.HHTM_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.HHTM_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case K:
                            rdo.K_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.K_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case MRI:
                            rdo.MRI_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.MRI_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case NS:
                            rdo.NS_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.NS_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case PTTT:
                            rdo.PTTT_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.PTTT_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case SA:
                            rdo.SA_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.SA_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case TDCN:
                            rdo.TDCN_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.TDCN_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case VLTL:
                            rdo.VLTL_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.VLTL_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case XN:
                            rdo.XN_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.XN_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        case XQ:
                            rdo.XQ_AMOUNT = PRICE_SERVICE * sereServ.AMOUNT;
                            rdo.XQ_TOTAL_AMOUNT = TotalPrice * sereServ.AMOUNT;
                            break;
                        default:
                            break;
                    }
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
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                ListRdo = ListRdo.OrderBy(o => o.REQUEST_DEPARTMENT_CODE).ToList();

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
