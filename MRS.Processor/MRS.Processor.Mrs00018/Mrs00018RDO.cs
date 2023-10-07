using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00018
{
    public class Mrs00018RDO
    {
        public string MEDICINE_HOATCHAT_NAME { get;  set;  }
        public string MEDICINE_CODE_DMBYT { get;  set;  }
        public string MEDICINE_STT_DMBYT { get;  set;  }
        public string MEDICINE_TYPE_NAME { get;  set;  }
        public string MEDICINE_DUONGDUNG_NAME { get;  set;  }
        public string MEDICINE_HAMLUONG_NAME { get;  set;  }
        public string MEDICINE_SODANGKY_NAME { get;  set;  }
        public string MEDICINE_UNIT_NAME { get;  set;  }
        public string BID_NUM_ORDER { get; set; }
        public string BID_NUMBER { get; set; }
        public decimal AMOUNT_NGOAITRU { get;  set;  }
        public decimal AMOUNT_NOITRU { get;  set;  }
        public decimal? PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }

        public long? SERVICE_ID { get; set; }
        public string ROUTE_CODE { get; set; }

        public string PROVINCE_CHECK { get; set; }

        public bool IS_OXY { get; set; }

        public bool IS_VCM { get; set; }

        public string HEIN_APPROVAL_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string END_CODE { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_MEDI_ORG_CODE { get; set; }
        public string INTRUCTION_DATE_STR { get; set; }
        public string START_TIME_STR { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public string AX_CODE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal AMOUNT_CC { get; set; }
        public decimal AMOUNT_TNT { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string ICD_NAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }
        public string REGISTER_NUMBER { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public string PROVINCE_TYPE { get; set; }// nội tỉnh/ ngoại tỉnh
        public string RIGHT_ROUTE_CODE { get; set; } // tuyến
    }
}
