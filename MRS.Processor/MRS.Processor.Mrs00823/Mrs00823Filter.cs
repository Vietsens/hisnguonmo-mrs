using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00823
{
    public class Mrs00823Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<string> ICD_CODEs { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public string TREATMENT_CODE { get; set; }
    }
}
