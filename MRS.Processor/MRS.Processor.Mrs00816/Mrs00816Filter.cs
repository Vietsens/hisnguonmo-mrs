using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00816
{
    public class Mrs00816Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public bool IS_CASHER { get; set; }
        public long MEDI_STOCK_BUSINESS_ID { set; get; }
    }
}
