using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00821
{
    public class Mrs00821RDO
    {
        public long SERE_SERV_ID { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public decimal EXPEND_PRICE { get; set; }
        public decimal TOTAL_EARN_PRICE { get; set; }
    }
}
