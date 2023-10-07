using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00398
{
    public class Mrs00398Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }

        public List<long> TREATMENT_END_TYPE_IDs { get; set; }

        public bool? IS_NOT_TRANSFER { get; set; }
        public string DEPARTMENT_CODE__OUTPATIENTs { get; set; }
        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }

        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public List<long> PATIENT_CLASSIFY_IDs { get; set; }//đối tượng chi tiết
    }
}
