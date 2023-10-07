using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00703
{
    public class Mrs00703Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long MEDI_STOCK_ID { get; set; }
        public List<long> MEDI_STOCK_CABINET_IDs { get; set; }
    }
}
