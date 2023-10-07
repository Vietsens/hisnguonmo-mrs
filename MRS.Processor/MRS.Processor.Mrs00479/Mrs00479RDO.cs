using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00479
{
    public class Mrs00479RDO
    {
        // báo cáo thống kê bn theo mặt bệnh
        public V_HIS_SERE_SERV SERE_SERV { get;  set;  }
        public V_HIS_SERE_SERV_PTTT SERE_SERV_PTTT { get;  set;  }
        public V_HIS_SERVICE_REQ SERVICE_REQ { get;  set;  }

        public string NEXT_DEPARTMENT_NAME { get;  set;  }
    }
}
