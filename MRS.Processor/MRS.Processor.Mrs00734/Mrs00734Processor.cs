using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00734 
{
    class Mrs00734Processor : AbstractProcessor
    {
        List<Mrs00734RDO> ListData = new List<Mrs00734RDO>();
        List<Mrs00734RDO> ListRdo = new List<Mrs00734RDO>();
        Mrs00734Filter filter = null;
        
        public Mrs00734Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00734Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00734Filter)this.reportFilter;
            bool result = true;
            try
            {
                ListData = new ManagerSql().GetTimeData(filter) ?? new List<Mrs00734RDO>();
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
                if (ListData != null)
                {
                    foreach (var item in ListData)
                    {
                        Mrs00734RDO rdo = new Mrs00734RDO();
                        rdo.LOGIN_NAME = item.LOGIN_NAME;
                        rdo.USER_NAME = item.USER_NAME;
                        rdo.LATEST_LOGIN_TIME = item.LOGIN_TIME;
                        rdo.LATEST_LOGIN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.LOGIN_TIME);
                        rdo.LOGOUT_TIME = item.ACTIVITY_TYPE_ID == 8 ? item.ACTIVITY_TIME : 0;
                        
                        
                        System.DateTime? in_t = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(rdo.LATEST_LOGIN_TIME);
                        System.DateTime? out_t = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(rdo.LOGOUT_TIME);

                        TimeSpan tdiff = out_t.Value - in_t.Value;

                        rdo.WORKING_TIME_STR = string.Format("{1} giờ {2} phút {3} giây", tdiff.Hours, tdiff.Minutes, tdiff.Seconds);
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
                objectTag.AddObjectData(store, "Report", ListRdo);
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
