using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00622
{
    public class Mrs00622RDO
    {
        public string IN_MONTH { get; set; }
        public string ICD_CODE { get; set; }	
        public string ICD_NAME { get; set; }	
        public int COUNT_TREAT { get; set; }	
        public long DAY { get; set; }
        public int COUNT_KHOI { get; set; }
        public int COUNT_DO { get; set; }
        public int COUNT_KTD { get; set; }
        public int COUNT_NANG { get; set; }
        public int COUNT_CV { get; set; }
        public int COUNT_CHET { get; set; }
        public int COUNT_BH_NT { get; set; }
        public int COUNT_BH_NGT { get; set; }
        public int COUNT_VP_NT { get; set; }
        public int COUNT_VP_NGT { get; set; }
        public int COUNT_CV_NT { get; set; }
        public int COUNT_CV_NGT { get; set; }
        public int COUNT_VV { get; set; }// vào viện
        public int COUNT_NAM { get; set; }
        public int COUNT_NU { get; set; }
        public int COUNT_TE_DUOI15 { get; set; }


    }
   
}
