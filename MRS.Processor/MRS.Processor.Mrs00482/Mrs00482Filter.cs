using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00482
{
    public class Mrs00482Filter
    {
        public long TIME_FROM { get; set; }                 // thời gian thực hiện giao địch
        public long TIME_TO { get; set; }

        public long? BRANCH_ID { get; set; }                // chi nhánh   
        public string CASHIER_LOGINNAME { get; set; }       // thu ngân   
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public long? EXACT_CASHIER_ROOM_ID { get; set; }       // phòng thu ngân   
        public long? ACCOUNT_BOOK_ID { get; set; }       // Số thu chi
        public long? PAY_FORM_ID { get; set; }       // Hình thức thanh toán
        public bool? REMOVE_DUPLICATE { get; set; }
        public List<long> PATIENT_TYPE_IDs { set; get; }
        public List<long> DEPARTMENT_IDs { set; get; }
        public bool? ADD_INFO_HOPITAL_FEE { get; set; }
        public List<long> PAY_FORM_IDs { set; get; }
        public Mrs00482Filter() { }
        public bool? IS_DEPOSIT_NO_PAY { get; set; }// là tạm ứng và chưa thanh toán
        public bool? IS_ACTIVE { get; set; }// là chưa khóa viện phí
    }
}
