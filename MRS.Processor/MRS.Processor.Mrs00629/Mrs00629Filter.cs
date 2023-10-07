using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00629
{
    public class Mrs00629Filter
    {
        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
    }
}