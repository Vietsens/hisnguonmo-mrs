
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisInvoiceDetailFilter : FilterBase
    {
        public List<long> INVOICE_IDs { get; set; }
        public long? INVOICE_ID { get; set; }
        
        public HisInvoiceDetailFilter()
            : base()
        {
        }
    }
}
