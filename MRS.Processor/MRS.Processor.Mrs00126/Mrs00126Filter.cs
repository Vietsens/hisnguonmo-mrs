using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00126
{
    /// <summary>
    /// Báo cáo Bảng kê thanh toán tiền viện phí BN ra viện
    /// </summary>
    class Mrs00126Filter
    {
        public long DATE_FROM { get;  set;  }
        public long DATE_TO { get;  set;  }

        public long? PATIENT_TYPE_ID { get; set; }
        public long? SERE_SERV_PATIENT_TYPE_ID { get; set; }

        public long? PAY_FORM_ID { get; set; }
        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }
    }
}
