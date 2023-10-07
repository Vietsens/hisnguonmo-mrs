using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00198
{
    public class Mrs00198Filter
    {
        public long? BRANCH_ID { get; set; }
        public long DATE_FROM { get; set; }
        public long DATE_TO { get; set; }
        public short? IS_CAME { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
    }
}
