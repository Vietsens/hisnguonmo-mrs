using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00563
{
    public class Mrs00563Filter : HisTreatmentFilter
    {
        public List<string> SERVICE_CODE_KHAMYHCTs { get; set; }
        public List<string> ROOM_CODE_KHAMYHCTs { get; set; }

    }
}
