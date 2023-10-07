using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00801
{
    /// <summary>
    /// báo cáo số lượt khám của các phòng khám
    /// </summary>
    public class Mrs00801Filter
    {
        public Mrs00801Filter() { }
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
    }
}
