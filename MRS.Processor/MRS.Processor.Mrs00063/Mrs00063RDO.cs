using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00063
{
    class Mrs00063RDO
    {
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string MALE_YEAR { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string IS_DUOI_12THANG { get; set; }
        public string IS_1DEN15TUOI { get; set; }
        public string IS_1DEN6TUOI { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string GIOITHIEU { get; set; }
        public string TRANSFER_IN_MEDI_ORG_NAME { get; set; }
        public string DATE_OUT_STR { get;  set;  }
        public string IS_DEAD_IN_24H { get;  set;  }
        public string DIAGNOSE { get;  set;  }
        public string IS_CURED { get;  set;  }
        public string IS_ABATEMENT { get;  set;  }
        public string IS_AGGRAVATION { get;  set;  }
        public string IS_UNCHANGED { get;  set;  }
        public string DATE_DEAD_STR { get;  set;  }
        public long? END_ORDER { get;  set;  }
        public decimal TOTAL_DATE_TREATMENT { get;  set;  }

        public string FEE_LOCK_DATE_STR { get;  set;  }

        public int? FEMALE_AGE { get; set; }
        public int? MALE_AGE { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }

        public string TREATMENT_RESULT_NAME { get; set; }
        public string STORE_CODE { get; set; }
        public string GENDER_NAME { get; set; }
        public string TRANSFER_IN_MEDI_ORG_CODE { get; set; }
        public string ADVISE { get; set; }
        public string APPOINTMENT_TIME_STR { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string END_ROOM_NAME { get; set; }
        public string DOCTOR_USERNAME { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string DOB_STR { get; set; }

        public string CCCD_DATE { get; set; }
        public string CCCD_NUMBER { get; set; }
        public string CCCD_PLACE { get; set; }
        public string CMND_DATE { get; set; }
        public string CMND_NUMBER { get; set; }
        public string CMND_PLACE { get; set; }
        public string CAREER_NAME { get; set; }
        public string EMAIL { get; set; }
        public string ETHNIC_NAME { get; set; }
        public string FATHER_NAME { get; set; }
        public string MILITARY_RANK_NAME { get; set; }
        public string MOTHER_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string MOBILE { get; set; }
        public string PHONE { get; set; }
        public string RELATIVE_ADDRESS { get; set; }
        public string RELATIVE_CMND_NUMBER { get; set; }
        public string RELATIVE_MOBILE { get; set; }
        public string RELATIVE_NAME { get; set; }
        public string RELATIVE_PHONE { get; set; }
        public string RELATIVE_TYPE { get; set; }
        public string RELIGION_NAME { get; set; }
        public string TREATMENT_END_TYPE_CODE { get;  set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public long IN_TIME { get;  set; }
        public long? OUT_TIME { get;  set; }
    }
}
