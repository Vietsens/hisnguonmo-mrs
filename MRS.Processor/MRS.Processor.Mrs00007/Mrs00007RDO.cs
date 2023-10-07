using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00007
{
    class Mrs00007RDO
    {
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string CONCENTRA { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal IMP_PRICE_1000 { get;  set;  }
        public decimal AMOUNT_LS { get;  set;  }
        public decimal AMOUNT_CLS { get;  set;  }
        public decimal AMOUNT_KKB { get;  set;  }
        public decimal AMOUNT_K { get;  set;  }
        public decimal AMOUNT_H { get;  set;  }
        public decimal OUT_EXP_AMOUNT { get;  set;  }
        public decimal IN_EXP_AMOUNT { get;  set;  }	
        public decimal USE_EXP_AMOUNT { get;  set;  }	
        public decimal OTHER_AMOUNT { get;  set;  }

        public long REQ_DEPARTMENT_ID { get;  set;  }

        public Mrs00007RDO(string code, string name, string unitName, string concentra, decimal impPrice)
        {
            try
            {
                MEDICINE_TYPE_CODE = code; 
                MEDICINE_TYPE_NAME = name; 
                SERVICE_UNIT_NAME = unitName; 
                CONCENTRA = concentra; 
                PRICE = impPrice; 
                IMP_PRICE_1000 = impPrice / 1000; 
                SetExtendField(this); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void SetExtendField(Mrs00007RDO data)
        {

        }

        public decimal AMOUNT_BHYT { get; set; }

        public decimal AMOUNT_BHYT_HT { get; set; }

        public decimal AMOUNT_VP { get; set; }

        public decimal AMOUNT_VP_HT { get; set; }

        public decimal AMOUNT_FREE { get; set; }

        public decimal AMOUNT_FREE_HT { get; set; }

        public decimal AMOUNT_HPKP { get; set; }

        public decimal AMOUNT_HPKP_HT { get; set; }

        public string MEDICINE_GROUP_CODE { get; set; }

        public string MEDICINE_GROUP_NAME { get; set; }
    }
}
