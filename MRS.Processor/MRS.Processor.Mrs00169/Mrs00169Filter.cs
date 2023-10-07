using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00169
{
    public class Mrs00169Filter
    {
        public long DATE_FROM { get; set; }
        public long DATE_TO { get; set; }
        public long? MEDI_ORG_ID { get; set; }
        public List<long> MEDI_ORG_IDs { get; set; }
        public bool CHECK_INTIME_OUTTIME { set; get; }

        public long? DEPARTMENT_ID { get; set; }
        public List<long> ROOM_IDs { set; get; }
    }
}
