using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00823
{
    public class Mrs00823RDO
    {
        public decimal TREATMENT_DAY_COUNT { get; set; }
        public long  COUNT_TREATMENT { get; set; }
        public decimal  TREATMENT_DAY_COUNT_TB { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal HEIN_PRICE { get; set; }
        public decimal VP_PRICE { get; set; }
        public decimal PATIENT_PRICE_BHYT { get; set; }
        public Dictionary<string, decimal> DIC_SERVICE_BHYT { get; set; }
        public Dictionary<string, decimal> DIC_HEIN_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_PATIENT_PRICE_BHYT { get; set; }
        public Dictionary<string,decimal> DIC_VP_PRICE { get; set; }
        public long SERVICE_COUNT { get; set; }
    }
}
