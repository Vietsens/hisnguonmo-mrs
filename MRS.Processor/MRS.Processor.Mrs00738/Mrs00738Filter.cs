using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00738
{
    class Mrs00738Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; } //lọc theo khoa chuyển
    }
}
