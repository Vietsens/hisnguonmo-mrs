using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00820
{
    public class Mrs00820Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }

        public List<long> PATIENT_CLASSIFY_IDs { get; set; }

        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public string EXECUTE_ROOM_CODE__PKKs { get; set; }

        public string EXECUTE_ROOM_CODE__KCCs { get; set; }
    }
}
