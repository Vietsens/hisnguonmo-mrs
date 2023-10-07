using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00740
{
    class Mrs00740Processor : AbstractProcessor
    {
        List<Mrs00740RDO> listData = new List<Mrs00740RDO>();
        List<Mrs00740RDO> listRdo = new List<Mrs00740RDO>();
        Mrs00740Filter filter = new Mrs00740Filter();
        public Mrs00740Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        
        public override Type FilterType()
        {
            return typeof(Mrs00740Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                filter = ((Mrs00740Filter)this.reportFilter);
                listData = new ManagerSql().GetData(filter);
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
                        Mrs00740RDO rdo = new Mrs00740RDO();
                        rdo.BLOOD_TYPE_NAME = item.BLOOD_TYPE_NAME;
                        rdo.BLOOD_TYPE_CODE = item.BLOOD_TYPE_CODE;
                        rdo.BLOOD_GROUP = item.BLOOD_ABO_CODE + "" + item.BLOOD_RH_CODE;
                        rdo.EXECUTE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXECUTE_TIME);
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXPIRED_DATE ?? 0);
                        System.DateTime? expire_time = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.EXPIRED_DATE ?? 0);
                        TimeSpan tdiff = expire_time.Value - DateTime.Now;
                        rdo.DAY_LEFT = tdiff.Days;
                        rdo.IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.IMP_TIME ?? 0);
                        rdo.IMP_MEST_TYPE_NAME = item.IMP_MEST_TYPE_NAME;
                        listRdo.Add(rdo);
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
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(p => p.DAY_LEFT).ToList());
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }
    }
}
