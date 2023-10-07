using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00524
{
    class Mrs00524Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public long? TAKE_MONTH { get; set; }
        public List<string> REQUEST_LOGINNAMEs { get; set; }


        public List<long> MEDI_STOCK_IDs { get; set; }
    }
}