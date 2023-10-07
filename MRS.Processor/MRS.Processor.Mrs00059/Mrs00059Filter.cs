using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00059
{
    /// <summary>
    /// Bao cao tong hop doanh thu theo doi tuong thanh toan doi voi ho so da duyet khoa
    /// </summary>
    class Mrs00059Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set; }

        public List<long> TDL_TREATMENT_TYPE_IDs { get; set; }

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public List<string> LOGINNAMEs { get; set; }

        public int? INPUT_DATA_ID_STT_TYPE { get; set; }//1: chưa ra viện ; 2: đã ra viện

        public Mrs00059Filter()
            : base()
        {
        }

        public List<string> CASHIER_LOGINNAMEs { get; set; }

        public List<long> LAST_DEPARTMENT_IDs { get; set; }

        public bool? IS_ADD_VOLUNTARY { get; set; }
    }
}
