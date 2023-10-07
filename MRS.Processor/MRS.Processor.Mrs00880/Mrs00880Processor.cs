using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00880
{
    class Mrs00880Processor : AbstractProcessor
    {
        Mrs00880Filter filter = null;
        List<Mrs00880RDO> listData = new List<Mrs00880RDO>();
        List<Mrs00880RDO> listRdo = new List<Mrs00880RDO>();
        public Mrs00880Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00880Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                filter = (Mrs00880Filter)reportFilter;
                CommonParam paramGet = new CommonParam();
                listData = new ManagerSQL().GetListRdo(filter) ?? new List<Mrs00880RDO>();
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (listData != null)
                {
                    foreach (var item in listData)
                    {
                        item.EMPLOYEE_ID_STR = item.EMPLOYEE_ID.ToString();
                    }
                    
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("MONTH", Inventec.Common.DateTime.Convert.TimeNumberToMonthString(filter.MONTH ?? 0));
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", listData);
        }
    }
}
