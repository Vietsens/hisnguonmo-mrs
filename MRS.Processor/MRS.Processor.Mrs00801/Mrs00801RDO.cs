using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00801
{
    public class Mrs00801RDO
    {
        public string ROOM_NAME { get; set; }
        public string ROOM_CODE { get; set; }
        public int COUNT_EXAM  { get; set; }
        public int COUNT_EXAM_BHYT { get; set; }
        public int COUNT_EXAM_VP { get; set; }
        public int COUNT_FEMALE_EXAM { get; set; }
        public int DOB15 { get; set; }
        public int COUNT_MEDI_HOME_EXAM { get; set; }
        public int COUNT_TRANPATI_EXAM { get; set; }
        public int COUNT_TREATMENT_IN { get; set; }
        public Mrs00801RDO() { }
    }
}
