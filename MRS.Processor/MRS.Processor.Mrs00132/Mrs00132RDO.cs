using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00132
{
    class Mrs00132RDO
    {
        public string PATIENT_NAME { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string BEFORE_SURG { get;  set;  }
        public string AFTER_SURG { get;  set;  }
        public string SURG_PPPT { get;  set;  }
        public string SURG_PPVC { get;  set;  }
        public string TIME_SURG_STR { get;  set;  }
        public string SURG_TYPE_NAME { get;  set;  }
        public string EXECUTE_DOCTOR { get;  set;  }
        public string ANESTHESIA_DOCTOR { get;  set;  }
        public string NOTE { get;  set;  }

        public string IS_BHYT { get;  set;  }
        public string IS_QUAN { get;  set;  }//quan
        public string IS_BHQD { get;  set;  }//bao hiem quan doi
        public string IS_CS { get;  set;  }//chinh sach
        public string IS_BHTQ { get;  set;  }//than nhan
        public string IS_BHQH { get;  set;  }//quan huu
        public string IS_TE { get;  set;  }//tre em duoi 6 tuoi
        public string IS_DV { get;  set;  }//dich vu
        public string IS_QT { get;  set;  }//quoc te
        public string IS_OTHER { get;  set;  }//doi tuong khac

        public string PATIENT_CODE { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public string DESCRIPTION_SURGERY { get;  set;  }

        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }
    }
}
