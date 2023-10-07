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

namespace MRS.Processor.Mrs00262
{
    public class Mrs00262Processor : AbstractProcessor
    {
        private List<Mrs00262RDO> listRDO = new List<Mrs00262RDO>(); 
        private List<V_HIS_INVOICE> ListInvoice = new List<V_HIS_INVOICE>(); 
         private List<HIS_INVOICE_DETAIL> ListInvoiceDetail = new List<HIS_INVOICE_DETAIL>(); 
        CommonParam paramGet = new CommonParam(); 
        public Mrs00262Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00262Filter); 
        }

        protected override bool GetData()
        {
			var filter = ((Mrs00262Filter)reportFilter); 
            var result = true; 
            try
            {
                HisInvoiceViewFilterQuery filterInvoice = new HisInvoiceViewFilterQuery() 
                {
                    INVOICE_TIME_FROM = filter.TIME_FROM,
                    INVOICE_TIME_TO = filter.TIME_TO
                }; 
                ListInvoice = new HisInvoiceManager(paramGet).GetView(filterInvoice);

                var invoiceIds = ListInvoice.Select(o => o.ID).Distinct().ToList();
                if (IsNotNullOrEmpty(invoiceIds))
                {
                    var skip = 0;
                    while (invoiceIds.Count - skip > 0)
                    {
                        var listIds = invoiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisInvoiceDetailFilterQuery filterInvoiceDetail = new HisInvoiceDetailFilterQuery();
                        filterInvoiceDetail.INVOICE_IDs = listIds;
                        ListInvoiceDetail.AddRange(new HisInvoiceDetailManager(paramGet).Get(filterInvoiceDetail));
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

        protected override bool ProcessData()
        {
            var result = true; 
            try
            {
                listRDO.Clear(); 
                var groupByVatRatio = ListInvoice.GroupBy(o => o.VAT_RATIO??0).ToList(); 
                foreach (var group in groupByVatRatio)
                {
                    List<V_HIS_INVOICE> listSub = group.ToList<V_HIS_INVOICE>(); 
                    
                        Mrs00262RDO rdo = new Mrs00262RDO(); 

                        rdo.VAT_RATIO = listSub.First().VAT_RATIO ?? 0; 
                        rdo.VIR_TOTAL_PRICE = ListInvoiceDetail.Where(o =>listSub.Select(p=>p.ID).Contains(o.INVOICE_ID)).Sum(o => o.VIR_TOTAL_PRICE); 
                        
                        listRDO.Add(rdo); 
                  
                }
                
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
            if (((Mrs00262Filter)reportFilter).TIME_FROM != null) dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00262Filter)reportFilter).TIME_FROM)); 
            if (((Mrs00262Filter)reportFilter).TIME_TO != null) dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00262Filter)reportFilter).TIME_TO)); 
listRDO = listRDO.OrderBy(o=>o.VAT_RATIO).ToList(); 
            objectTag.AddObjectData(store, "VatRatio", listRDO); 
            }
    }
}
