using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 

namespace MRS.Processor.Mrs00340
{
    public class Mrs00340RDO
    {
        public long NUMBER { get;  set;  }
        public string LOGINNAME { get;  set;  }
        public string USERNAME { get;  set;  }
        public string EXECUTE_ROLE_NAME { get;  set;  }
        public string DAY_01 { get;  set;  }
        public string DAY_02 { get;  set;  }
        public string DAY_03 { get;  set;  }
        public string DAY_04 { get;  set;  }
        public string DAY_05 { get;  set;  }
        public string DAY_06 { get;  set;  }
        public string DAY_07 { get;  set;  }
        public string DAY_08 { get;  set;  }
        public string DAY_09 { get;  set;  }
        public string DAY_10 { get;  set;  }
        public string DAY_11 { get;  set;  }
        public string DAY_12 { get;  set;  }
        public string DAY_13 { get;  set;  }
        public string DAY_14 { get;  set;  }
        public string DAY_15 { get;  set;  }
        public string DAY_16 { get;  set;  }
        public string DAY_17 { get;  set;  }
        public string DAY_18 { get;  set;  }
        public string DAY_19 { get;  set;  }
        public string DAY_20 { get;  set;  }
        public string DAY_21 { get;  set;  }
        public string DAY_22 { get;  set;  }
        public string DAY_23 { get;  set;  }
        public string DAY_24 { get;  set;  }
        public string DAY_25 { get;  set;  }
        public string DAY_26 { get;  set;  }
        public string DAY_27 { get;  set;  }
        public string DAY_28 { get;  set;  }
        public string DAY_29 { get;  set;  }
        public string DAY_30 { get;  set;  }
        public string DAY_31 { get;  set;  }
        public long TOTAL_GROUP_01 { get;  set;  }
        public long TOTAL_GROUP_02 { get;  set;  }
        public long TOTAL_GROUP_03 { get;  set;  }
        public long TOTAL_GROUP_04 { get;  set;  }

        public Mrs00340RDO()
        {

        }
    }

    public class DAY
    {
        public string DAY_01 { get;  set;  }
        public string DAY_02 { get;  set;  }
        public string DAY_03 { get;  set;  }
        public string DAY_04 { get;  set;  }
        public string DAY_05 { get;  set;  }
        public string DAY_06 { get;  set;  }
        public string DAY_07 { get;  set;  }
        public string DAY_08 { get;  set;  }
        public string DAY_09 { get;  set;  }
        public string DAY_10 { get;  set;  }
        public string DAY_11 { get;  set;  }
        public string DAY_12 { get;  set;  }
        public string DAY_13 { get;  set;  }
        public string DAY_14 { get;  set;  }
        public string DAY_15 { get;  set;  }
        public string DAY_16 { get;  set;  }
        public string DAY_17 { get;  set;  }
        public string DAY_18 { get;  set;  }
        public string DAY_19 { get;  set;  }
        public string DAY_20 { get;  set;  }
        public string DAY_21 { get;  set;  }
        public string DAY_22 { get;  set;  }
        public string DAY_23 { get;  set;  }
        public string DAY_24 { get;  set;  }
        public string DAY_25 { get;  set;  }
        public string DAY_26 { get;  set;  }
        public string DAY_27 { get;  set;  }
        public string DAY_28 { get;  set;  }
        public string DAY_29 { get;  set;  }
        public string DAY_30 { get;  set;  }
        public string DAY_31 { get;  set;  }
    }

    public class EXECUTE_LOGIN
    {
        public string LOGINNAME { get;  set;  }
        public string USERNAME { get;  set;  }
        public long EXECUTE_ROLE_ID { get;  set;  }
        public string EXECUTE_ROLE_NAME { get;  set;  }
        public string EXECUTE_TIME { get;  set;  }
        public long PTTT_GROUP_ID { get;  set;  }
    }

    public class TIME_LINE
    {
        public long ID { get;  set;  }
        public string DATE_VALUE { get;  set;  }
        public string DATE_STRING { get;  set;  }
    }
}

