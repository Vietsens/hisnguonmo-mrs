using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.DateTime; 
using MRS.MANAGER.Config; 
using FlexCel.Report; 

namespace MRS.Processor.Mrs00712
{
    public class Mrs00712Processor : AbstractProcessor
    {
        private Mrs00712Filter filter;
        List<Mrs00712RDO> listCountTreatment = new List<Mrs00712RDO>();
        CommonParam paramGet = new CommonParam(); 
        public Mrs00712Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00712Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00712Filter)reportFilter; 
            try
            {
                listCountTreatment = new MRS.Processor.Mrs00712.ManagerSql().GetCount(filter) ?? new List<Mrs00712RDO>();
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
               
            }
            catch (Exception ex)
            {
                result = false; 
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }

       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store, "Report", listCountTreatment);
            dicSingleTag.Add("PTTT_GROUP_ID__GROUP1", HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1);
            dicSingleTag.Add("PTTT_GROUP_ID__GROUP2", HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2);
            dicSingleTag.Add("PTTT_GROUP_ID__GROUP3", HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3);
            dicSingleTag.Add("PTTT_GROUP_ID__GROUP4", HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4);
        }

       
    }
}
