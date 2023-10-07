using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00316
{
    public class Mrs00316Filter
    {
     public long TIME_FROM { get;  set;  }
     public long TIME_TO { get; set; }
     public long? CLINICAL_DEPARTMENT_ID { get; set; }
     public List<string> CLINICAL_DEPARTMENT_CODEs { get; set; }

				
    }
}
	