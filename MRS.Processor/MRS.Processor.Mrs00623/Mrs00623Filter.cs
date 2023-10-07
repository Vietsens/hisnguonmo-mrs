using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00623
{
    public class Mrs00623Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public long? REPORT_TYPE_CAT_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }

        public Mrs00623Filter()
            : base()
        {

        }
    }
}
