using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00814
{
    public class Mrs00814Filter
    {
        public long TIME_FROM { get; set; }
        public long  TIME_TO { get; set; }
        public List<long> BED_TYPE_IDs { set; get; }
        public long BED_ROOM_IDs { set; get; }
    }
}
