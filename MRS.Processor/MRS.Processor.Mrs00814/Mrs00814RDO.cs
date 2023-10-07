using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00814
{
    public class Mrs00814RDO
    {
        public long  TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { set; get; }
        public string TREATMENT_NAME { get; set; }
        public string IN_TIME { set; get; }
        public string OUT_TIME { set; get; }
        public string ICD_CODE { set; get; }
        public string ICD_NAME { get; set; }
        public long TREATMENT_END_TYPE_ID { get; set; }
        public string TREATMENT_END_TYPE_CODE { set; get; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public long TREATMENT_RESULT_ID { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_RESULT_NAME { get; set; }
        public long PATIENT_ID { get; set; }
        public string  PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_DOB { set; get; }
        public long AGE { get; set; }
        public string PATIENT_GENDER { get; set; }
        public string   DEPARTMENT_CODE{set;get;}
        public string DEPARTMENT_NAME { get; set; }
        public decimal DAY_USE_BED { set; get; }
    }
}
