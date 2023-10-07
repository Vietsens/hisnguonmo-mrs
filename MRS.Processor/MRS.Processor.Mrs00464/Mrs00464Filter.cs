using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00464
{
    public class Mrs00464Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public List<long> DEPARTMENT_IDs { get;  set;  }

        public bool? IS_TREAT_IN { get; set; }
        public string DEPARTMENT_CODE__OUTPATIENTs { get; set; }

        public string PREFIX_ICD_CODE__DTTTs { get; set; }//cac ma icd duc thuy tinh the,
        public string PREFIX_ICD_CODE__MOs { get; set; }//cac ma icd mong mat ,
        public string PREFIX_ICD_CODE__QUs { get; set; }//cac ma icd quam ,
        public string PREFIX_ICD_CODE__GLs { get; set; }//cac ma icd glocom ,

        public bool? RELATIONSHIP_METHOD { get; set; }

        public bool RELATIONSHIP_EXAM_EYE { get; set; }

        public bool RELATIONSHIP_APPOINT { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
    }
}
