using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00389
{
    public class Mrs00389Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

        public List<long> REQ_ROOM_IDs { get; set; }
        public string KEY_GROUP_EXP { get; set; }
    }
}
