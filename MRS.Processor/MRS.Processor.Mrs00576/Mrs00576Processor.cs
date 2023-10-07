using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00576
{
    class Mrs00576Processor : AbstractProcessor
    {
        Mrs00576Filter castFilter = null;
        List<Mrs00576RDO> ListRdo = new List<Mrs00576RDO>();

        public Mrs00576Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00576Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                var param = new CommonParam();
                castFilter = (Mrs00576Filter)this.reportFilter;

                ListRdo = new ManagerSql().GetBaby(castFilter);
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

            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
