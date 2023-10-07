using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00126
{
    class Mrs00126RDO
    {
        public string TRANSACTION_CODE { get;  set;  }
        public string CREATE_TIME { get;  set;  }
        public string VIR_PATIENT_NAME { get;  set;  }
        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal TOTAL_PAY { get;  set;  }//Thanh toán
        public decimal TOTAL_PRICE_DEPOSIT { get;  set;  }//Tạm ứng
        public decimal TOTAL_PRICE_BILL { get;  set;  }//Viện phí
        public decimal TOTAL_PRICE_MEDICINE { get;  set;  }//tiền thuốc
        public decimal TOTAL_PRICE_BEDS { get;  set;  }//tiền giường
        public decimal TOTAL_PRICE_MATERIAL { get;  set;  }//y cụ
        public decimal TOTAL_PRICE_TEST_BLOOD { get;  set;  }//xét nghiệm máu
        public decimal TOTAL_PRICE_SERVICE_OTHER { get;  set;  }//Dịch vụ khác
        public decimal TOTAL_PRICE_BHYT_5 { get;  set;  }//BHYT 5%
        public decimal TOTAL_PRICE_BHYT_20 { get;  set;  }//BHYT 20%
        public decimal TOTAL_PRICE_BHYT_40 { get;  set;  }//BHYT 40%
        public decimal TOTAL_MONEY_EXCESS { get;  set;  }//Số tiền còn thừa
        public decimal TOTAL_MONEY_SHORTAGE { get;  set;  }//Số tiền còn thiếu

        public Mrs00126RDO() { }

        public string TREATMENT_CODE { get; set; }

        public string PATIENT_CODE { get; set; }

        public long NUM_ORDER { get; set; }

        public long TRANSACTION_TIME { get; set; }

        public decimal TOTAL_HEIN_PRICE { get; set; }

        public decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public decimal TOTAL_PATIENT_PRICE_DIFF { get; set; }

        public decimal TOTAL_PRICE { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_1 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_2 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_3 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_4 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_5 { get; set; }

        public decimal BILL_AMOUNT { get; set; }

        public decimal KC_AMOUNT { get; set; }

        public decimal TRANSFER_AMOUNT { get; set; }

        public decimal EXEMPTION { get; set; }

        public decimal TDL_BILL_FUND_AMOUNT { get; set; }

        public string PAY_FORM_CODE { get; set; }

        public string PAY_FORM_NAME { get; set; }
    }
}
