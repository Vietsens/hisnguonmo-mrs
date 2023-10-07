using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00086
{
    class Mrs00086RDO
    {
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string IS_BHYT { get; set; }

        public decimal AMOUNT { get; set; }
        public int? MALE_AGE { get; set; }
        public int? FEMALE_AGE { get; set; }
        public string FEMALE_YEAR { get; set; }
        public string MALE_YEAR { get; set; }
    }
}
