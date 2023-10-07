using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00385
{
    public class Mrs00385RDO
    {
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal IMP_PRICE_1000 { get;  set;  }
        public decimal AMOUNT_LS { get;  set;  }
        public decimal AMOUNT_CLS { get;  set;  }
        public decimal AMOUNT_KKB { get;  set;  }
        public decimal AMOUNT_K { get;  set;  }
        public decimal AMOUNT_H { get; set; }
        public Dictionary<string, decimal> DIC_REASON { get; set; }
        public Dictionary<string, decimal> DIC_REQ_DEPARTMENT { get; set; }

        public Mrs00385RDO(string code, string name, string unitName, decimal impPrice, string nationalName)
        {
            try
            {
                MATERIAL_TYPE_CODE = code;
                MATERIAL_TYPE_NAME = name;
                SERVICE_UNIT_NAME = unitName;
                NATIONAL_NAME = nationalName;
                IMP_PRICE_1000 = impPrice / 1000;
                DIC_REASON = new Dictionary<string, decimal>();
                DIC_REQ_DEPARTMENT = new Dictionary<string, decimal>();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public decimal AMOUNT_KHAC { get; set; }
        public decimal AMOUNT_NOI_TRU { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_HPKP { get; set; }
        public decimal AMOUNT_XUATXA { get; set; }
        public decimal AMOUNT_KHAC_NT { get; set; }
        public short? IS_CHEMICAL_SUBSTANCE { get; set; }

        public string PARENT_MATERIAL_TYPE_CODE { get; set; }

        public string PARENT_MATERIAL_TYPE_NAME { get; set; }
        public long PARENT_MATERIAL_TYPE_ID { get; set; }
        public string NATIONAL_NAME { get; set; }
    }
}
