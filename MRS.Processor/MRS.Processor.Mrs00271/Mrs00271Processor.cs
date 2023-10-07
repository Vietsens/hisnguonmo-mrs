using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 

namespace MRS.Processor.Mrs00271
{
    public class Mrs00271Processor : AbstractProcessor
    {
        private List<Mrs00271RDO> _listMrs00271Rdos = new List<Mrs00271RDO>(); 

        public Mrs00271Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00271Filter); 
        }

        protected override bool GetData()
        {
			var filter = ((Mrs00271Filter)reportFilter); 
            var result = true; 
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

        protected override bool ProcessData()
        {
            var result = true; 
            try
            {
                
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            objectTag.AddObjectData(store, "Report", _listMrs00271Rdos); 
        }
    }
}
