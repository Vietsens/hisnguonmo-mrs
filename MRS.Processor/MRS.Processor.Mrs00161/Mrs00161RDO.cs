using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00161
{
    class Mrs00161RDO
    {
        public long TREATMENT_TYPE_ID { get;  set;  }
        public string TREATMENT_TYPE_CODE { get;  set;  }
        public string TREATMENT_TYPE_NAME { get;  set;  }

        public long TREATMENT_ID { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }

        public string PATIENT_NAME { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string BEFORE_MISU { get;  set;  }
        public string AFTER_MISU { get;  set;  }
        public string MISU_PPTT { get;  set;  }
        public string MISU_PPVC { get;  set;  }
        public string TIME_MISU_STR { get;  set;  }
        public string MISU_TYPE_NAME { get;  set;  }
        public string EXECUTE_DOCTOR { get;  set;  }
        public string ANESTHESIA_DOCTOR { get;  set;  }
        public string NOTE { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public string DESCRIPTION_SURGERY { get;  set;  }

        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }
    }
}
