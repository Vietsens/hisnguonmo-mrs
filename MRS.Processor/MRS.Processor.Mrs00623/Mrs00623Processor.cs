using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using MOS.MANAGER.HisService;

namespace MRS.Processor.Mrs00623
{
    public class Mrs00623Processor : AbstractProcessor
    {
        Mrs00623Filter castFilter = null;
        List<Mrs00623RDO> listRdo = new List<Mrs00623RDO>();
        List<HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>();
        List<HIS_REPORT_TYPE_CAT> ListReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        List<HIS_SERVICE_PATY> listHisServicePaty = new List<HIS_SERVICE_PATY>();
        CommonParam paramGet = new CommonParam();
        public Mrs00623Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00623Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00623Filter)reportFilter);
            var result = true;
            try
            {
                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery();
                reportTypeCatFilter.ID = castFilter.REPORT_TYPE_CAT_ID;
                ListReportTypeCat = new HisReportTypeCatManager().Get(reportTypeCatFilter);
                if (ListReportTypeCat != null)
                {
                    HisServiceRetyCatFilterQuery srFilter = new HisServiceRetyCatFilterQuery();
                    srFilter.REPORT_TYPE_CAT_ID = ListReportTypeCat.Select(o => o.ID).FirstOrDefault();
                    ListServiceRetyCat = new HisServiceRetyCatManager().Get(srFilter);
                }

                var serviceIds = ListServiceRetyCat.Select(s => s.SERVICE_ID).Distinct().ToList();

                //YC
                HisServiceReqFilterQuery filterSr = new HisServiceReqFilterQuery();
                filterSr.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filterSr.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                filterSr.HAS_EXECUTE = true;
                filterSr.SERVICE_REQ_STT_ID = 3;
                filterSr.REQUEST_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                var listServicereqSub = new HisServiceReqManager(paramGet).Get(filterSr);
                if (IsNotNullOrEmpty(listServicereqSub))
                    ListServiceReq.AddRange(listServicereqSub);
                //Inventec.Common.Logging.LogSystem.Info("ListServiceReq" + ListServiceReq.Count);
                if (IsNotNullOrEmpty(serviceIds))
                {
                    var skip = 0;
                    while (serviceIds.Count - skip > 0)
                    {
                        var listIDs = serviceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //YC - DV
                        HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                        filterSs.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                        filterSs.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                        filterSs.HAS_EXECUTE = true;
                        filterSs.SERVICE_IDs = listIDs;
                        filterSs.TDL_REQUEST_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                        var listSereServSub = new HisSereServManager(paramGet).Get(filterSs);
                        if (IsNotNullOrEmpty(listSereServSub))
                            ListSereServ.AddRange(listSereServSub);
                        //DV
                        HisServiceFilterQuery filterSv = new HisServiceFilterQuery();
                        filterSv.IDs = listIDs;
                        var listServiceSub = new HisServiceManager(paramGet).Get(filterSv);
                        if (IsNotNullOrEmpty(listServiceSub))
                            listHisService.AddRange(listServiceSub);
                     
                    }
                    ListSereServ = ListSereServ.Where(o => ListServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                }

                //Inventec.Common.Logging.LogSystem.Info("ListSereServ" + ListSereServ.Count);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (ListSereServ != null)
                {
                    ListSereServ = ListSereServ.Where(r=>r.VIR_PRICE>0).OrderBy(o=>o.SERVICE_ID).ThenBy(p=>p.PATIENT_TYPE_ID).ThenBy(q=>q.VIR_PRICE.Value).ToList();
                    foreach (var r in ListSereServ)
                    {
                        var serviceReq = ListServiceReq.FirstOrDefault(o => o.ID == r.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
                        var service = listHisService.FirstOrDefault(o => o.ID == r.SERVICE_ID)??new HIS_SERVICE();
                        Mrs00623RDO rdo = new Mrs00623RDO(r, serviceReq, service);
                        if (listRdo.Count == 0 || r.SERVICE_ID != listRdo[listRdo.Count - 1].SERVICE_ID || (listRdo[listRdo.Count - 1].VIR_PRICE != rdo.VIR_PRICE && listRdo[listRdo.Count - 1].PATIENT_TYPE_ID == rdo.PATIENT_TYPE_ID))
                        {
                            listRdo.Add(rdo);
                        }
                        else
                        {
                            listRdo[listRdo.Count - 1].PATIENT_TYPE_ID = r.PATIENT_TYPE_ID;
                            listRdo[listRdo.Count - 1].VIR_PRICE = r.VIR_PRICE ?? 0;
                            if (rdo.PRICE_BHYT > 0)
                            {
                                listRdo[listRdo.Count - 1].PRICE_BHYT = rdo.PRICE_BHYT;
                            }
                            else if (rdo.PRICE_TP > 0)
                            {
                                listRdo[listRdo.Count - 1].PRICE_TP = rdo.PRICE_TP;
                            }
                            else if (rdo.PRICE_MP > 0)
                            {
                                listRdo[listRdo.Count - 1].PRICE_MP = rdo.PRICE_MP;
                            }

                            listRdo[listRdo.Count - 1].VIR_TOTAL_PRICE_BHYT += rdo.VIR_TOTAL_PRICE_BHYT;
                            listRdo[listRdo.Count - 1].VIR_TOTAL_PRICE_TP += rdo.VIR_TOTAL_PRICE_TP;
                            listRdo[listRdo.Count - 1].VIR_TOTAL_PRICE_MP += rdo.VIR_TOTAL_PRICE_MP;
                            listRdo[listRdo.Count - 1].AMOUNT_BHYT += rdo.AMOUNT_BHYT;
                            listRdo[listRdo.Count - 1].AMOUNT_TP += rdo.AMOUNT_TP;
                            listRdo[listRdo.Count - 1].AMOUNT_MP += rdo.AMOUNT_MP;
                            listRdo[listRdo.Count - 1].AMOUNT_OTHER += rdo.AMOUNT_OTHER;
                           
                                listRdo[listRdo.Count - 1].AMOUNT_TE += rdo.AMOUNT_TE;
                                listRdo[listRdo.Count - 1].AMOUNT += rdo.AMOUNT;
                                listRdo[listRdo.Count - 1].VIR_TOTAL_PRICE += rdo.VIR_TOTAL_PRICE;
                        } 
                        
                    }

                }

            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00623Filter)this.reportFilter).TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00623Filter)this.reportFilter).TIME_TO ?? 0));
            if (ListReportTypeCat != null)
            {
                dicSingleTag.Add("CATEGORY_NAME", ((ListReportTypeCat.FirstOrDefault() ?? new HIS_REPORT_TYPE_CAT()).CATEGORY_NAME ?? "").ToUpper());
            }
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.SERVICE_NAME).ToList());

        }

    }
}
