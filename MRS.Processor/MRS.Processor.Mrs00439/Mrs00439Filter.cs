using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00439
{
    public class Mrs00439Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian khó viện phí
        public long TIME_TO { get;  set;  }


        public string CATEGORY_CODE__NT { get; set; }

        public string CATEGORY_CODE__HH { get; set; }

        public string CATEGORY_CODE__VS { get; set; }

        public string CATEGORY_CODE__SH { get; set; }

        public string CATEGORY_CODE__GP { get; set; }

        public string CATEGORY_CODE__NORMAL { get; set; }

        public string CATEGORY_CODE__HARD { get; set; }

        public string CATEGORY_CODE__PTLT { get; set; }

        public string CATEGORY_CODE__SU_M { get; set; }

        public string CATEGORY_CODE__SU_E { get; set; }

        public string CATEGORY_CODE__XQ { get; set; }

        public string CATEGORY_CODE__MRI { get; set; }

        public string CATEGORY_CODE__CT { get; set; }

        public string CATEGORY_CODE__SURG { get; set; }
    }
}
