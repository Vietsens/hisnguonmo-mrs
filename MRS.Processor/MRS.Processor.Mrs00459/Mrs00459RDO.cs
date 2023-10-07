using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00459
{
    class Mrs00459RDO
    {
        public long GROUP_ID { get;  set;  }
        public string TREATMENT_TYPE_NAME { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  } 
        public string FEMALE { get;  set;  } 
        public string MALE { get;  set;  } 
        public string ADDRESS { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }
        public string END_DEPARTMENT_NAME { get;  set;  }
        public string IN_ICD { get;  set;  }
        public string IN_ICD_CODE { get; set; }
        public string OUT_ICD { get;  set;  }
        public string OUT_ICD_CODE { get; set; }
        public string IN_TIME { get;  set;  }
        public string OUT_TIME { get;  set;  }
        public string TREATMENT_RESULT_NAME { get;  set;  }
        public decimal? TREATMENT_DAY_COUNT { get; set; }
        public string HOSPITAL_TRAN_TIME { get; set; }
        public string PATIENT_NATIONAL_NAME { get; set; }
        public string PATIENT_CAREER_NAME { get; set; }
        public string PATIENT_ETHNIC_NAME { get; set; }


        public Mrs00459RDO() { }

        public long? OUT_TIME_NUM { get; set; }

        public long? CLINICAL_IN_TIME_NUM { get; set; }
    }
}
