using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00382
{
    class Mrs00382AgeRDO
    {
        
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        
        public long TDL_PATIENT_AGE { get; set; }

        public decimal IMP_PRICE { get; set; }

        public decimal? EXP_PRICE { get; set; }

        public long MEDICINE_TYPE_ID { get; set; }

        public decimal AMOUNT { get; set; }

        public decimal UNDER_6_AMOUNT { get; set; }

        public decimal FROM_6_TO_15_AMOUNT { get; set; }

        public decimal MORE_THAN_15_AMOUNT { get; set; }

        public long REQUEST_ROOM_ID { get; set; }

        public string REQ_ROOM_CODE { get; set; }

        public string REQ_ROOM_NAME { get; set; }

        public long REQUEST_DEPARTMENT_ID { get; set; }

        public string REQ_DEPARTMENT_CODE { get; set; }

        public string REQ_DEPARTMENT_NAME { get; set; }

        public decimal IMP_VAT_RATIO { get; set; }

        public decimal? VAT_RATIO { get; set; }

        public string CONCENTRA { get; set; }
    }
}
