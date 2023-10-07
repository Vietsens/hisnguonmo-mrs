using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00147
{
    class Mrs00147RDO
    {
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string MEDICINE_USE_FORM_CODE { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public string ACTIVE_INGR_BHYT_CODE { get;  set;  }
        public string ACTIVE_INGR_BHYT_NAME { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }
        public decimal? PRICE { get;  set;  }
        public decimal? AMOUNT_USED { get;  set;  }
        //thêm nguồn nhâp
        public string IMP_SOURCE_NAME { get; set; }
        public decimal? TOTAL_PRICE { get;  set; }
        public string ATC_CODES { get; set; }
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get; set; }//Biệt dược
        public string CONCENTRA { get; set; }//hàm lượng nồng độ
        public decimal? AMOUNT_TL { set; get; }// số lượng thanh lý
        public string TT_BIET_DUOC { get; set; }
        public Dictionary<long, decimal> DIC_EXP_TYPE_AMOUNT { get; set; }

        public string REQ_DEPARTMENT_NAME { get; set; }

        public long? TREATMENT_TYPE_ID { get; set; }

        public long EXP_MEST_TYPE_ID { get; set; }

        public decimal AMOUNT_EXP { get; set; }

        public decimal AMOUNT_IMP { get; set; }

        public decimal AMOUNT_EXP_NEW { get; set; }

        public decimal TOTAL_PRICE_NEW { get; set; }

        public decimal AMOUNT_KHAC_TNCC { get; set; }

        public string HEIN_SERVICE_BHYT_CODE { get; set; }

        public string HEIN_SERVICE_BHYT_NAME { get; set; }
    }
    public class PL5
    {
        public long AT_NUM_ORDER { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string ATC_CODES { get; set; }
        public string METY_NUM_ORDER { get; set; }
        public string HEIN_SERVICE_BHYT_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string CONCENTRA { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public decimal? AMOUNT_USED { get; set; }
        public decimal? PRICE { get; set; }
    }
}
