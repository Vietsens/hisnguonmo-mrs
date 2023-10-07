using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00649
{
    public class Mrs00649Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }

        public bool? IS_MEDICINE { get; set; }
        public bool? IS_MATERIAL { get; set; }
        public bool? IS_CHEMICAL_SUBSTANCE { get; set; }

        public List<long> MEDI_STOCK_IDs { get; set; }

        public List<long> IMP_MEST_TYPE_IDs { get; set; }
    }
}
