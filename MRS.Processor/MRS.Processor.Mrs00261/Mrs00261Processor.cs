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

namespace MRS.Processor.Mrs00261
{
    public class Mrs00261Processor : AbstractProcessor
    {
        private List<Mrs00261RDO> listRDO = new List<Mrs00261RDO>(); 
        private List<V_HIS_INVOICE> ListInvoice = new List<V_HIS_INVOICE>(); 
        private List<Mrs00261RDO> listVatRatio = new List<Mrs00261RDO>(); 
        private List<HIS_INVOICE_DETAIL> ListInvoiceDetail = new List<HIS_INVOICE_DETAIL>(); 
        CommonParam paramGet = new CommonParam(); 
        public Mrs00261Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00261Filter); 
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00261Filter)reportFilter); 
            var result = true; 
            try
            {
                HisInvoiceViewFilterQuery filterInvoice = new HisInvoiceViewFilterQuery()
                {
                    INVOICE_TIME_FROM = filter.TIME_FROM,
                    INVOICE_TIME_TO = filter.TIME_TO,

                }; 
                ListInvoice = new HisInvoiceManager(paramGet).GetView(filterInvoice);
                if (IsNotNullOrEmpty(ListInvoice))
                {
                    ListInvoice = ListInvoice.Where(o => o.IS_CANCEL != (short)1).ToList();
                }

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
                var groupByVatRatio = ListInvoice.GroupBy(o => o.VAT_RATIO ?? 0).ToList(); 
                foreach (var group in groupByVatRatio)
                {
                    List<V_HIS_INVOICE> listSub = group.ToList<V_HIS_INVOICE>(); 
                    foreach (var inVoice in listSub)
                    {
                        Mrs00261RDO rdo = new Mrs00261RDO()
                        {
                            VIR_NUM_ORDER = inVoice.VIR_NUM_ORDER,
                            INVOICE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(inVoice.INVOICE_TIME),
                            BUYER_NAME = inVoice.BUYER_NAME,
                            BUYER_ACCOUNT_NUMBER = inVoice.BUYER_ACCOUNT_NUMBER,
                            VAT_RATIO = inVoice.VAT_RATIO ?? 0,
                            VIR_TOTAL_PRICE = ListInvoiceDetail.Where(o => o.INVOICE_ID == inVoice.ID).Sum(o => o.VIR_TOTAL_PRICE)
                        }; 
                        listRDO.Add(rdo); 
                    }
                }
                listVatRatio = listRDO.GroupBy(o => o.VAT_RATIO).Select(p => p.First()).ToList(); 
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
            if (((Mrs00261Filter)reportFilter).TIME_FROM != null) dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00261Filter)reportFilter).TIME_FROM)); 
            if (((Mrs00261Filter)reportFilter).TIME_TO != null) dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00261Filter)reportFilter).TIME_TO)); 
            dicSingleTag.Add("SUM_VIR_TOTAL_PRICE", listRDO.Sum(p => p.VIR_TOTAL_PRICE)); 
            dicSingleTag.Add("SUM_VIR_TOTAL_PRICE_0", listRDO.Where(o => o.VAT_RATIO == 0).Sum(p => p.VIR_TOTAL_PRICE)); 
            dicSingleTag.Add("SUM_VIR_TOTAL_PRICE_1", listRDO.Sum(p => p.VIR_TOTAL_PRICE) - listRDO.Where(o => o.VAT_RATIO == 0).Sum(p => p.VIR_TOTAL_PRICE)); 
            listVatRatio = listVatRatio.OrderBy(o => o.VAT_RATIO).ToList(); 
            objectTag.AddObjectData(store, "VatRatio", listVatRatio); 
            objectTag.AddObjectData(store, "Report", listRDO); 
            objectTag.AddRelationship(store, "VatRatio", "Report", "VAT_RATIO", "VAT_RATIO"); 
        }
    }
}
