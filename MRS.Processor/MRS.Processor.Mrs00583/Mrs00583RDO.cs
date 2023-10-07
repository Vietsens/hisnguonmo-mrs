using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00583
{
    public class Mrs00583RDO
    {
        public long DATE_TIME { get; set; }
        public string DATE_TIME_STR { get; set; }
        public decimal COUNT_TREATMENT { get; set; }
        public decimal COUNT_SERE_SERV { get; set; }
        public decimal AMOUNT_TEETH_BH { get; set; }
        public decimal AMOUNT_TEETH_VP { get; set; }

        public decimal AMOUNT_VP_NT { get; set; }

        public decimal AMOUNT_VP_NGT { get; set; }

        public decimal AMOUNT_BH_NT { get; set; }

        public decimal AMOUNT_BH_NGT { get; set; }

    }
}
