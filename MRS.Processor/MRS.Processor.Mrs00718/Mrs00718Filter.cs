using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00718
{
    class Mrs00718Filter
    {
        public long? TIME_TO { get; set; }
        public long? TIME_FROM { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public long INPUT_DATA_ID_TIME_TYPE { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> EXACT_BED_ROOM_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
    }
}
