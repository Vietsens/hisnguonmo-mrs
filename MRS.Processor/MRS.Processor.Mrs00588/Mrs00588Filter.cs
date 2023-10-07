using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00588
{
    public class Mrs00588Filter
    {
        public long? IN_TIME_FROM { get; set; }
        public long? IN_TIME_TO { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
    }
}