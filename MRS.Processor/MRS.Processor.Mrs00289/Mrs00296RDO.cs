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
        public short? IS_CANCEL { get; set; }
        public long SERE_SERV_ID { get; set; }
        public long? TRANSACTION_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
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
        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string TDL_REQUEST_ROOM_CODE { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }

        public decimal PRICE { get; set; }
        public decimal AMOUNT_DEPOSIT_BILL { get; set; }
        public decimal TOTAL_DEPOSIT_BILL_PRICE { get; set; }
        public decimal AMOUNT_REPAY { get; set; }
        public decimal TOTAL_REPAY_PRICE { get; set; }
        //them thong tin benh nhan dong chi tra
        public decimal TOTAL_PATIENT_BHYT_PRICE { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        public long CASHIER_ROOM_ID { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public long? PAY_FORM_ID { get; set; }
        public string PAY_FORM_CODE { get; set; }
        public string PAY_FORM_NAME { get; set; }
        public Mrs00296RDO() { }

        public long? AREA_ID { get; set; }

        public string AREA_CODE { get; set; }

        public string AREA_NAME { get; set; }

        public long? SS_PATIENT_TYPE_ID { get; set; }

        public string SS_PATIENT_TYPE_CODE { get; set; }

        public string SS_PATIENT_TYPE_NAME { get; set; }
        public long NUM_ORDER { get; set; }

        public decimal CHENHLECH { get; set; }

        public string PRE_CASHIER_USERNAME { get; set; }

        public string DEPOSIT_BILL_NUM_ORDER { get; set; }
    }

}
