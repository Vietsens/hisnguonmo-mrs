using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.Filter;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00189
{
    internal class Mrs00189Processor : AbstractProcessor
    {
        List<VSarReportMrs00189RDO> _listSarReportMrs00189Rdos = new List<VSarReportMrs00189RDO>();
        Mrs00189Filter CastFilter;

        string thisReportTypeCode = "";
        public Mrs00189Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00189Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00189Filter)this.reportFilter;

                //-------------------------------------------------------------------------------------------------- V_HIS_BILL
                var metyFilterServiceReqView = new HisServiceReqViewFilterQuery
                {
                    START_TIME_FROM = CastFilter.START_TIME_FROM,
                    START_TIME_TO = CastFilter.START_TIME_TO,
                    SERVICE_REQ_TYPE_IDs = CastFilter.SERVICE_REQ_TYPE_IDs,
                };
                var listServiceReqs = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).GetView(metyFilterServiceReqView);

                var hhhhh = listServiceReqs.Select(s => s.START_TIME).OrderBy(s => s).ToList();
                //--------------------------------------------------------------------------------------------------

                ProcessFilterData(listServiceReqs);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        { return true; }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.START_TIME_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.START_TIME_TO));

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00189Rdos);
        }

        private void ProcessFilterData(List<V_HIS_SERVICE_REQ> listServiceReqs)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00189 ===============================================================");

                var aaaa = listServiceReqs.Select(s => s.START_TIME).ToList();
                var ssss = listServiceReqs.Select(s => s.INTRUCTION_TIME).ToList();
                var oooo = listServiceReqs.Where(s => aaaa.Contains(s.START_TIME)).Average(s => s.START_TIME);
                var kkkk = listServiceReqs.Where(s => ssss.Contains(s.INTRUCTION_TIME)).Average(s => s.INTRUCTION_TIME);
                var eeee = oooo - kkkk;
                VSarReportMrs00189RDO rdo = new VSarReportMrs00189RDO
                {
                    TG_TRUNG_BINH = eeee / 360

                };
                _listSarReportMrs00189Rdos.Add(rdo);

                LogSystem.Info("Ket thuc xu ly du lieu MRS00189 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }
    }
}
