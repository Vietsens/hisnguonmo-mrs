using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00354
{
    public class Mrs00354Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }

        public string SERVICE_CODE__ECGs { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }

        public int? INPUT_DATA_ID_TIME_TYPE { get; set; }

        public bool? IS_SWAP_DEPARTMENT_AND_DEPARTMENTYC { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }
    }
}
