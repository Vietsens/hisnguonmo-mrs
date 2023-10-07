using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00723
{
    class Mrs00723RDO
    {
        public string IN_TIME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_DOB { get; set; }
        public string PATIENT_ADDRESS { get; set; }
        public string PATIENT_CAREER { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public int PATIENT_COUNT { get; set; }
    }
}
