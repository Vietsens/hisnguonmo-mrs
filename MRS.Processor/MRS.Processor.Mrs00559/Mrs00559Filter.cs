using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00559
{
    public class Mrs00559Filter
    {
        public long? CREATE_TIME_FROM { get;  set;  }
        public long? CREATE_TIME_TO { get;  set;  }
        public long? INTRUCTION_TIME_FROM { get;  set;  }
        public long? INTRUCTION_TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }

        public List<long> EXAM_ROOM_IDs { get;  set;  }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public long? INPUT_DATA_ID_TIME_TYPE { set; get; }
        public List<long> BRANCH_IDs { get; set; }
        public long? BRANCH_ID { get; set; }

        public Mrs00559Filter()
            : base()
        {
        }
    }
}
