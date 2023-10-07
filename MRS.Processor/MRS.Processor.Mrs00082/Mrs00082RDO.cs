using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00082
{
    class Mrs00082RDO
    {
        public string MATERIAL_TYPE_CODE { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }
        public decimal? IMP_PRICE { get;  set;  }

        public Mrs00082RDO(V_HIS_MEST_PERIOD_MATY periodMaty)
        {
            try
            {
                MATERIAL_TYPE_CODE = periodMaty.MATERIAL_TYPE_CODE; 
                MATERIAL_TYPE_NAME = periodMaty.MATERIAL_TYPE_NAME; 
                SERVICE_UNIT_NAME = periodMaty.SERVICE_UNIT_NAME; 
                BEGIN_AMOUNT = periodMaty.BEGIN_AMOUNT; 
                IMP_AMOUNT = periodMaty.IN_AMOUNT; 
                EXP_AMOUNT = periodMaty.OUT_AMOUNT; 
                END_AMOUNT = periodMaty.VIR_END_AMOUNT ?? 0; 
                IMP_PRICE = periodMaty.IMP_PRICE; 
                //SetImpPrice(periodMaty); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
