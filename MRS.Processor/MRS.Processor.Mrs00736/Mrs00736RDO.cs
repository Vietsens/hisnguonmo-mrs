using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00736
{
    class Mrs00736RDO
    {
        public long TRANSACTION_TIME { get; set; }
        //public long TRANSACTION_DATE { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public long ID { get; set; } //ID của treatment
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public long? OTHER_PAY_SOURCE_ID { get; set; }
        public string OTHER_PAY_SOURCE_CODE { get; set; }
        public string OTHER_PAY_SOURCE_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        //public decimal? OTHER_SOURCE_PRICE { get; set; }

        public decimal? TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public string DOB_YEAR { get; set; }
    }
}
