using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00360
{
    public class Mrs00360RDO
    {
        public long REQUEST_DEPARTMENT_ID { get;  set;  }
        public string REQUEST_DEPARTMENT_CODE { get;  set;  }
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }

        public decimal SUIM_TOTAL_AMOUNT { get;  set;  }
        public decimal SUIM_AMOUNT_BHYT_COLOR { get;  set;  }
        public decimal SUIM_AMOUNT_BHYT_BW { get;  set;  }
        public decimal SUIM_TOTAL_PRICE_BHYT { get;  set;  }
        public decimal SUIM_AMOUNT_ND_COLOR { get;  set;  }
        public decimal SUIM_AMOUNT_ND_BW { get;  set;  }
        public decimal SUIM_TOTAL_PRICE_ND { get;  set;  }

        public decimal ECG_TOTAL_AMOUNT { get;  set;  }
        public decimal ECG_AMOUNT_BHYT { get;  set;  }
        public decimal ECG_AMOUNT_ND { get;  set;  }
        public decimal ECG_TOTAL_PRICE { get;  set;  }

        public decimal EEG_TOTAL_AMOUNT { get;  set;  }
        public decimal EEG_AMOUNT_BHYT { get;  set;  }
        public decimal EEG_AMOUNT_ND { get;  set;  }
        public decimal EEG_TOTAL_PRICE { get;  set;  }

        public decimal REG_TOTAL_AMOUNT { get;  set;  }
        public decimal REG_AMOUNT_BHYT { get;  set;  }
        public decimal REG_AMOUNT_ND { get;  set;  }
        public decimal REG_TOTAL_PRICE { get;  set;  }

        public decimal LASER_TOTAL_AMOUNT { get;  set;  }
        public decimal LASER_AMOUNT_BHYT { get;  set;  }
        public decimal LASER_AMOUNT_ND { get;  set;  }
        public decimal LASER_TOTAL_PRICE { get;  set;  }
    }
}
