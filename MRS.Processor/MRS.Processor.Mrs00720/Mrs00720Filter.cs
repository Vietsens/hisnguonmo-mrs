using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00720
{
    class Mrs00720Filter
    {
        public bool? IS_SERVICE;
        public long? TIME_TO { get; set; }
        public long? TIME_FROM { get; set; }
        public bool? IS_CANCEL { get; set; }
        public List<long> PAY_FORM_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        
        public bool? SPLIT_BY_BILL { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        
        public long? DEPARTMENT_ID { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public List<long> ACCOUNT_BOOK_IDs { get; set; }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> ROOM_IDs { get; set; }
        public List<long>  SS_PATIENT_TYPE_IDs { get; set; }

        public bool? IS_GROUP_BY_PATIENT { get; set; }

        public bool? IS_GROUP_BY_REQ_DEPARTMENT { get; set; }

        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }
        public List<long> TDL_PATIENT_CLASSIFY_IDs { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }

        public long? END_DEPARTMENT_ID { get; set; }

        public long? INPUT_DATA_ID_CANCEL_TYPE { get; set; } //loại hủy: 1: hủy, 2: Không hủy, còn lại: tất cả
        public string EXACT_TREATMENT_CODE { get; set; }
        public bool? IS_SPLIT_INTO_ROOMS { get; set; }
        
    }
}
