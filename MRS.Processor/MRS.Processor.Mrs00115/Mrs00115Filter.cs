using MOS.MANAGER.HisSereServ;
using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00115
{
    /// <summary>
    /// Báo cáo tiền công khám cho toàn viện
    /// </summary>
    class Mrs00115Filter
    {
        public List<long> LIST_SERVICE_TYPE_ID { get;  set;  }

        public string SERVICE_TYPE_CODE { get;  set;  }

        //public bool BILL { get;  set;  }

        //public bool NotBILL { get;  set;  }

        public long CREATE_TIME_FROM { get;  set;  }

        public long CREATE_TIME_TO { get;  set;  }

        public List<V_HIS_SERE_SERV> ListHisSereServ2 = new List<V_HIS_SERE_SERV>(); 

        public Mrs00115Filter() : base() { }
    }
}
