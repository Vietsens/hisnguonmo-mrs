using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00430
{
    public class Mrs00430Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public string LOGINNAME { get; set; }
        public List<string> LOGINNAMEs { get; set; }
        public long BRANCH_ID { get; set; }
        public bool? IS_DETAIL_PF { get; set; }
    }
}
	