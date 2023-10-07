using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00386
{
    public class DepartmentInOut
    {
        public long ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public long? DEPARTMENT_IN_TIME { get; set; }
        public long? NEXT_ID { get; set; }
        public long? NEXT_DEPARTMENT_ID { get; set; }
        public long? NEXT_DEPARTMENT_IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? OUT_DATE { get; set; }
        public string TREATMENT_ICD_CODE { get; set; }
        public string TREATMENT_ICD_NAME { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public short? IS_PAUSE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
    }
}
