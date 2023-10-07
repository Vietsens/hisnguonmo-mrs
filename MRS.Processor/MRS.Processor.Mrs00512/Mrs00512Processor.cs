using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00512;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00512
{
    public class Mrs00512Processor : AbstractProcessor
    {
        private List<Mrs00512RDO> ListRdo = new List<Mrs00512RDO>();
        private List<Mrs00512RDO> ListParent = new List<Mrs00512RDO>();
        Mrs00512Filter filter = null;
        List<HIS_SERVICE_REQ> HisServiceReqs = new List<HIS_SERVICE_REQ>();
        string thisReportTypeCode = "";

        List<HIS_SERE_SERV> HisSereServs = new List<HIS_SERE_SERV>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        public Mrs00512Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00512Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00512Filter)this.reportFilter;
            try
            {

                
                HisSereServs = new ManagerSql().GetSereServDO(filter);
                HisServiceReqs = new ManagerSql().GetServiceReqDO(filter);
                if (!string.IsNullOrWhiteSpace(this.filter.TEST_DEPARTMENT_CODEs))
                { 
                     List<long> testDepartmentIds = (HisDepartmentCFG.DEPARTMENTs ?? new List<HIS_DEPARTMENT>()).Where(o => (this.filter.TEST_DEPARTMENT_CODEs ?? "").Split(',').Contains(o.DEPARTMENT_CODE)).Select(p => p.ID).ToList();
                     HisSereServs = HisSereServs.Where(o => testDepartmentIds.Contains(o.TDL_EXECUTE_DEPARTMENT_ID)).ToList();
                }
                var serviceIds = HisSereServs.Select(x => x.SERVICE_ID).Distinct().ToList();
                HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery();
                HisServiceRetyCatfilter.REPORT_TYPE_CODE__EXACT = this.thisReportTypeCode;
                HisServiceRetyCatfilter.SERVICE_IDs = serviceIds;
                listServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter);
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
            bool result = false;
            try
            {
                if (HisSereServs != null)
                {
                    ListRdo = (from r in listServiceRetyCat select new Mrs00512RDO(r, HisSereServs, HisServiceReqs)).ToList();
                    
                    ListParent = ListRdo.GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).ToList();
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00512RDO>();
                result = false;
            }
            return result;
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            if (this.filter.REQUEST_DEPARTMENT_IDs != null)
            {
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAMEs", string.Join(" - ", (HisDepartmentCFG.DEPARTMENTs.Where(o => this.filter.REQUEST_DEPARTMENT_IDs.Contains(o.ID)).ToList() ?? new List<HIS_DEPARTMENT>()).Select(p => p.DEPARTMENT_NAME).ToList()));
            }
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Parent", ListParent);
            objectTag.AddRelationship(store, "Parent", "Report", "CATEGORY_CODE", "CATEGORY_CODE");
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
        }

    }

}
