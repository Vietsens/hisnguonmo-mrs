using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00293
{
    public class Mrs00293RDO 
    {
        public string INSTRUCTION_DATE_STR { get; set; }
        public long INSTRUCTION_DATE { get; set; }
        public long TDL_INTRUCTION_TIME { get; set; }
        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string INSTRUCTION_TIME_STR { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }
        public long ID { get; set; }
        public int MALE_AGE { get; set; }
        public int FEMALE_AGE { get; set; }
        public long DOB { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public long PATIENT_ID { get; set; }
        public string PATIENT_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public long GENDER_ID { get; set; }
        
        public string HEIN_CARD_NUMBER { get; set; }
        public long? HEIN_CARD_FROM_TIME { get; set; }
        public long? HEIN_CARD_TO_TIME { get; set; }
        public string HEIN_MEDI_ORG_CODE { get; set; }
        public string HEIN_MEDI_ORG_NAME { get; set; }
        public string HEIN_ADDRESS { get; set; }

        public long? TreatmentId { get; set; }
        public decimal? PRICE { get; set; }
        public decimal? HEIN_PRICE { get; set; }
        public decimal? TDL_VIR_TOTAL_PATIENT_PRICE { get; set; }
        public decimal? TDL_VIR_TOTAL_HEIN_PRICE { get; set; }
        public decimal AMOUNT { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public long TRANSACTION_DATE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_TEXT { get; set; }

        public decimal AMOUNT_DEPOSIT { get; set; }

        public Mrs00293RDO() { }

        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }

        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }

        public string TDL_REQUEST_LOGINNAME { get; set; }
        public string TDL_REQUEST_USERNAME { get; set; }

        public string REQUEST_LOGINNAME { get; set; }

        public string REQUEST_USERNAME { get; set; }

        public string PARENT_SERVICE_NAME { get; set; }

        public string PARENT_SERVICE_CODE { get; set; }
    }
}
