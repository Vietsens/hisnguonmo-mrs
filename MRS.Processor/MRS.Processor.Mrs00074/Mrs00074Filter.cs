using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00074
{
    /// <summary>
    /// Báo cáo tình hình thanh toán viện phí bệnh nhân theo khoa
    /// </summary>
    class Mrs00074Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }

        public long? DEPARTMENT_ID { get; set; }

        public List<long> WORK_PLACE_IDs { get; set; }

        public List<long> PATIENT_TYPE_IDs { get; set; }

        public List<long> PATIENT_CLASSIFY_IDs { get; set; }

        public bool? IS_FEE_LOCK { get; set; }

        public Mrs00074Filter()
            : base()
        {
        }
    }
}
