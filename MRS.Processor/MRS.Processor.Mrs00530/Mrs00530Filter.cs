using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00530
{
    public class Mrs00530Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public string CATEGORY_CODE__ECG { get; set; }
        public string CATEGORY_CODE__BRAIN_BLOOD { get; set; }
        public string CATEGORY_CODE__CERVICAL_ENDO { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }

        public short? TREATMENT_STT_ID { get; set; }
    }
}