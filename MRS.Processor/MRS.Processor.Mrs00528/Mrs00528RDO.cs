using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00528
{
    class Mrs00528RDO
    {
        public string SERVICE_NAME { get; set; }
        public decimal PRICE { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal VAT_RATIO { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public long HEIN_SERVICE_TYPE_NUM_ORDER { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
    }
}
