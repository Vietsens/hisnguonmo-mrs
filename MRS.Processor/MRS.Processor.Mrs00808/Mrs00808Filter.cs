using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00808
{
    public class Mrs00808Filter
    {
        public long TIME_FROM { set; get; }
        public long TIME_TO { set; get; }
        public List<long> SERVICE_IDs { set; get; }
        public long? SERVICE_ID { set; get; }
        public bool? CHECK_TREATMENT_TYPE { set; get; }
        public List<long> EXECUTE_DEPARTMENT_IDs { set; get; }
        public List<long> EXECUTE_ROOM_IDs { set; get; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

        public List<long> REQUEST_ROOM_IDs { get; set; }
    }
}
