using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00822
{
    public class Mrs00822Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; } //điều kiện lọc loại dịch vụ
        public string KEY_GROUP_SV { get; set; } // key group dịch vụ
    }
}
