using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00436
{
    public class Mrs00436RDO
    {
        //KEY BÁO CÁO
        public string NUM_ORDER_STR { get;  set;  }//số biên lai
        public string CREATE_TIME_STR { get;  set;  }//ngày thanh toán
        public string REASON_STR { get;  set;  }//ngày thanh toán
        public string TREATMENT_CODE { get;  set;  }
        public decimal TOTAL_NOT_VAT_RATIO { get;  set;  }// ma benh nhan
        public decimal TOTAL_VAT_RATIO { get;  set;  }// ma benh nhan
        public string PATIENT_NAME { get;  set;  }
       
    }

    public class CASHIER
    {
        public string CASHIER_LOGINNAME { get; set; }//Tài khoản người thu, hủy
        public string CASHIER_USERNAME { get; set; }//Tên người thu, hủy
        public decimal AMOUNT_SERVICE { get; set; }//số lượng dịch vụ
        public decimal PRICE_SERVICE { get; set; }//Tổng tiền dịch vụ
        public decimal PRICE_EXAM { get; set; }//Tổng tiền khám
        public decimal PRICE_CANCEL_ON { get; set; }// Số tiền hủy trong ngày
        public decimal PRICE_CANCEL_BEFORE { get; set; }// Số tiền hủy của giao dịch trước đó

    }

    public class BILL_OF_CASHIER
    {
        public string CASHIER_LOGINNAME { get; set; }//Tài khoản người thu, hủy
        public string CASHIER_USERNAME { get; set; }//Tên người thu, hủy
        public decimal PRICE_CANCEL_ON { get; set; }// Số tiền hủy trong ngày
        public decimal PRICE_CANCEL_BEFORE { get; set; }// Số tiền hủy của giao dịch trước đó
        public string PRICE { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string CASHIER_ROOM_CODE { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string ACCOUNT_BOOK_CODE { get; set; }
        public long? NUM_ORDER { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
    }
}
