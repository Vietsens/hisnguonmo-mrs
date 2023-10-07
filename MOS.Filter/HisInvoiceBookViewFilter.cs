
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisInvoiceBookViewFilter : FilterBase
    {
        public string SYMBOL_CODE__EXACT { get; set; }
        public string TEMPLATE_CODE__EXACT { get; set; }
        public long? LINK_ID { get; set; }
        public List<long> LINK_IDs { get; set; }

        public HisInvoiceBookViewFilter()
            : base()
        {
        }
    }
}
