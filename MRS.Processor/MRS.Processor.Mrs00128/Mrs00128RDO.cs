using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00128
{
    class Mrs00128RDO
    {
        public string EXP_TIME { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public decimal TOTAL_AMOUNT { get;  set;  }
        public decimal? TOTAL_DISCOUNT { get;  set;  }
        public decimal? TOTAL_PRICE { get;  set;  }

        public Mrs00128RDO() { }

        public Mrs00128RDO(Mrs00128RDO rdo)
        {
            if (rdo != null)
            {
                EXP_TIME = rdo.EXP_TIME; 
                MEDICINE_TYPE_NAME = rdo.MEDICINE_TYPE_NAME; 
                TOTAL_AMOUNT = rdo.TOTAL_AMOUNT; 
                TOTAL_DISCOUNT = rdo.TOTAL_DISCOUNT; 
                TOTAL_PRICE = rdo.TOTAL_PRICE; 
            }
        }
    }
}
