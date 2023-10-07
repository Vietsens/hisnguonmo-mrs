using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00269
{
    public class Mrs00269Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }

        public List<long> EXAM_ROOM_IDs { get; set; }

        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public string DEPARTMENT_CODE__OUTPATIENTs { get; set; }
    }
}
