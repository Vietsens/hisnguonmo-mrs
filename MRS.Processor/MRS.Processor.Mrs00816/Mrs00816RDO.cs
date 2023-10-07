using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00816
{
    public class Mrs00816RDO
    {
        public long  EXP_TIME { get; set; }
        public string EXP_TIME_STR { get; set; }
        public decimal AMOUNT { set; get; }
        public decimal PRICE { set; get; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal TOTAL_DISCOUNT_PRICE { get; set; }
        public decimal TOTAL_CUSTOMER_PRICE { get; set; }
        public decimal TOTAL_CUSTOMER_DISCOUNT_PRICE { get; set; }
        public string  EXP_MEST_CODE { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public long TRANSACTION_DATE { get; set; }
    }
}
