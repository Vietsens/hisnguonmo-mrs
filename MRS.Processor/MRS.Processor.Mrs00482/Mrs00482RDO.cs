using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00482
{
    public class Mrs00482RDO
    {
        public V_HIS_TRANSACTION TRANSACTION { get; set; }
        public HIS_TREATMENT HIS_TREATMENT { get; set; }
        public string NOW_DEPARTMENT_NAME { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string PATIENT_ADDRESS { get; set; }
        public string DOB_YEAR { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public string DESCRIPTION { get;  set;  }
        public decimal DEPOSIT_CASH { get;  set;  }
        public decimal? DEPOSIT_TRANSFER { get;  set;  }
        public decimal? DEPOSIT_OTHER_PAY_FORM { get;  set;  }
        public decimal? REPAY { get; set; }
        public string REPAY_USERNAME { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public bool IS_CANCEL { get; set; }
        public short? CANCEL_VALUE { get; set; }

        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }

        public string CASHIER_ROOM_CODE { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }

        public long TRANSACTION_DATE { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string ACCOUNT_BOOK_NAME { get; set; }
        public long? NUM_ORDER { get; set; }

        public string CANCEL_LOGINNAME { get; set; }
        public string CANCEL_USERNAME { get; set; }
        public string PAY_FORM_CODE { get; set; }
        public string PAY_FORM_NAME { get; set; }
        public long? CANCEL_TIME { get; set; }

        public long? OUT_TIME { get; set; }

        public long? FEE_LOCK_TIME { get; set; }

        public string OUT_TIME_STR { get; set; }

        public string FEE_LOCK_TIME_STR { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public decimal AMOUNT { get; set; }//số tiền tạm ứng đã thu trong thời gian báo cáo
        public decimal TRANSFER_AMOUNT { get; set; }//số tiền tạm ứng đã chuyển khoản trong thời gian báo cáo


        public long ACCOUNT_BOOK_ID { get; set; }
        public long TDL_PATIENT_DOB { get; set; }

        public long PAY_FORM_ID { get; set; }

        public long ID { get; set; }

        public decimal AMOUNT_CANCEL { get; set; }//số tiền hủy tạm ứng trong thời gian báo cáo

        public decimal DEPOSIT_BLUNT { get; set; }

        public string TRANSACTION_CODE { get; set; }

        public string TRANSACTION_CODEs { get; set; }

        public long TRANSATION_TYPE_ID { get; set; }

        public string REPAY_STATUS { get; set; }

        public decimal TOTAL_PATIENT_PRICE { get; set; }

        public long IN_TIME { get; set; }

        public string IN_TIME_STR { get; set; }

        public string EINVOICE_NUM_ORDER { get; set; }

        public string BANK_TRANSACTION_CODE { get; set; }

        public long? BANK_TRANSACTION_TIME { get; set; }
    }
}
