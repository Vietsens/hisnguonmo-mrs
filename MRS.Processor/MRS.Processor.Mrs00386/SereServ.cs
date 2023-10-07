using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00386
{
    class SereServCat
    {
        public long TDL_INTRUCTION_TIME { get; set; }
        public long TDL_TREATMENT_ID { get; set; }
        public long SERVICE_REQ_STT_ID { get; set; }
        public long REPORT_TYPE_CAT_ID { get; set; }
        public decimal AMOUNT { get; set; }
    }
    class SereServType
    {
        public long TDL_INTRUCTION_TIME { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public decimal AMOUNT { get; set; }
    }

    class SereServPrice
    {
        public long TIME { get; set; }
        public int COUNT_TREATMENT { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
    }
}
