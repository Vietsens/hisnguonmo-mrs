
namespace MOS.Filter
{
    public class HisUserInvoiceBookViewFilter : FilterBase
    {
        public string LOGINNAME { get; set; }
        public long? INVOICE_BOOK_ID { get; set; }

        public HisUserInvoiceBookViewFilter()
            : base()
        {
        }
    }
}
