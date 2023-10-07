using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00673
{
    public class Mrs00673Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }//Chon phong chi dinh
        public List<long> EXECUTE_ROOM_IDs { get; set; }//chobn phong thuc hien
        public List<string> EXECUTE_LOGINNAMEs { get; set; }//nguoi thuc hien
        public List<string> REQUEST_LOGINNAMEs { get; set; }//nguoi chi dinh
    }
}
