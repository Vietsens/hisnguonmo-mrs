using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00437
{
    public class Mrs00437RDO
    {
        public decimal TOTAL_TREATMENT_EXAM { get;  set;  }               // tổng số hsđt khám (bao gồm bhyt và viện phí)
        public decimal TREATMENT_EXAM_HEIN { get;  set;  }
        public decimal TREATMENT_EXAM_HEIN_HN { get;  set;  }
        public decimal TREATMENT_EXAM_HEIN_CN { get;  set;  }
        public decimal TREATMENT_EXAM_FEE { get;  set;  }
        public decimal TREATMENT_TREAT_IN { get;  set;  }                 // điều trị nội trú
        public decimal TREATMENT_TREAT_IN_BHYT { get;  set;  }
        public decimal TREATMANT_TREAT_IN_BHYT_HN { get;  set;  }
        public decimal TREATMENT_TREAT_IN_BHYT_CN { get;  set;  }
        public decimal TREATMENT_TREAT_IN_FEE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }                        // tiền dịch vị + viện phí
        public decimal TOTAL_HEIN_PRICE { get;  set;  }                   // bhyt trả
        public decimal TOTAL_PATIENT_PRICE { get;  set;  }                // bn chi trả
        //==========================================================
        public decimal BHYT_CHITRA { get;  set;  }
        public decimal BN_CHITRA { get;  set;  }
        public decimal XQ_CANCHOP { get;  set;  }
        public decimal XQ_CANCHOP_BHYT_CHITRA { get;  set;  }
        public decimal CDHA_BHYT_CHITRA { get;  set;  }
        public decimal CDHA_CHIPHI { get;  set;  }
        //==========================================================
        public decimal TOTAL_XHH { get;  set;  }                          // tiền XHH
        public decimal TOTAL_EXAM { get;  set;  }                         // tiền công khám
        public decimal TOTAL_KSK { get;  set;  }                          // tiền ksk
        public decimal TOTAL_BED { get;  set;  }                          // tiền giường
        public decimal TOTAL_TEST { get;  set;  }                         // xét nghiệm
        public decimal TOTAL_DIIM_XQSANS { get;  set;  }                  // xquang, nội soi, siêu âm
        public decimal TOTAL_SURG_MISU { get;  set;  }                    // pttt
        public decimal TOTAL_OTHER { get;  set;  }                        // dịch vụ khác
        public decimal TOTAL_MEDICINE { get;  set;  }                     // thuốc
        public decimal TOTAL_BLOOD { get;  set;  }                        // máu
        public decimal TOTAL_MATERIAL { get;  set;  }                     // vật tư
        public decimal TOTAL_TRUYEN { get;  set;  }                       // dịch vụ truyền
        public decimal TOTAL_COPY { get;  set;  }                         // sao bệnh án
        public decimal TOTAL_EXEMPTION { get;  set;  }                    // miễn giảm
    }
    public class DATA_GET
    {
        public long ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public decimal? AMOUNT { get; set; }
        public decimal? VIR_TOTAL_PRICE { get; set; }
        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }
        public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }
        public long? TDL_EXECUTE_ROOM_ID { get; set; }
        public long? REPORT_TIME { get; set; }
        public decimal? EXEMPTION { get; set; }
    }
    public class DATA_REPORT
    {
        public string PARENT_CODE { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }

        public decimal AMOUNT_HASCARD { get; set; }
        public decimal TOTAL_PRICE_HASCARD { get; set; }
        public decimal TOTAL_HEIN_PRICE_HASCARD { get; set; }
        public decimal TOTAL_PATIENT_PRICE_HASCARD { get; set; }

        public decimal AMOUNT_NOCARD { get; set; }
        public decimal TOTAL_PRICE_NOCARD { get; set; }

    }
    public class SERVICE_PTTT_GROUP
    {
        public long SERVICE_ID { get; set; }
        public string PTTT_GROUP_CODE { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
    }

}
