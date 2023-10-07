using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00727
{
    class Mrs00727RDO
    {
        public long DEBATE_TIME { get; set; }
        public string DEBATE_TIME_STR { get; set; }
        public string PATIENT_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string DEBATE_DOCTORs { get; set; }
        public long DEBATE_TYPE_ID { get; set; }
        public string DEBATE_TYPE_CODE { get; set; }
        public string DEBATE_TYPE_NAME { get; set; }
        public long CONTENT_TYPE { get; set; }
        public string CONTENT_TYPE_NAME { get; set; }

        public string PATIENT_TYPE_NAME { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
    }
}
