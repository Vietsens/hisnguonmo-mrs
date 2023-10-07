using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00506
{
    public class Mrs00506RDO
    {
        public long SERVICE_ID { get;  set;  }
        public long SERE_SERV_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get;  set;  }
        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public long HEIN_SERVICE_TYPE_NUM_ORDER { get; set; }

        public long? TDL_HEIN_SERVICE_TYPE_ID { get; set; }

        public long? PACKAGE_ID { get; set; }

        public long? PARENT_ID { get; set; }

        public short? IS_OUT_PARENT_FEE { get; set; }

        public decimal? VIR_PRICE { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }


        public decimal PRICE { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal? VIR_TOTAL_PRICE { get; set; }
    }
}
