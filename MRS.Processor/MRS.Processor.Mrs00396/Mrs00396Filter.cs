using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00396
{
    public class Mrs00396Filter
    {
     public long TIME_FROM { get;  set;  }
     public long TIME_TO { get; set; }
     public long? CLINICAL_DEPARTMENT_ID { get; set; }
     public bool? IS_THIS_YEAR { get; set; }
     public bool? IS_WITHIN_A_YEAR { get; set; }
    }
}
	