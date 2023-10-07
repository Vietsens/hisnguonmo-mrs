using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00545
{
    public class Mrs00545Filter:HisTreatmentView4FilterQuery
    {
        
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public List<string> ADDRESS_EMPLOYEEs { get; set; }
    }
}
