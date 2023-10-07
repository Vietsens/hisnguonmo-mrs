using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00357
{
    public class Mrs00357RDO
    {
        public string SERVICE_STT_DMBYT { get;  set;  }
        public string SERVICE_CODE_DMBYT { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }
        public decimal AMOUNT_NGOAITRU { get;  set;  }
        public decimal AMOUNT_NOITRU { get;  set;  }
        public decimal? VIR_PRICE { get;  set;  }
        public decimal VIR_TOTAL_PRICE { get;  set;  }

        public long? SERVICE_ID { get;  set;  }

        public Mrs00357RDO() { }

        public Mrs00357RDO(V_HIS_SERE_SERV_3 data)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID; 
                this.SERVICE_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE; 
                this.SERVICE_STT_DMBYT = data.TDL_HEIN_ORDER; 
                this.SERVICE_TYPE_NAME = data.TDL_HEIN_SERVICE_BHYT_NAME;
                this.VIR_PRICE = data.ORIGINAL_PRICE * (1 + data.VAT_RATIO);
                this.VIR_TOTAL_PRICE = data.ORIGINAL_PRICE * (1 + data.VAT_RATIO) * data.AMOUNT;
            }
        }
    }
}
