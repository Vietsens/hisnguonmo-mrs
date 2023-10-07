using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00882
{
    class Mrs00882Processor : AbstractProcessor
    {
        List<Mrs00882RDO> listData = new List<Mrs00882RDO>();
        Mrs00882Filter filter = null;
        public Mrs00882Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00882Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00882Filter)this.reportFilter;
            bool result = true;
            try
            {
                listData = new ManagerSql().GetList(filter);
            }
            catch(Exception e)
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
                if(listData != null)
                {
                    foreach (var item in listData)
                    {
                        item.MONTH_STR = item.MONTH.ToString().Substring(4, 2);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", listData);
        }
    }
}
