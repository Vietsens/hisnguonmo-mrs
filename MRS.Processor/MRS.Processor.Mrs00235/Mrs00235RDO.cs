using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00235
{
    public class Mrs00235RDO
    {
        public long? REQ_DEPARTMENT_ID { get; set; }
        public string REQ_DEPARTMENT_NAME { get; set; }
        public string REQ_DEPARTMENT_CODE { get; set; }
        public long? IMP_DEPARTMENT_ID { get; set; }
        public string IMP_DEPARTMENT_NAME { get; set; }
        public string IMP_DEPARTMENT_CODE { get; set; }
        public long? REQ_ROOM_ID { get; set; }
        public string REQ_ROOM_NAME { get; set; }
        public string REQ_ROOM_CODE { get; set; }
        public string IMP_MEDI_STOCK_NAME { get; set; }
        public string IMP_MEDI_STOCK_CODE { get; set; }
        public string EXP_MEST_CODE { get; set; }
        public long? EXP_MEST_ID { get; set; }
        public decimal? TOTAL_PRICE { get; set; }
        public long EXP_MEST_TYPE_ID { get; set; }
        public long? CHMS_TYPE_ID { get; set; }
        public string EXP_MEST_TYPE_NAME { get; set; }
        public string EXP_MEST_REASON_NAME { get; set; }
        public long? EXP_TIME { get; set; }
        public string OTHER_PAY_SOURCE_NAME { get; set; }

        public long INTRUCTION_TIME { get; set; }
        public string EXP_TIME_STR { get; set; }

        public string PATIENT_CODE { get; set; }

        public string PATIENT_NAME { get; set; }

        public string TREATMENT_CODE { get; set; }

        public string PATIENT_TYPE_NAME { get; set; }
        public string TT_PATIENT_TYPE_NAME { get; set; }
        public string PATIENT_CLASSIFY_NAME { get; set; }

        public string MEDI_MATE_CODE { get; set; }

        public string MEDI_MATE_NAME { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public decimal? AMOUNT { get; set; }

        public decimal? PRICE { get; set; }

        public decimal? VAT_RATIO { get; set; }

        public decimal? IMP_TOTAL_PRICE { get; set; }

        public string ICD_NAME { get; set; }

        public string ICD_CODE { get; set; }

        public string ICD_SUB_CODE { get; set; }

        public string ICD_TEXT { get; set; }

        public decimal? TOTAL_REUSABLE_EXP { get; set; }
    }
}
