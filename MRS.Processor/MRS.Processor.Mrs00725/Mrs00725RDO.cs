using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00725
{
    class Mrs00725RDO
    {
		public string PATIENT_CODE { get; set; }

		public string PATIENT_NAME { get; set; }

		public int? MALE_AGE { get; set; }

		public int? FEMALE_AGE { get; set; }

		public string BED_NAME { get; set; }

		public string BED_ROOM_NAME { get; set; }

		public string EXECUTE_DEPARTMENT_NAME { get; set; }

		public string ACCESSION_NUMBER { get; set; }

        public string TREATMENT_CODE { get; set; }

        public long PATIENT_DOB { get; set; }

        public long PATIENT_GENDER_ID { get; set; }

        public string IN_TIME_STR { get; set; }

        public string OUT_TIME_STR { get; set; }

        public decimal TREATMENT_DAY_COUNT { get; set; }

        public string ICD_CODE { get; set; }

        public string ICD_SUB_CODE { get; set; }

        public string SERVICE_CODE { get; set; }

        public string SERVICE_NAME { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public string REGISTER_NUMBER { get; set; }

        public string CONCENTRA { get; set; }

        public string MEDICINE_USE_FORM_NAME { get; set; }

        public decimal AMOUNT { get; set; }

        public decimal PRICE { get; set; }
        public decimal PRICE_BY_PT { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_PRICE_NO_EXPEND { get; set; }
        public decimal DISCOUNT { get; set; }
        public short? IS_EXPEND { get; set; }
        public long EXPEND_TYPE_ID { get; set; }
        public decimal VAT { get; set; }

        public string TT30BYT_CODE { get; set; }

        public string INTRUCTION_TIME_STR { get; set; }

        public string EXECUTE_DEPARTMENT_CODE { get; set; }

        public string TUTORIAL { get; set; }

        public string EXECUTE_LOGINNAME { get; set; }

        public string EXECUTE_USERNAME { get; set; }

        public string PATIENT_DOB_STR { get; set; }

        public long? MEDICINE_USE_FORM_ID { get; set; }

        public long SERVICE_TYPE_ID { get; set; }

        public long? OUT_TIME { get; set; }

        public string ACTIVE_INGR_BHYT_NAME { get; set; }

        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public string REQUEST_DEPARTMENT_CODE { get; set; }

        public string REQUEST_LOGINNAME { get; set; }

        public string REQUEST_USERNAME { get; set; }

        public int XN_AMOUNT { get; set; }

        public int CDHA_AMOUNT { get; set; }

        public int ALL_SERVICE_AMOUNT { get; set; }

        public int EXAM_AMOUNT { get; set; }

        public decimal TOTAL_PRICE { get; set; }

        public string SERVICE_REQ_CODE { get; set; }

        public int TOTAL_PATIENT { get; set; }

        public int AVERAGE_XN { get; set; }

        public int AVERAGE_CDHA { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }

        public decimal TOTAL_HP_MEDI_PRICE { get; set; }

        public string EXECUTE_ROOM_CODE { get; set; }

        public string EXECUTE_ROOM_NAME { get; set; }

        public string REQUEST_ROOM_CODE { get; set; }

        public string REQUEST_ROOM_NAME { get; set; }
        public long PATIENT_TYPE_ID { get;  set; }
        public string PATIENT_TYPE_CODE { get;  set; }
        public string PATIENT_TYPE_NAME { get;  set; }
        public long? OTHER_PAY_SOURCE_ID { get;  set; }
        public string OTHER_PAY_SOURCE_CODE { get;  set; }
        public string OTHER_PAY_SOURCE_NAME { get;  set; }
        public string EXAM_ROOM_CODE { get; set; }
        public string EXAM_ROOM_NAME { get; set; }
        public string ICD_NAME { get; set; }
    }
}
