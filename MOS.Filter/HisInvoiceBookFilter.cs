
namespace MOS.Filter
{
    public class HisInvoiceBookFilter : FilterBase
    {
        public string SYMBOL_CODE__EXACT { get; set; }
        public string TEMPLATE_CODE__EXACT { get; set; }

        public HisInvoiceBookFilter()
            : base()
        {
        }
    }
}
