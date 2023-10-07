using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00037
{
    class Mrs00037RDO
    {
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal IMP_PRICE_1000 { get;  set;  }
        public decimal AMOUNT_LS { get;  set;  }
        public decimal AMOUNT_CLS { get;  set;  }
        public decimal AMOUNT_KKB { get;  set;  }
        public decimal AMOUNT_K { get;  set;  }
        public decimal AMOUNT_H { get;  set;  }

        public Mrs00037RDO(string code, string name, string unitname, decimal price)
        {
            try
            {
                MATERIAL_TYPE_CODE = code; 
                MATERIAL_TYPE_NAME = name; 
                SERVICE_UNIT_NAME = unitname; 
                IMP_PRICE_1000 = price / 1000; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
