
namespace MOS.Filter
{
    public class HisInvoiceFilter : FilterBase
    {
        public long? INVOICE_BOOK_ID { get; set; }
        public long? INVOICE_TIME_FROM { get; set; }
        public long? INVOICE_TIME_TO { get; set; }

        public HisInvoiceFilter()
            : base()
        {
        }
    }
}
