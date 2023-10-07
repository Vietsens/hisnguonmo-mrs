using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.Filter; 

namespace MRS.Processor.Mrs00406
{
    public class Mrs00406Filter: FilterBase
    {       
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }             
    }
}
