using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00718
{
    public class Mrs00718RDO
    {
        public string PR_SERVICE_CODE { get; set; }
        public string PR_SERVICE_NAME { get; set; }

        public string SV_SERVICE_CODE { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string SV_SERVICE_NAME { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public int COUNT_TOTAL { get; set; }
        public int PR_COUNT_TOTAL { get; set; }

        public Dictionary<string, int> DIC_PATIENT_TYPE { get; set; }

        public Dictionary<string, int> DIC_TREATMENT_TYPE { get; set; }
    }
}
