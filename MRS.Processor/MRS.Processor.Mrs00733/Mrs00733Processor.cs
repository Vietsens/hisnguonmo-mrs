using IMSys.DbConfig.HIS_RS;
using Inventec.Core;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00733
{
    class Mrs00733Processor : AbstractProcessor
    {
        List<Mrs00733RDO> ListRdo = new List<Mrs00733RDO>();
        Mrs00733Filter filter = null;
        
        public Mrs00733Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00733Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                ListRdo = new ManagerSql().GetMedicine() ?? new List<Mrs00733RDO>();
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
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(p => p.ACTIVE_INGR_BHYT_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
