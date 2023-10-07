using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00051
{
    class Mrs00051RDO
    {
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }
        public string REQUEST_ROOM_NAME { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SERVICE_REQ_CODE { get;  set;  }
        public string EXECUTE_USERNAME { get;  set;  }
        public string FINISH_TIME_STR { get;  set;  }

        public decimal AMOUNT { get;  set;  }
    }
}
