using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00564
{
    public class Mrs00564Filter : HisServiceReqFilter
    {
        public string DEPARTMENT_CODEs { get; set; }
        public string ROOM_CODEs { get; set; }
    }
}
