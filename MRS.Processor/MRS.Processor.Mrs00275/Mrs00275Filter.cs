using MOS.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00275
{
    public class Mrs00275Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> MEDI_STOCK_BUSINESS_IDs { get; set; }
        public List<string> LOGINNAMEs { get; set; }
    }
}
