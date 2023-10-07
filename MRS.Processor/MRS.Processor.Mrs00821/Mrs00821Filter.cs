using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00821
{
    public class Mrs00821Filter
    {
        public long TIME_TO { get; set; }
        public long TIME_FROM { get; set; }
        public long? EXCUTE_ROOM_ID { get; set; }
        public List<long> EXCUTE_ROOM_IDs { get; set; }
        public long? REQ_ROOM_ID { get; set; }
        public List<long> REQ_ROOM_IDs { get; set; }
        public long? EXCUTE_DEPARTMENT_ID { get; set; }
        public List<long> EXCUTE_DEPARTMENT_IDs { get; set; }
        public long? REQ_DEPARTMENT_ID { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
    }
}
