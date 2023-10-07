using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00395
{
    public class Mrs00395RDO:V_HIS_TREATMENT
    {

        public string NATIONAL_NAME { get; set; }
        public long TOTAL_EXAM { get;  set;  }
        public long TOTAL_IN { get;  set;  }
        public long TREATMENT_TIME { get;  set;  }
        public string ICD_10 { get;  set;  }
       

    }
}
