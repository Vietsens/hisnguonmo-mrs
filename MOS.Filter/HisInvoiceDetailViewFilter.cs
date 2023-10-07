
namespace MOS.Filter
{
    public class HisInvoiceDetailViewFilter : FilterBase
    {
        public long? INVOICE_BOOK_ID { get; set; }
        public long? INVOICE_ID { get; set; }

        public HisInvoiceDetailViewFilter()
            : base()
        {
        }
    }
}
