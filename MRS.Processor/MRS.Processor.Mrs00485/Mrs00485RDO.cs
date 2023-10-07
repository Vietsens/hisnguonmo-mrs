using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00485
{
    public class Mrs00485RDO
    {
        public V_HIS_TREATMENT TREATMENT { get;  set;  }
        public V_HIS_TREATMENT_FEE TREATMENT_FEE { get;  set;  }
        public string TRANSACTION_CODE { get;  set;  }
        public long TRANSACTION_TIME { get;  set;  }
        public decimal HEIN_05 { get;  set;  }
        public decimal HEIN_20 { get;  set;  }
        public decimal HEIN_40 { get;  set;  }
        public decimal SERVICE_TYPE_01 { get;  set;  }
        public decimal SERVICE_TYPE_02 { get;  set;  }
        public decimal SERVICE_TYPE_03 { get;  set;  }
        public decimal SERVICE_TYPE_04 { get;  set;  }
        public decimal SERVICE_TYPE_05 { get;  set;  }
        public decimal SERVICE_TYPE_06 { get;  set;  }
        public decimal SERVICE_TYPE_07 { get;  set;  }
        public decimal SERVICE_TYPE_08 { get;  set;  }
        public decimal SERVICE_TYPE_09 { get;  set;  }
        public decimal SERVICE_TYPE_10 { get;  set;  }
        public decimal SERVICE_TYPE_11 { get;  set;  }
        public decimal SERVICE_TYPE_12 { get;  set;  }
        public decimal SERVICE_TYPE_13 { get;  set;  }
        public decimal SERVICE_TYPE_14 { get;  set;  }
        public decimal SERVICE_TYPE_15 { get;  set;  }
        public decimal SERVICE_TYPE_16 { get;  set;  }
        public decimal SERVICE_TYPE_17 { get;  set;  }
        public decimal SERVICE_TYPE_18 { get;  set;  }
        public decimal SERVICE_TYPE_19 { get;  set;  }
        public decimal SERVICE_TYPE_20 { get;  set;  }
        public decimal REDUNDANCY { get;  set;  }         // thừa
        public decimal SHORT { get;  set;  }              // thiếu
    }
}
