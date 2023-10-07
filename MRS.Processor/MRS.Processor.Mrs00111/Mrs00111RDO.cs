using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00111
{
    class Mrs00111RDO
    {
        public string TREATMENT_CODE { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public string HEIN_CARD_NUMBER { get;  set;  }
        public string IS_DUOI_12THANG { get;  set;  }
        public string IS_1DEN15TUOI { get;  set;  }
        public string VIR_ADDRESS { get;  set;  }
        public string GIOITHIEU { get;  set;  }
        public string DATE_IN_STR { get;  set;  }
        public string DIAGNOSE_TUYENDUOI { get;  set;  }
        public string ICD_CODE_TUYENDUOI { get; set; }
        public string DIAGNOSE_KKB { get; set; }
        public string DIAGNOSE_KDT { get; set; }
        public string ICD_CODE_KKB { get; set; }
        public string ICD_CODE_KDT { get; set; }

        public string MALE_YEAR { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }

        public int? MALE_AGE { get;  set;  }
        public int? FEMALE_AGE { get;  set;  }

        public decimal? TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal? TOTAL_HEIN_PRICE { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }
        public string CASHIER_ROOM_CODE { get; set; }

        public long COUNT { get; set; }
    }
}
