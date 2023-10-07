using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00435
{
    public class Mrs00435RDO
    {
        public V_HIS_TREATMENT TREATMENT { get;  set;  }
        public V_HIS_TREATMENT_FEE TREATMENT_FEE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string ADDRESS { get;  set;  }
        public string STORE_CODE  { get;  set;  }
        public string END_DEPARTMENT_NAME { get;  set;  }
        public decimal MONEY_EXAM { get;  set;  }
        public decimal MONEY_BED { get;  set;  }
        public decimal MONEY_TEST { get;  set;  }
        public decimal MONEY_XQUANG_CC { get;  set;  }
        public decimal MONEY_XQUANG { get;  set;  }
        public decimal MONEY_CT { get;  set;  }
        public decimal MONEY_SA { get;  set;  }
        public decimal MONEY_DND { get;  set;  }
        public decimal MONEY_DTD { get;  set;  }
        public decimal MONEY_NS { get;  set;  }
        public decimal MONEY_TT { get;  set;  }
        public decimal MONEY_MEDICINE { get;  set;  }
        public decimal MONEY_MATERIAL { get;  set;  }
        public decimal MONEY_HCG { get;  set;  }
        public decimal MONEY_HIV { get;  set;  }  
        public decimal MONEY_APHETAMIN { get;  set;  }
        public decimal MONEY_SAO_BA { get;  set;  }
        public decimal MONEY_DCT_5 { get;  set;  }
        public decimal MONEY_DCT_20 { get;  set;  }
        public decimal MONEY_FREE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public decimal TOTAL_PRICE_2 { get;  set;  }
        public decimal DCT { get;  set;  }
        public Mrs00435RDO() { }
        
    }
}
