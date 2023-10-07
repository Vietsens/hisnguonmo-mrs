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

namespace MRS.Processor.Mrs00616
{
    public class Mrs00616Processor : AbstractProcessor
    {
        Mrs00616Filter castFilter = null;
        private List<Mrs00616RDO> ListRdo = new List<Mrs00616RDO>();
        List<HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>();
        List<HIS_REPORT_TYPE_CAT> ListReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        CommonParam paramGet = new CommonParam();
        public Mrs00616Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00616Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00616Filter)reportFilter);
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

                if (IsNotNullOrEmpty(serviceIds))
                {
                    var skip = 0;
                    while (serviceIds.Count - skip > 0)
                    {
                        var listIDs = serviceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //YC
                        HisServiceReqFilterQuery filterSr = new HisServiceReqFilterQuery();
                        filterSr.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                        filterSr.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                        filterSr.HAS_EXECUTE = true;
                        filterSr.SERVICE_REQ_STT_ID = 3;
                        var listServicereqSub = new HisServiceReqManager(paramGet).Get(filterSr);
                        if (IsNotNullOrEmpty(listServicereqSub))
                            ListServiceReq.AddRange(listServicereqSub);
                        //YC - DV
                        HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                        filterSs.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                        filterSs.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                        filterSs.HAS_EXECUTE = true;
                        filterSs.SERVICE_IDs = listIDs;
                        var listSereServSub = new HisSereServManager(paramGet).Get(filterSs);
                        if (IsNotNullOrEmpty(listSereServSub))
                            ListSereServ.AddRange(listSereServSub);
                    }
                    ListSereServ = ListSereServ.Where(o => ListServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                }


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
                List<Mrs00616RDO> listRdo = new List<Mrs00616RDO>();
                if (ListSereServ != null)
                {
                    foreach (var item in ListSereServ)
                    {
                        var serviceReq = ListServiceReq.FirstOrDefault(o => o.ID == item.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
                        Mrs00616RDO rdo = new Mrs00616RDO(item, serviceReq);
                        
                        listRdo.Add(rdo);
                    }

                }
                ListRdo = listRdo.GroupBy(o => new { o.HisSereServ.SERVICE_ID, o.HisServiceReq.EXECUTE_LOGINNAME }).Select(p => new Mrs00616RDO(p.First().HisSereServ, p.First().HisServiceReq) { AMOUNT = p.Sum(s => s.HisSereServ.AMOUNT) }).ToList();
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00616Filter)this.reportFilter).TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00616Filter)this.reportFilter).TIME_TO ?? 0));
            if (ListReportTypeCat != null)
            {
                dicSingleTag.Add("CATEGORY_NAME", ((ListReportTypeCat.FirstOrDefault()??new HIS_REPORT_TYPE_CAT()).CATEGORY_NAME??"").ToUpper());
            }
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.HisSereServ.TDL_SERVICE_TYPE_ID).ThenBy(p => p.HisSereServ.TDL_SERVICE_CODE).ToList());
        }
       
    }
}
