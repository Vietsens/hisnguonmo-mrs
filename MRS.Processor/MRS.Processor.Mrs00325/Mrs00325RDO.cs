using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00325
{
    public class Mrs00325RDO
    {
        public string PROVINCE_NAME { get;  set;  }
      
        public decimal NUMBER_PARTIENT{get; set; }
        public decimal NUMBER_BHYT { get;  set;  }

        
        // Biến phụ
        public long PATIENT_TYPE_ID { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
      

  
    }
}
