using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00632
{
    class Mrs00632RDO
    {
        public V_HIS_TREATMENT V_HIS_TREATMENT { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string NUMBER_SAVE { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string IS_OFFICER { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string IS_CITY { get;  set;  }
        public string IS_COUNTRYSIDE { get;  set;  }
        public string IS_DUOI_12THANG { get;  set;  }
        public string IS_1DEN15TUOI { get;  set;  }
        public string JOB { get;  set;  }
        public string GIOITHIEU { get;  set;  }
        public string DATE_IN_STR { get;  set;  }
        public string DATE_TRIP_STR { get;  set;  }
        public string DATE_OUT_STR { get;  set;  }
        public string DIAGNOSE_TUYENDUOI { get;  set;  }
        public string ICD_CODE_TUYENDUOI { get;  set;  }
        public string DIAGNOSE_KKB { get;  set;  }
        public string DIAGNOSE_KDT { get;  set;  }
        public string DIAGNOSE_KGBP { get;  set;  }
        public string IS_CURED { get;  set;  }
        public string IS_ABATEMENT { get;  set;  }
        public string IS_AGGRAVATION { get;  set;  }
        public string IS_UNCHANGED { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }

        public decimal? MALE_AGE { get;  set;  }
        public decimal? FEMALE_AGE { get;  set;  }
        public string DALUUTRU { get; set; }
        public string CHUALUUTRU { get; set; }
        public bool HAS_STORE { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string CREATORDATASTORE { get; set; }
    }
}
