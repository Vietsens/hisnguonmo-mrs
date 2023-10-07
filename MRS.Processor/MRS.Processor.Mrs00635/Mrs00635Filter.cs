using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00635
{
    public class Mrs00635Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
    }
}
