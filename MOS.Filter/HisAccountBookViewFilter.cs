using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisAccountBookViewFilter : FilterBase
    {
        public string ACCOUNT_BOOK_CODE { get; set; }
        public string ACCOUNT_BOOK_NAME { get; set; }
        public List<long> TRANSACTION_TYPE_IDs { get; set; }
        public bool? IS_OUT_OF_BILL { get; set; }
        public bool? FOR_DEPOSIT { get; set; }
        public bool? FOR_REPAY { get; set; }
        public bool? FOR_BILL { get; set; }

        public HisAccountBookViewFilter()
            : base()
        {

        }
    }
}
