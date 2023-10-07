using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00179
{

    public class Mrs00179Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long MEDI_STOCK_ID { get; set; }
        public List<long> REPORT_TYPE_CAT_IDs { get; set; }
        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }

        public Mrs00179Filter()
            : base()
        {

        }
    }

}
