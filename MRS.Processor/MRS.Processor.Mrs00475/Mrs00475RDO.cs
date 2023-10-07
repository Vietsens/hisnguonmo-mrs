using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00475
{
    class Mrs00475RDO
    {
        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }
        //public string PATIENT_TYPE_NAME { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }
        public long SERVICE_ID { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public long SERVICE_TYPE_ID { get;  set;  }
        public decimal AMOUNT_TREAT_IN { get;  set;  }
        public decimal AMOUNT_TREAT_OUT { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public Mrs00475RDO() { }
    }

    public class MY_PATIENT_TYPE_ALTER
    {
        public long ID { get;  set;  }
        public long TREATMENT_TYPE_ID { get;  set;  }
        public long LOG_IN_TIME { get;  set;  }
        public long LOG_OUT_TIME { get;  set;  }
    }
}
