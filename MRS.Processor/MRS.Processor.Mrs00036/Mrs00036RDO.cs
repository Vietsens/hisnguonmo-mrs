using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00036
{
    class Mrs00036RDO
    {
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal IMP_PRICE_1000 { get;  set;  }
        public decimal AMOUNT_CLS { get;  set;  }
        public decimal AMOUNT_LS { get;  set;  }
        public decimal AMOUNT_KKB { get;  set;  }
        public decimal AMOUNT_K { get;  set;  }
        public decimal AMOUNT_H { get;  set;  }

        public Mrs00036RDO(string code, string name, string unitname, decimal impprice)
        {
            try
            {
                MEDICINE_TYPE_CODE = code; 
                MEDICINE_TYPE_NAME = name; 
                SERVICE_UNIT_NAME = unitname; 
                IMP_PRICE_1000 = impprice / 1000; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
