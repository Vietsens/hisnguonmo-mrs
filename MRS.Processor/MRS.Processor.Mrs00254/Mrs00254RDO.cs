using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00254
{
    public class Mrs00254RDO
    {
        public string CREATE_DATE_STR { get; set; }
        public string TRANSACTION_DATE_STR { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DESCRIPTION { get;  set;  }
        public string CASHIER_USERNAME { get;  set;  }
        public string IS_OUT { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public string DOB { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string GENDER_NAME { get;  set;  }
        
    }
}
