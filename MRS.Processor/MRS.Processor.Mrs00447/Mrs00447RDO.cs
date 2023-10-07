using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00447
{
    public class Mrs00447RDO
    {
        // báo cáo sổ điện tim, khí dung
        public long PATIENT_ID { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }

        public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }

        public string GENDER { get;  set;  }
        public long DOB { get;  set;  }
        public long DOB_MALE { get;  set;  }
        public long DOB_FEMALE { get;  set;  }

        public string ADDRESS { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }

        public string ICD_CODE { get;  set;  }
        public string ICD_NAME { get;  set;  }
        public string ICD_TEXT { get;  set;  }

        public long INTRUCTION_TIME { get;  set;  }
        public long? START_TIME { get;  set;  }
        public long? FINISH_TIME { get;  set;  }

        public string REQUEST_USERNAME { get;  set;  }
        public string REQUEST_ROOM_NAME { get;  set;  }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string MACHINE_NAME { get; set; }
        public string MACHINE_CODE { get; set; }
        public string EXECUTE_MACHINE_NAME { get; set; }
        public string EXECUTE_MACHINE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string DESCRIPTION { get; set; }

        public string CONCLUDE { get; set; }
    }
}
