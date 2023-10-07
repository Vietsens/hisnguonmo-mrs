using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00034
{
    public class Mrs00034RDO
    {
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }
        public string REQUEST_ROOM_CODE { get;  set;  }
        public string REQUEST_ROOM_NAME { get;  set;  }

        public decimal AMOUNT_EXAM { get;  set;  }
        public decimal AMOUNT_TEST { get;  set;  }
        public decimal AMOUNT_DIIM { get;  set;  }
        public decimal AMOUNT_MISU { get;  set;  }
        public decimal AMOUNT_FUEX { get;  set;  }
        public decimal AMOUNT_SURG { get;  set;  }
        public decimal AMOUNT_SUIM { get;  set;  }
        public decimal AMOUNT_ENDO { get;  set;  }
        
    }
}
