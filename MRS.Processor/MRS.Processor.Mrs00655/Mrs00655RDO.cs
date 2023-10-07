using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00655
{
    class Mrs00655RDO
    {

        public long? ACCOUNT_BOOK_ID { get; set; }
        public string ACCOUNT_BOOK_CODE { get; set; }
        public string ACCOUNT_BOOK_NAME { get; set; }
        public string TEMPLATE_CODE { get; set; }
        public string SYMBOL_CODE { get; set; }
        public long? NUM_ORDER_FROM { get; set; }
        public long? NUM_ORDER_TO { get; set; }
        public decimal? TOTAL_PRICE { get; set; }
        public long? CANCEL_NUM_ORDER { get; set; }
        public long? TRANSACTION_DATE { get; set; }
        public decimal? CANCEL_TOTAL_PRICE_IN_TIME { get; set; }// số tiền nhân viên hoàn trả trong thời gian báo cáo, cùng phòng, cùng nhân viên
        public decimal? CANCEL_TOTAL_PRICE_OUT_TIME { get; set; }// số tiền nhân viên hoàn trả khác thời gian báo cáo, khác phòng, khác nhân viên
    }
}
