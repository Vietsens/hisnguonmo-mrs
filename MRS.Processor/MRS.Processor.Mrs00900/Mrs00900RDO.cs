using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00900
{
    class Mrs00900RDO
    {
        public long SERVICE_ID { get; set; }
        public long PARENT_SERVICE_ID { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
    }

    class LIST_FOR_PROCESS 
    {
        public long? SERVICE_REQ_ID { get; set; }
        public long? SERVICE_REQ_STT_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? TRAN_PATI_REASON_ID { get; set; }
        public long? TRAN_PATI_TECH_ID { get; set; }
        public long? TREATMENT_DAY_COUNT { get; set; }
        public decimal AGE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }
        public string TDL_PATIENT_TYPE_NAME_OTHER { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string TDL_TREATMENT_TYPE_NAME { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
        public long? TRAN_PATI_FORM_ID { get; set; }
        public long? DEATH_WITHIN_ID { get; set; }
        public long? PTTT_GROUP_ID { get; set; }
        public string PT_GROUP_NAME { get; set; }
        public string TT_GROUP_NAME { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public long SERVICE_ID { get; set; }
        public long? PTTT_CATASTROPHE_ID { get; set; }
        public string HEIN_APPROVAL_CODE { get; set; }
    }
}
