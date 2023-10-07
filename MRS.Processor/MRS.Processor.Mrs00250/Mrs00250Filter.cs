using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00250
{
    public class Mrs00250Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public string LOGINNAME { get; set; }
        public bool? IS_REMOVE_DUPLICATE_PRES { get; set; }
        public bool? IS_THROW_KSK_OTHER_SOURCE { get; set; }//b? khám s?c kh?e và ngu?n khác
    }
}
