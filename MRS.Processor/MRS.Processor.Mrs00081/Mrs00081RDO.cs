using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00081
{
    class Mrs00081RDO
    {
        public string MEDICINE_TYPE_CODE { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }

        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal IMP_AMOUNT { get;  set;  }
        public decimal EXP_AMOUNT { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }
        public decimal? IMP_PRICE { get;  set;  }

        public Mrs00081RDO(V_HIS_MEST_PERIOD_METY periodMaty)
        {
            try
            {
                MEDICINE_TYPE_CODE = periodMaty.MEDICINE_TYPE_CODE; 
                MEDICINE_TYPE_NAME = periodMaty.MEDICINE_TYPE_NAME; 
                SERVICE_UNIT_NAME = periodMaty.SERVICE_UNIT_NAME; 
                BEGIN_AMOUNT = periodMaty.BEGIN_AMOUNT; 
                IMP_AMOUNT = periodMaty.IN_AMOUNT; 
                EXP_AMOUNT = periodMaty.OUT_AMOUNT; 
                END_AMOUNT = periodMaty.VIR_END_AMOUNT ?? 0; 
                IMP_PRICE = periodMaty.IMP_PRICE; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
