using MOS.EFMODEL.DataModels;
using System;

namespace MRS.Processor.Mrs00188
{
    class Mrs00188RDO
    {
        public string MEDICINE_TYPE_NAME { get; set; }
        public string UNIT { get; set; }
        public Decimal PRICE { get; set; }
        public Decimal AMOUNT { get; set; }
        public Decimal TT_PRICE { get; set; }
        public string EXP_TIME_STR { get; set; }
        public string EXP_MEST_CODE { get; set; }	
        public string DOCUMENT { get; set; }	
        public string REQ_DEPARTMENT_NAME { get; set; }	
        public string MEDICINE_GROUP_NAME { get; set; }	
        public string MEDICINE_TYPE_CODE { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public long? TDL_PATIENT_DOB { get; set; }

        public string TDL_PATIENT_ADDRESS { get; set; }

        public long? FINISH_TIME { get; set; }

        public string TYPE_NAME { get; set; }

        public long? MEDICINE_ID { get; set; }

        public string CONCENTRA { get; set; }

        public string REGISTER_NUMBER { get; set; }

        public decimal TH_AMOUNT { get; set; }

        public decimal TH_TT_PRICE { get; set; }

        public string REQ_DEPARTMENT_CODE { get; set; }
    }
}
