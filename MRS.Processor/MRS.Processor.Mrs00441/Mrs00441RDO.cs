using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00441
{
    public class Mrs00441RDO
    {
        public V_HIS_SERVICE_REQ V_HIS_SERVICE_REQ { get; set; }
        public V_HIS_SERE_SERV V_HIS_SERE_SERV { get; set; }
        public V_HIS_SERE_SERV_PTTT V_HIS_SERE_SERV_PTTT { get; set; }
        public string PTTT_GROUP_NAME { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  } 
        public string SERVICE_NAME { get;  set;  }
        public long? INTRUCTION_TIME { get; set; }
        public string USERNAMEs { get; set; }
       
    }
}
