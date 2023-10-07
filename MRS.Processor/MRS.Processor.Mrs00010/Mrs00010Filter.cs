using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00010
{
    class Mrs00010Filter
    {
        public long? CREATE_TIME_FROM { get;  set;  }
        public long? CREATE_TIME_TO { get;  set;  }
        public long? INTRUCTION_TIME_FROM { get;  set;  }
        public long? INTRUCTION_TIME_TO { get;  set;  }

        public List<long> EXAM_ROOM_IDs { get;  set;  }
        public List<long> ROOM_IDs
        {
            get
            {
                if (this.EXAM_ROOM_IDs != null) return this.EXAM_ROOM_IDs; 
                else return null; 
            }
            set { }
        }
        public Mrs00010Filter()
            : base()
        {
        }
    }
}
