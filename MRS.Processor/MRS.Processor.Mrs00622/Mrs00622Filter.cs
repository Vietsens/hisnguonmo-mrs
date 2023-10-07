using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00622
{
    public class Mrs00622Filter
    {
        public long TIME_FROM { get; set; }

        public long TIME_TO { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }

        public long? BRANCH_ID { get; set; }

        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public List<long> ICD_IDs { get; set; }

        public List<long> EXAM_ROOM_IDs { get; set; }

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }

        public int? INPUT_DATA_ID_TIME_TYPE { get; set; }

        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }
    }
}
