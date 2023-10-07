using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00701
{
    public class Mrs00701RDO
    {
        public long SERVICE_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public string KEY { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public long NUMBER_OF_FILM { get; set; }
        public decimal SERVICE_NUMBER_OF_FILM { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal PRICE { get; set; }
    }
}
