using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00324
{
    public class Mrs00324RDO
    {
        public long REQUEST_DEPARTMENT_ID { get;  set;  }// khoa yêu cầu
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }

        public decimal GROUP1_AMOUNT { get;  set;  }
        public decimal GROUP2_AMOUNT { get;  set;  }
        public decimal GROUP3_AMOUNT { get;  set;  }
        public decimal GROUP4_AMOUNT { get;  set;  }
        public decimal GROUP5_AMOUNT { get;  set;  }
        public decimal GROUP6_AMOUNT { get;  set;  }
        public decimal GROUP7_AMOUNT { get;  set;  }

    }
}
