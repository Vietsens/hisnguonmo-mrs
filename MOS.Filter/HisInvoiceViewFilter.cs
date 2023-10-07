
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisInvoiceViewFilter : FilterBase
    {
        public long? INVOICE_BOOK_ID { get; set; }
        public string SYMBOL_CODE__EXACT { get; set; }
        public List<string> CREATORs { get; set; }
        public long? INVOICE_TIME_FROM { get; set; }
        public long? INVOICE_TIME_TO { get; set; }

        public HisInvoiceViewFilter()
            : base()
        {
        }
    }
}
