using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00420
{
    class Mrs00420RDO
    {
        //public string REQUEST_ROOM_NAME { get;  set;  }

        public string REQUEST_DEPARTMENT_NAME { get;  set;  }
        public decimal TOTAL_ECG { get;  set;  }
        public decimal TOTAL_SUIM { get;  set;  }
        public decimal TOTAL_CST { get;  set;  }
        
        public Mrs00420RDO() { }
        
    }

    public class MY_PATIENT_TYPE_ALTER
    {
        public long ID { get;  set;  }
        public long TREATMENT_TYPE_ID { get;  set;  }
        public long LOG_IN_TIME { get;  set;  }
        public long LOG_OUT_TIME { get;  set;  }
    }
}
