using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00079
{
    public class Mrs00079RDO
    {
        public string HEIN_SERVICE_TYPE_CODE { get; set; }
        public string HEIN_SERVICE_TYPE_NAME { get; set; }
        public string SERVICE_STT_DMBYT { get;  set;  }
        public string SERVICE_CODE_DMBYT { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }
        public decimal AMOUNT_NGOAITRU { get;  set;  }
        public decimal AMOUNT_NOITRU { get;  set;  }
        public decimal? TOTAL_HEIN_PRICE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public decimal HEIN_PRICE { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }

        public decimal AMOUNT { set; get; }
        public long? SERVICE_ID { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00079RDO()
        {

        }

        public MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL HEIN_APPROVAL { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public long DOB { get; set; }
        public string GENDER_CODE { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_MEDI_ORG_CODE { get; set; }
        public long HEIN_CARD_FROM_TIME_STR { get; set; }
        public long HEIN_CARD_TO_TIME_STR { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string ICD_CODE_EXTRA { get; set; }
        public string REASON_INPUT_CODE { get; set; }
        public string MEDI_ORG_FROM_CODE { get; set; }
        public string OPEN_TIME_SEPARATE_STR { get; set; }
        public string CLOSE_TIME_SEPARATE_STR { get; set; }
        public long? TOTAL_DAY { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
       
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public long INSURANCE_YEAR { get; set; }
        public long INSURANCE_MONTH { get; set; }
        public string HEIN_LIVE_AREA_CODE { get; set; }
        public string TREATMENT_TYPE_CODE { get; set; }
        public string CURRENT_MEDI_ORG_CODE { get; set; }
        public long PLACE_PAYMENT_CODE { get; set; }
        public long INSURANCE_STT { get; set; }
        public decimal REASON_OUT_PRICE { get; set; }
        public string REASON_OUT { get; set; }
        public decimal POLYLINE_PRICE { get; set; }
        public decimal EXCESS_PRICE { get; set; }
        public string ROUTE_CODE { get; set; }
        public string ICD_NAME_MAIN { set; get; }
        public string ICD_NAME_EXTRA { set; get; }
        public string TREATMENT_TYPE_NAME { set; get; }
        public string TRANSFER_IN_MEDI_ORG_CODE { set; get; }// mã nơi chuyển đến

        public Mrs00079RDO(MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval)
        {
            this.HEIN_APPROVAL = heinApproval;
            this.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
        }

        public void SetExtendField(Mrs00079RDO Data)
        {
        }

        public long IN_TIME { get; set; }

        public long? OUT_TIME { get; set; }

        public long? CLINICAL_IN_TIME { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public long TREATMENT_DAY_COUNT_6556 { get; set; }

        public decimal TT_PRICE { get; set; }

        public decimal VIR_TOTAL_HEIN_PRICE { get; set; }
    }
}
