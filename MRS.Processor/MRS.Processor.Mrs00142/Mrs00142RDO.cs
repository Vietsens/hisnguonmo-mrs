using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00142
{
    class Mrs00142RDO
    {
        public long HEIN_APPROVAL_ID { get;  set;  }
        public long TREATMENT_ID { get;  set;  }
        public long PATIENT_ID { get;  set;  }

        public string HEIN_APPROVAL_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string DOB_STR { get;  set;  }
        public short GENDER_CODE { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string MEDI_ORG_CODE { get;  set;  }
        public string HEIN_DATE_FROM_STR { get;  set;  }
        public string HEIN_DATE_TO_STR { get;  set;  }
        public string ICD_NAME { get;  set;  }
        public string ICD_CODE { get;  set;  }
        public string ICD_TEXT { get;  set;  }
        public short RIGHT_ROUTE_CODE { get;  set;  }
        public string MEDI_ORG_CODE_TRAN { get;  set;  }
        public short ACCIDENT_HURT_CODE { get;  set;  }
        public string DATE_IN_STR { get;  set;  }
        public string DATE_OUT_STR { get;  set;  }
        public long? TOTAL_DATE { get;  set;  }
        public short TREATMENT_RESULT_CODE { get;  set;  }
        public short TREATMENT_END_TYPE_CODE { get;  set;  }
        public string FEE_LOCK_DATE_STR { get;  set;  }
        public decimal RATIO { get;  set;  }
        public decimal MEDICINE_TOTAL_PRICE { get;  set;  }
        public decimal MATERIAL_TOTAL_PRICE { get;  set;  }
        public decimal VIR_TOTAL_PRICE { get;  set;  }
        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE { get;  set;  }
        public decimal VIR_TOTAL_OTHER_PRICE { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE_NDS { get;  set;  }
        public int YEAR_FINAL { get;  set;  }
        public int MONTH_FINAL { get;  set;  }
        public short TREATMENT_TYPE_CODE { get;  set;  }
        public string DEPARTMENT_BHYT_CODE { get;  set;  }
        public string CURRENT_MEDI_ORG_CODE { get;  set;  }
        public string LIVE_AREA_CODE { get;  set;  }
        public string SURGMISU_CODE { get;  set;  }
        public decimal WEIGHT { get;  set;  }
    }
}
