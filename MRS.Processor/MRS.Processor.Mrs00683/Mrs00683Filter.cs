using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00683
{
    public class Mrs00683Filter
    {
        public long? EXAM_TIME_FROM { get; set; }
        public long? EXAM_TIME_TO { get; set; }

        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }

        public bool IS_PROVINCE { get; set; }//true: tỉnh khác; false: cùng tỉnh

        public List<long> PATIENT_TYPE_IDs { get; set; }//Đối tượng bn
    }
}
