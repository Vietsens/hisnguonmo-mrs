using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00609
{
    public class Mrs00609Filter : Mrs00609RDO
    {
        public long OUT_TIME_FROM { get; set; }
        public long OUT_TIME_TO { get; set; }
        public bool? IS_TREAT { get; set; }

        public List<long> END_DEPARTMENT_IDs { get; set; }

        public List<long> OTHER_PAY_SOURCE_IDs { get; set; }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }

        public List<string> ICD_CODEs { get; set; }

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }

        public List<long> TDL_TREATMENT_TYPE_IDs { get; set; }
    }
}
