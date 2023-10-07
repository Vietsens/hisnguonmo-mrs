using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00335
{
    public class Mrs00335RDO
    {
        public long REQUEST_DEPARTMENT_ID { get;  set;  }// khoa yêu cầu
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }

        public decimal NSTAI_BHYT_AMOUNT { get;  set;  }// số lượng nội soi tai
        public decimal NSTAI_ND_AMOUNT { get;  set;  }
        public decimal NSMUI_BHYT_AMOUNT { get;  set;  }// số lượng nội soi mũi
        public decimal NSMUI_ND_AMOUNT { get;  set;  }
        public decimal NSHONG_BHYT_AMOUNT { get;  set;  }// số lượng nội soi họng
        public decimal NSHONG_ND_AMOUNT { get;  set;  }
        public decimal NSCTC_BHYT_AMOUNT { get;  set;  }// số lượng nội soi ctc
        public decimal NSCTC_ND_AMOUNT { get;  set;  }

    }
}
