using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00272
{
    public class Mrs00272Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> ROOM_IDs { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }

        public bool? HAS_TRANSACTION { get; set; }

        public bool? IS_NO_DETAIL { get; set; }
    }
}
