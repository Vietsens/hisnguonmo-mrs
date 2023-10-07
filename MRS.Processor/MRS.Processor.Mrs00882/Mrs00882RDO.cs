using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00882
{
    class Mrs00882RDO
    {
        public long MONTH { get; set; }
        public string MONTH_STR { get; set; }
        public decimal BORN_ALIVE_TOTAL { get; set; }
        public decimal BORN_ALIVE_MALE { get; set; }
        public decimal BORN_ALIVE_FEMALE { get; set; }
        public decimal BORN_ALIVE_EENC { get; set; }
        public decimal BORN_ALIVE_PREMATURE { get; set; }
        public decimal BORN_ALIVE_NGAT { get; set; }
        public decimal BORN_WEIGHT_TOTAL { get; set; }
        public decimal BORN_WEIGHT_LESS_THAN_2500 { get; set; }
        public decimal BORN_WEIGHT_MORE_THAN_4000 { get; set; }
        public decimal BORN_K1 { get; set; }
        public decimal BORN_SANG_LOC { get; set; }
        public decimal BORN_MOTHER_WITH_HIV { get; set; }
        public decimal BORN_FROM_22WEEK_AND_MORE { get; set; }
    }
}
