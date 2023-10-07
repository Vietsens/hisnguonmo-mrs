using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00167
{
    public class Mrs00167Filter
    {
        public long DATE_FROM { get; set; }
        public long DATE_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> REQ_ROOM_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public short? IS_PATIENT_TYPE { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public long? SERVICE_ID { get; set; }
        public List<long> SERVICE_TYPE_IDs { set; get; }
        public bool? IS_EXECUTE_DEPARTMENT { get; set; }
    }
}
