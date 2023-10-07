using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00405
{
   
    public class Mrs00405Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public List<long> TREATMENT_TYPE_IDs { get;  set;  }
        public List<long> PATIENT_TYPE_ID_WITH_CARDs { get;  set;  }//Doi tuong benh nhan theo the
        public List<long> PATIENT_TYPE_ID_WITH_SERVICEs { get; set; }//Doi tuong thanh toan
        public bool? IS_ALL { get; set; }
        public bool? HAS_ZERO_TOTAL_PRICE { get; set; }//có lấy tổng chi phí = 0 hay không
        public long? BRANCH_ID { get; set; }
        public Mrs00405Filter()
            : base()
        {

        }
    }
}
