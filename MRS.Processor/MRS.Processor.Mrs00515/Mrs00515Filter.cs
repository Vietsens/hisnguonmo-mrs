using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00515
{
    /// <summary>
    /// Báo cáo chi tiết tiền thu tạm ứng của bệnh nhân
    /// </summary>
    class Mrs00515Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public Mrs00515Filter()
            : base()
        {
        }
    }
}
