using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00822
{
    public class Mrs00822RDO
    {
        public long PARENT_SERVICE_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public long  SERVICE_TYPE_ID { get; set; }
        public long HEIN_SERVICE_TYPE_ID { get; set; }
        public string HEIN_SERVICE_TYPE_NAME { get; set; }
        public string HEIN_SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal  BHYT_PRICE { get; set; }
        public decimal VP_PRICE { get; set; }
        public decimal BHYT_AMOUNT { get; set; }
        public decimal VP_AMOUNT { get; set; }
        public decimal YC_AMOUNT { get; set; }

        public decimal VIR_PRICE { get; set; } // đơn giá
    }
}
