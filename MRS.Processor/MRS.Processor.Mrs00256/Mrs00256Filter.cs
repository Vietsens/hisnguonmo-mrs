using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00256
{
    public class Mrs00256Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? BID_ID { get; set; }
        public List<long> BID_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }

        public string KEY_GROUP_BID_DETAIL { get; set; }

        public List<long> SUPPLIER_IDs { get; set; }
    }
}
