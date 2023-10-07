using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00427
{
    public class Mrs00427RDO
    {
        public long? IMP_EXP_TIME { get;  set;  }
        public string IMP_MEST_CODE { get;  set;  }
        public string EXP_MEST_CODE { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public long? EXPIRED_DATE { get;  set;  }
        public string DESCRIPTION { get;  set;  }
        //public string MEST_TYPE_NAME { get;  set;  }
        //public string SECOND_MEDI_STOCK_NAME { get;  set;  }
       // public string SUPPLIER_NAME { get;  set;  }
        //public string REQ_DEPARTMENT_NAME { get;  set;  }
        public string SECOND_MEDI_STOCK_NAME { get;  set;  }
        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }
        
        public Mrs00427RDO() { }
    }
}
