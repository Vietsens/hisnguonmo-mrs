using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00444
{
    public class Mrs00444Filter
    {
     public long TIME_FROM { get;  set;  }
     public long TIME_TO { get;  set;  }
     public long? DEPARTMENT_ID { get;  set;  }
     public bool? IS_TREAT_IN { get; set; }			
    }
}
	