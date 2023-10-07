using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00389
{
    public class Mrs00389RDO
    {
        public long MEDI_STOCK_ID { get;  set;  }
        public long SERVICE_ID { get; set; }
        public string SERVICE_STOCK_NAME { get; set; }
        public string REQ_DEPARTMENT_NAME { get; set; }
        public string REQ_ROOM_NAME { get; set; }
        public string REQ_USERNAME { get; set; }
        public string SERVICE_TYPE_CODE { get;  set;  } 
        public string SERVICE_TYPE_NAME { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
        public decimal TOTAL_IMP_PRICE { get;  set;  }
        public string TYPE { get; set; }
        public string  CONCENTRA { get; set; }
    }
}
