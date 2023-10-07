using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00650
{
    public class Mrs00650Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
    }
}
