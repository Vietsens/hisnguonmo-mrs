using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00581
{
    public class Mrs00581Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        //public long? PAY_FORM_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public long? BRANCH_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public string LOGINNAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public List<string> LOGINNAMEs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public long? EXACT_CASHIER_ROOM_ID { get; set; }
        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }
        public bool? IS_EINVOICE_CREATED { get; set; }
        public List<long> FUND_IDs { get; set; }

        public List<long> ACCOUNT_BOOK_IDs { get; set; }
        public List<long> PAY_FORM_IDs { get; set; }

        public bool? IS_END_DEPARTMENT { set; get; }
        public short? IS_CANCEL { set; get; }

        public List<long> INPUT_DATA_ID_GUARANTEE_TYPEs { get; set; }//1:Chỉ lấy bệnh nhân có bảo lãnh,2:Chỉ lấy bệnh nhân không có bảo lãnh
        public string TREATMENT_CODE__EXACT { get; set; }

        public List<long> TDL_TREATMENT_TYPE_IDs { get; set; }
        public short? INPUT_DATA_ID_IS_PAUSE { get; set; } // 1: đã kết thúc điều trị; 2: Chưa kết thúc điều trị; 3: Tất cả
        public string KEY_PRICE { get; set; } // key để thay đổi cách chia các chi phí
    }
}
