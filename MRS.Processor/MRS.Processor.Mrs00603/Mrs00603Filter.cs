using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00603
{
    public class Mrs00603Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public bool? IS_TREAT_IN { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public bool? IS_END { get; set; }
        public long? INPUT_DATA_ID_PATIENT_STT { get; set; } //1:chưa ra viện - 2: đã thanh toán - 3: đã ra viện nhưng chưa thanh toán
    }
}
