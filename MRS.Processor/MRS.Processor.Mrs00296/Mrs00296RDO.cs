using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisService;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00296
{
    public class Mrs00296RDO
    {
        public long? IS_CANCEL { get; set; }
        public long SERE_SERV_ID { get; set; }
        public long TRANSACTION_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public long TDL_EXECUTE_DEPARTMENT_ID { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_CODE { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_NAME { get; set; }
        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }
        public string TDL_REQUEST_DEPARTMENT_CODE { get; set; }
        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }
        public long TDL_EXECUTE_ROOM_ID { get; set; }
        public string TDL_EXECUTE_ROOM_CODE { get; set; }
        public string TDL_EXECUTE_ROOM_NAME { get; set; }
        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string TDL_REQUEST_ROOM_CODE { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }

        public decimal PRICE { get; set; }
        public decimal AMOUNT_DEPOSIT_BILL { get; set; }
        public decimal TOTAL_DEPOSIT_BILL_PRICE { get; set; }
        public decimal AMOUNT_REPAY { get; set; }
        public decimal TOTAL_REPAY_PRICE { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public long TRANSACTION_NUM_ORDER { get; set; }
        public string ACCOUNT_BOOK_CODE { get; set; }
        public string ACCOUNT_BOOK_NAME { get; set; }
        public long CASHIER_ROOM_ID { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }

        public Mrs00296RDO() { }

        public long? AREA_ID { get; set; }

        public string AREA_CODE { get; set; }

        public string AREA_NAME { get; set; }

        public string PARENT_SERVICE_CODE { get; set; }

        public string PARENT_SERVICE_NAME { get; set; }

        public string MEDIUM_SERVICE_CODE { get; set; }

        public string MEDIUM_SERVICE_NAME { get; set; }

        public long? PAY_FORM_ID { get; set; }

        public string PAY_FORM_CODE { get; set; }

        public string PAY_FORM_NAME { get; set; }

        public string CASHIER_LOGINNAME { get; set; }

        public string CASHIER_USERNAME { get; set; }

        public string CONSULTANT_LOGINNAME { get; set; }
        public string CONSULTANT_USERNAME { get; set; }

        public long EXDEPA_NUM_ORRDER { get; set; }
        public long TRANSACTION_DATE { get; set; }

        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public string INTRUCTION_TIME_STR { get; set; }

        public long? EXECUTE_TIME { get; set; }
        public string EXECUTE_TIME_STR { get; set; }

        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }

        public string PATIENT_TYPE_NAME { get; set; }

        public long? SS_PATIENT_TYPE_ID { get; set; }

        public long? PRIMARY_PATIENT_TYPE_ID { get; set; }

        public string SS_PATIENT_TYPE_CODE { get; set; }

        public string SS_PATIENT_TYPE_NAME { get; set; }

        public long? TDL_PATIENT_AGE { get; set; }

        public decimal? HEIN_RATIO { get; set; }

        public long TRANSACTION_TIME { get; set; }

        public string PATIENT_CLASSIFY_NAME { get; set; }

        public string PATIENT_CLASSIFY_CODE { get; set; }

        public long? TDL_PATIENT_CLASSIFY_ID { get; set; }
        public string STR_HOLIDAYS_NEWs { get; set; }
        public string STR_LUNAR_HOLIDAYS_NEWs { get; set; }

        public decimal? VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }




        public Dictionary<string, decimal> DIC_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PRICE_TUTRA { get; set; }
    }

}
