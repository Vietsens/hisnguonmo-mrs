using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00741
{
    class Mrs00741RDO
    {
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string PATIENT_CLASSIFY_CODE { get; set; }
        public string PATIENT_CLASSIFY_NAME { get; set; }
        public decimal PRICE { get; set; }
        public decimal? REAL_PRICE { get; set; }
        public decimal? AMOUNT { get; set; }
        public decimal COUNT_SERVICE_CODE { get; set; }


        public decimal? TOTAL_BASIC_PRICE { get; set; }

        public decimal? TOTAL_REAL_PRICE { get; set; }
    }
}
