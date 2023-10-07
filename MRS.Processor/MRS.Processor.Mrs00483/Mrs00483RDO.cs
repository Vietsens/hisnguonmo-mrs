using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00483
{
    public class Mrs00483RDO
    {
        //public V_HIS_SERVICE_REQ SERVICE_REQ { get;  set;  }
        public V_HIS_SERE_SERV SERE_SERV { get; set; }
        public V_HIS_PATIENT HIS_PATIENT { get; set; }

        public string ICD_CODE { get; set; }

        public string ICD_NAME { get; set; }

        public long INTRUCTION_TIME { get; set; }
    }
}
