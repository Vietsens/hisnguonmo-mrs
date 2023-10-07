using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00465
{
    public class Mrs00465RDO
    {
        public string PATIENT_NAME { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public long DOB_MALE { get;  set;  }
        public long DOB_FEMALE { get;  set;  }
        public long? EXECUTE_TIME { get;  set;  }

        public string REQUEST_USER { get;  set;  }
        public string EXECUTE_USER { get;  set;  }
        public string NURSING_USER { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        // THÊM
        public string TREATMENT_CODE { get;  set;  }
        public long AGE { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
        public string SV_PTTT_GROUP_NAME { get; set; }
        public decimal AMOUNT { get;  set;  }

        public string EXECUTE_ROLE_01 { get;  set;  }
        public string EXECUTE_ROLE_02 { get;  set;  }
        public string EXECUTE_ROLE_03 { get;  set;  }
        public string EXECUTE_ROLE_04 { get;  set;  }
        public string EXECUTE_ROLE_05 { get;  set;  }
        public string EXECUTE_ROLE_06 { get;  set;  }
        public string EXECUTE_ROLE_07 { get;  set;  }
        public string EXECUTE_ROLE_08 { get;  set;  }
        public string EXECUTE_ROLE_09 { get;  set;  }
        public string EXECUTE_ROLE_10 { get;  set;  }

        public string EXECUTE_ROLE_11 { get;  set;  }
        public string EXECUTE_ROLE_12 { get;  set;  }
        public string EXECUTE_ROLE_13 { get;  set;  }
        public string EXECUTE_ROLE_14 { get;  set;  }
        public string EXECUTE_ROLE_15 { get;  set;  }
        public string EXECUTE_ROLE_16 { get;  set;  }
        public string EXECUTE_ROLE_17 { get;  set;  }
        public string EXECUTE_ROLE_18 { get;  set;  }
        public string EXECUTE_ROLE_19 { get;  set;  }
        public string EXECUTE_ROLE_20 { get;  set;  }
    }

    public class EXECUTE_ROLE_465
    {
        public long NUMBER { get;  set;  }
        public long EXECUTE_ROLE_ID { get;  set;  }
        public string EXECUTE_ROLE_NAME { get;  set;  }
        public string EXECUTE_ROLE_TAG { get;  set;  }
    }
}
