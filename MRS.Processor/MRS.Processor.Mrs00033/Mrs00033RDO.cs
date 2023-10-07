using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00033
{
    class Mrs00033RDO
    {
        public string EXECUTE_DEPARTMENT_CODE { get;  set;  }
        public string EXECUTE_DEPARTMENT_NAME { get;  set;  }

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
