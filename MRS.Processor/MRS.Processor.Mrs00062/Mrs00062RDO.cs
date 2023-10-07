using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00062
{
    class Mrs00062RDO
    {
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string IS_BHYT { get;  set;  }
        public string IS_DUOI_12THANG { get;  set;  }
        public string IS_1DEN15TUOI { get;  set;  }
        public string VIR_ADDRESS { get; set; }
        public string TRANSFER_IN_MEDI_ORG_NAME { get; set; }
        public string GIOITHIEU { get; set; }
        public string DATE_IN_STR { get;  set;  }
        public string DIAGNOSE_TUYENDUOI { get;  set;  }
        public string ICD_CODE_TUYENDUOI { get;  set;  }
        public string DIAGNOSE_KKB { get;  set;  }
        public string DIAGNOSE_KDT { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public short? IS_EXAM_IN_TREAT { get; set; }

        public string MALE_YEAR { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }

        public int? MALE_AGE { get;  set;  }
        public int? FEMALE_AGE { get;  set;  }
        public string DATE_CLINICAL_IN_STR { get; set; }
        public string IN_DEPARTMENT_CODE { get; set; }
        public string IN_DEPARTMENT_NAME { get; set; }
        public string IN_ROOM_CODE { get; set; }
        public string IN_ROOM_NAME { get; set; }


        public string TDL_PATIENT_NATIONAL_NAME { get; set; }

        public long TDL_PATIENT_DOB { get; set; }

        public string ICD_NAME { get; set; }

        public long? CLINICAL_IN_DEPARTMENT_ID { get; set; }
        public string CLINICAL_IN_DEPARTMENT_CODE { get; set; }
        public string CLINICAL_IN_DEPARTMENT_NAME { get; set; }
    }
    public class CATEGORY
    {
        public CATEGORY() { }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public decimal AMOUNT { get; set; }
    }
    public class EXAM_IN_TREAT
    {
        public EXAM_IN_TREAT() { }
        public long TREATMENT_ID { get; set; }
    }
}
