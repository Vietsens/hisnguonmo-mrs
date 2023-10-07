using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00638
{
    public class Mrs00638Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public bool? IS_TREATING_OR_FEE_LOCK { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public string KEY_GROUP_SV { get; set; }
    }
}