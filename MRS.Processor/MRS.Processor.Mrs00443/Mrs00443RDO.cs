using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00443
{
    public class Mrs00443RDO
    {
        public string LOGINNAME { get;  set;  }
        public string USERNAME { get;  set;  }
        public string ROOMNAME { get;  set;  }
        public Decimal AMOUNT { get;  set;  }
        public Decimal? TOTAL_PRICE { get;  set;  }

        public Decimal AMOUNT_EXAM_SERVICE { get;  set;  }
        public Decimal AMOUNT_IN_TREAT { get;  set;  }

    }
}
