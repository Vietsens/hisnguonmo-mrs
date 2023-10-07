using MOS.EFMODEL.DataModels;
using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00717
{
    class Mrs00717Filter
    {
        public long? TIME_TO { get; set; }
        public long? TIME_FROM { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
    }
}
