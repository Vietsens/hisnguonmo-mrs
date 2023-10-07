using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00666
{
    public class Mrs00666Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }

        public long? MEDI_STOCK_ID { get; set; }

        public List<long> MEDI_STOCK_IDs { get; set; }

        public List<long> MEDICINE_TYPE_IDs { get; set; }

        public string ACTIVE_INGR_BHYT { get; set; } 

        public Mrs00666Filter() { }
    }
}
