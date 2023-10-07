using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00608
{
    public class Mrs00608RDO
    {
        public string TREATMENT_CODE { get; set; }//Mã điều trị
        public string PATIENT_CODE { get; set; }// ma benh nhan
        public string ADDRESS { get; set; }//địa chỉ
        public string HEIN_CARD_NUMBER { get; set; }// Số thẻ
        public string PATIENT_NAME { get; set; }//Tên

        public string DEP_TRANSACTION_CODE { get; set; }//Các mã giao dịch tạm ứng
        public string BIL_TRANSACTION_CODE { get; set; }//Các mã giao dịch thanh toán
        public string REP_TRANSACTION_CODE { get; set; }//Các mã giao dịch hoàn ứng
        public string BILL_TIME_STR { get; set; }//Thời gian thanh toán
        public string FEE_LOCK_TIME_STR { get; set; }//Thời gian khóa vp

        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }//Tổng tiền tạm ứng
        public decimal TOTAL_REPAY_AMOUNT { get; set; }//Tổng tiền hoàn ứng
        public decimal TOTAL_PATIENT_PRICE { get; set; }//Tổng tiền cần trả
        public decimal TOTAL_BILL_OUTKC_AMOUNT { get; set; }//Thanh toán ngoài kết chuyển

        public string DOB_STR { get; set; }//ngày sinh
        public int? AGE { get;  set;  }//tuổi



        public decimal BILL_AMOUNT_VP { get; set; }

        public decimal BILL_AMOUNT_DV { get; set; }

        public decimal BILL_AMOUNT_AN { get; set; }

        public decimal DIFF { get; set; }

        public string IN_TIME_STR { get; set; }

        public string BILL_CASHIER_LOGINNAME { get; set; }

        public string BILL_CASHIER_USERNAME { get; set; }

        public string PAY_FORM_NAME { get; set; }

        public string PAY_FORM_SERVICE_NAME { get; set; }

        public string PAY_FORM_NORMAL_NAME { get; set; }

        public bool IS_SAME_DATE { get; set; }
    }
}
