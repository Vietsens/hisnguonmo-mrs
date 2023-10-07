using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00532
{
    public class GROUP_PRICE
    {
        public Decimal COUNT_FEE_LOCK { get; set; }
        public Decimal SUM_TOTAL_PRICE { get; set; }
        public Decimal SUM_MEDI_PRICE { get; set; }
        public Decimal SUM_CLS_PRICE { get; set; }
        public Decimal INFO_AVG { get; set; }
        public Decimal INFO_PERCENT_MEDI { get; set; }
        public Decimal INFO_PERCENT_CLS { get; set; }

    }
}
