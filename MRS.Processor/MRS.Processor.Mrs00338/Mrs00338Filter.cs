using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00338
{
    public class Mrs00338Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public string CATEGORY_CODE__KHAC { get; set; }
        public string CATEGORY_CODE__XHH { get; set; }
        public string ICD_DTD { get; set; }
    }
}
