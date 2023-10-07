using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00555
{
    public class Mrs00555Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public bool? IS_TREAT_IN { get; set; }
        public bool? IS_EXAM_INFO { get; set; }
        public bool? IS_EXAM { get; set; }
        public string DEPARTMENT_CODE__KKB { get; set; }
        public string DEPARTMENT_CODE__KCCs { get; set; }
        public string DEPARTMENT_CODE__OUTPATIENTs { get; set; }
        public string ROOM_CODE__CC { get; set; }
        public string ROOM_CODE__SAN { get; set; }
        public bool? IS_DETAIL_TREATMENT { get; set; }
        public string KEY_DETAIL_TREATMENT { get; set; }
        public bool? IS_DETAIL_TREATMENT_BEDROOM { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public bool? IS_COUNT_BED_LOG { get; set; }
        public List<long> TDL_REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> INPUT_DATA_ID_STTs { get; set; }
    }
}
