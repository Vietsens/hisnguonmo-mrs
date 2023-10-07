using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00726
{
    class Mrs00726Processor : AbstractProcessor
    {
        Mrs00726Filter filter = null;
        List<Mrs00726RDO> ListRdo = new List<Mrs00726RDO>();
        CommonParam paramGet = new CommonParam();

        public Mrs00726Processor(CommonParam param, string reportTypeCode)
				: base(param, reportTypeCode)
		{
		}

        public override Type FilterType()
        {
            return typeof(Mrs00726Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            filter = (Mrs00726Filter)base.reportFilter;
            try
            {
                ListRdo = new ManagerSql().GetRdo(filter);
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
            bool result = true;

            try
            {
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData<Mrs00726RDO>(store, "Report", ListRdo.OrderBy(p => p.ROOM_CODE).ToList());
            objectTag.AddObjectData<Mrs00726RDO>(store, "Department", ListRdo.GroupBy(p => p.DEPARTMENT_CODE).Select(p => p.First()).OrderBy(p => p.DEPARTMENT_CODE).ToList());
            objectTag.AddRelationship(store, "Department", "Report", "DEPARTMENT_CODE", "DEPARTMENT_CODE");
        }
    }
}
