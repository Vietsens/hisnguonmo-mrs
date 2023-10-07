using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00368
{
    public class Mrs00368RDO
    {
        public long REQUEST_DEPARTMENT_ID { get;  set;  }// khoa yêu cầu
        public string REQUEST_DEPARTMENT_NAME { get;  set;  }

        public decimal GROUP_NHOM_TEST_KHAC_AMOUNT { get;  set;  }
        public decimal GROUP_ZUNG_AMOUNT { get;  set;  }
        public decimal GROUP_RAVEN_AMOUNT { get;  set;  }
        public decimal GROUP_BECK_AMOUNT { get;  set;  }
        public decimal GROUP_SIEU_AM_AMOUNT { get;  set;  }
        public decimal GROUP_SIEU_AM_DROPPLER_XN_AMOUNT { get;  set;  }
        public decimal GROUP_DIEN_TIM_AMOUNT { get;  set;  }
        public decimal GROUP_LUU_HUYET_NAO_AMOUNT { get;  set;  }
        public decimal GROUP_DIEN_NAO_AMOUNT { get;  set;  }

    }
}
