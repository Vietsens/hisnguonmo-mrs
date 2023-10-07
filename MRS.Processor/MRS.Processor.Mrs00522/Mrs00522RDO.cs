using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00522
{
    class Mrs00522RDO
    {
        public string SERVICE_STT_DMBYT { get; set; }
        public string SERVICE_CODE_DMBYT { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal? ORIGINAL_PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal HEIN_PRICE { get; set; }
        public long? SERVICE_ID { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
    }
}
