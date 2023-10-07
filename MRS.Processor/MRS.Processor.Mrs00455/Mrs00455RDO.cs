using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00455
{
    public class Mrs00455RDO
    {
        public long TREATMENT_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long? TDL_HEIN_SERVICE_TYPE_ID { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string HEIN_SERVICE_TYPE_CODE { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public long EXECUTE_ROOM_ID { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set; }
        public string TREATMENT_CODE { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public long? FEE_LOCK_DATE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
        public string TREATMENT_RESULT_NAME { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long? CASHIER_ROOM_ID { get; set; }
        public string CASHIER_ROOM_CODE { get; set; }
        public string BIRTH_DAY { get; set; }
        public string GENDER_NAME { get; set; }
        public string ICD_10 { get;  set; }
        public long IN_TIME_NUM { get; set; }
        public string IN_TIME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public long? FINISH_TIME { get; set; }
        public long? START_TIME { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public long INTRUCTION_DATE { get; set; }
        public string DEPARTMENT_CODE { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }
        public string ROOM_CODE { get;  set;  }
        public string ROOM_NAME { get;  set; }
        public long? OUT_TIME_NUM { get; set; }
        public long? COUNT_TREATMENT { get; set; }
        public string OUT_TIME { get; set; }
        public decimal? VIR_TOTAL_PRICE { get; set; }
        public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }
        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }
        public decimal? VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal? TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public decimal? TOTAL_RICE_TEST { get; set; } //xet nghiem
        public decimal? TOTAL_RICE_XRAY { get;  set;  } 
        public decimal? TOTAL_RICE_MRI { get;  set;  }
        public decimal? TOTAL_RICE_CT { get;  set;  }
        public decimal? TOTAL_RICE_FUEX { get;  set;  } // tham do chuc nang
        public decimal? TOTAL_RICE_ENDO { get;  set;  } // noi soi
        public decimal? TOTAL_RICE_SUIM { get;  set;  } // sieu am
        public decimal? TOTAL_RICE_MEDI { get;  set;  } // thuoc
        public decimal? TOTAL_RICE_BLOOD { get;  set;  }// máu
        public decimal? TOTAL_RICE_SURG { get;  set;  } // PTTT
        public decimal? TOTAL_RICE_EXAM { get;  set;  } // khám
        public decimal? TOTAL_RICE_MATE { get;  set;  } // vat tu
        public decimal? TOTAL_RICE_BED { get;  set;  }  // giuong
        public decimal? TOTAL_RICE_OXY { get;  set;  }  // oxy 
        public decimal? TOTAL_RICE_TRSP { get;  set;  } // van chuyen
        public decimal? TOTAL_RICE_OTHER { get;  set;  } // van chuyen
        public decimal? TOTAL_PRICE_EXEM { get;  set;  }// mien giam
        public decimal? TOTAL_PRICE { get;  set;  }// tong tien
        public decimal? TOTAL_PRICE_TT { get;  set; }// tong tien thuc thu
        public Dictionary<string, int> DIC_SERVICE { get; set; }
        public Dictionary<string, decimal> DIC_CATE_TOTAL_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_CATE_TOTAL_HEIN_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_CATE_TOTAL_PATIENT_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_CATE_TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public Dictionary<string, decimal> DIC_SVT_TOTAL_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_SVT_TOTAL_HEIN_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_SVT_TOTAL_PATIENT_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_SVT_TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public Mrs00455RDO() { }
        public string CASHIER_USERNAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public long? BILL_NUM_ORDER { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public long DEPARTMENT_ID { get;  set; }

        public string PR_SERVICE_CODE { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }

        public string TEST_RESULT { get; set; }
        public string OTHER_RESULT { get; set; }

        public string INTRUCTION_TIME_STR { get; set; }

        public long SERE_SERV_ID { get; set; }
        public string MEDI_ORG_CODE { get; set; }
        public string MEDI_ORG_NAME { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string IN_CODE { get; set; }
        public string OUT_CODE { get; set; }
        public decimal? TREATMENT_DAY_COUNT { get; set; }
        //DHST
        public decimal? WEIGHT { get; set; }
        public decimal? TEMPERATURE { get; set; }
        public decimal? BREATH_RATE { get; set; }
        public decimal? HEIGHT { get; set; }
        public decimal? CHEST { get; set; }
        public decimal? BELLY { get; set; }
        public long? BLOOD_PRESSURE_MAX { get; set; }
        public long? BLOOD_PRESSURE_MIN { get; set; }
        public long? PULSE { get; set; }
        public decimal? SP02 { get; set; }

        public string PR_SERVICE_NAME { get; set; }

        public string TDL_SERVICE_TYPE_NAME { get; set; }

        public string TDL_SERVICE_TYPE_CODE { get; set; }

        public string PCR { get; set; }

        public string SERVICE_REQ_CODE { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }

        public string EXECUTE_DEPARTMENT_CODE { get; set; }

        public string EXECUTE_DEPARTMENT_NAME { get; set; }



        public Dictionary<string, decimal> DIC_CATE_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PATIENT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PATIENT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_DEXE_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_DEXE_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_DEXE_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Dictionary<string, decimal> DIC_DEXE_TOTAL_PATIENT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_DEXE_TOTAL_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_DEXE_TOTAL_PRICE { get; set; }

        public long? OTHER_PAY_SOURCE_ID { get; set; }

        public decimal? TOTAL_EXPEND_PRICE { get; set; }
    }
}
