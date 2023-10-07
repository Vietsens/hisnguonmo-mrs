using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00414
{
    public class Mrs00414RDO
    {
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }

        public string TREATMENT_CODE { get;  set;  }
        public long TREATMENT_ID { get;  set;  }

        public string TRANSACTION_CODE { get;  set;  }

        public string BILL_01_NUMBER { get;  set;  }
        public decimal BILL_01_AMOUNT { get;  set;  }
        public decimal BILL_01_PRICE { get;  set;  }

        public string BILL_02_NUMBER { get;  set;  }
        public decimal BILL_02_AMOUNT { get;  set;  }
        public decimal BILL_02_PRICE { get;  set;  }
    }
}
