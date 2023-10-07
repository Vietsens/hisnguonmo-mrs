
namespace MOS.Filter
{
    public class HisUserInvoiceBookFilter : FilterBase
    {
        public string LOGINNAME { get; set; }
        public long? INVOICE_BOOK_ID { get; set; }

        public HisUserInvoiceBookFilter()
            : base()
        {
        }
    }
}
