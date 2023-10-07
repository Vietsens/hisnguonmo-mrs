using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00607
{
    /// <summary>
    /// Sổ vào viện theo khoa
    /// </summary>
    class Mrs00607Filter
    {
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? DATE { get; set; }
        public bool IS_CACULATION_PRICE { set; get; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public long? INPUT_DATA_ID_PATIENT_STT { get; set; } //1:chưa ra viện - 2: đã thanh toán - 3: đã ra viện nhưng chưa thanh toán
        public bool? IS_END { get; set; }
        

        public Mrs00607Filter()
            : base()
        {
        }
    }
}
