using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00679
   
{
   
    public class Mrs00679RDO
    {
        public string SERVICE_REQ_CODE { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string ICD_TEXT { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string MEDI_ORG_CODE { get; set; }
        public string MEDI_ORG_NAME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }//địa chỉ
        public long ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_CODE { get; set; }
        public long? FEE_LOCK_TIME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public long? TDL_INTRUCTION_TIME { get; set; }
        public string TDL_REQUEST_USERNAME { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal VIR_PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string ROOM_NAME { get; set; }
        public string SERVICE_STATUS { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public decimal AMOUNT_EXPEND { get; set; }
        public decimal VIR_TOTAL_PRICE_EXPEND { get; set; }
        public decimal BN_CUNG_CHI_TRA { get; set; }
        public decimal BH_CHITRA { get; set; }
        public decimal NGUON_KHAC { get; set; }
        public string RQ_ROOM { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }



    }
    
}
