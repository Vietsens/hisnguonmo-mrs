
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisTransactionFilter : FilterBase
    {
        public List<long> CASHIER_ROOM_IDs { get; set; }
        public long? TRANSACTION_TYPE_ID { get; set; }
        public long? TREATMENT_ID { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public long? ACCOUNT_BOOK_ID { get; set; }
        public long? PAY_FORM_ID { get; set; }
        public List<long> PAY_FORM_IDs { get; set; }
        public long? CASHIER_ROOM_ID { get; set; }
        public long? CASHOUT_ID { get; set; }
        public List<long> CASHOUT_IDs { get; set; }
        public long? TRANSACTION_TIME_FROM { get; set; }
        public long? TRANSACTION_TIME_TO { get; set; }
        public long? CANCEL_TIME_FROM { get; set; }
        public long? CANCEL_TIME_TO { get; set; }
        public long? NUM_ORDER_FROM { get; set; }
        public long? NUM_ORDER_TO { get; set; }
        public bool? HAS_CASHOUT { get; set; }
        public long? NUM_ORDER__EQUAL { get; set; }

        public bool? HAS_TDL_SERE_SERV_DEPOSIT { get; set; }
        public bool? HAS_TDL_SESE_DEPO_REPAY { get; set; }

        public bool? IS_CANCEL { get; set; }

        public bool? HAS_SALL_TYPE { get; set; }

        public HisTransactionFilter()
            : base()
        {
        }
    }
}
