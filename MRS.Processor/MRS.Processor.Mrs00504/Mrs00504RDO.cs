using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Core; 

namespace MRS.Processor.Mrs00504
{
    public class Mrs00504RDO
    {
        public long ID { get;  set;  }
        public string INDEX { get;  set;  }

        public decimal TNT { get;  set;  }
        public decimal CAPD { get;  set;  }
        public decimal KKB { get;  set;  }
        public decimal NTR { get;  set;  }

        public decimal HEIN_TNT { get;  set;  }
        public decimal HEIN_CAPD { get;  set;  }
        public decimal HEIN_KKB { get;  set;  }
        public decimal HEIN_NTR { get;  set;  }

        public decimal FEE_TNT { get;  set;  }
        public decimal FEE_CAPD { get;  set;  }
        public decimal FEE_KKB { get;  set;  }
        public decimal FEE_NTR { get;  set;  }

        public decimal PRICE_TNT { get;  set;  }
        public decimal PRICE_CAPD { get;  set;  }
        public decimal PRICE_KKB { get;  set;  }
        public decimal PRICE_NTR { get;  set;  }

        public Mrs00504RDO() { }
    }
}
