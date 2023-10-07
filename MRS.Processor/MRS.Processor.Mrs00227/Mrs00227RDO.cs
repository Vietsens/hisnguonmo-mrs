using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00227
{
    public class Mrs00227RDO
    {
        public string CREATE_TIME_STRING { get;  set;  }
        public HIS_BID V_HIS_BID { get;  set;  }
        public decimal TOTAL_BIDS { get;  set;  }
    }
}
