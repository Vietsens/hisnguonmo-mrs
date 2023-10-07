using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00160
{
    public class Mrs00160Filter
    {
        public long? EXP_MEST_STT_ID { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public long EXP_TIME_FROM { get;  set;  }
        public long EXP_TIME_TO { get;  set;  }

    }
}
