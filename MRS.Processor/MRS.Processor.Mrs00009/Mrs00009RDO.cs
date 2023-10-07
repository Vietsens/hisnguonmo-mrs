using Inventec.Common.Logging; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00009
{
    class Mrs00009RDO
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

        public Mrs00009RDO(string code, string name, string unitName, decimal impPrice)
        {
            try
            {
                MATERIAL_TYPE_CODE = code; 
                MATERIAL_TYPE_NAME = name;
                SERVICE_UNIT_NAME = unitName;
                PRICE = impPrice; 
                IMP_PRICE_1000 = impPrice / 1000; 
                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00009RDO data)
        {

        }

        public long REQ_DEPARTMENT_ID { get; set; }

        public decimal OUT_EXP_AMOUNT { get; set; }

        public decimal IN_EXP_AMOUNT { get; set; }

        public decimal USE_EXP_AMOUNT { get; set; }

        public decimal OTHER_AMOUNT { get; set; }

        public decimal PRICE { get; set; }
    }
}
