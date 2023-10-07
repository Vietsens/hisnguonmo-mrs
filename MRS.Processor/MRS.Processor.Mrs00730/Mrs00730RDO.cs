using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00730
{
    class Mrs00730RDO
    {
        public string MEDICINE_CODE { get; set; }
        public string MEDICINE_NAME { get; set; }
        public string PARENT_MEDICINE_CODE { get; set; }
        public string PARENT_MEDICINE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string CONCENTRA { get; set; }
        public decimal? EXP_AMOUNT { get; set; }
        public decimal? PRICE { get; set; }

        public decimal? TOTAL_PRICE { get; set; }

        public decimal? PERCENT_PRICE { get; set; }

        public string NATIONAL_NAME { get; set; }

        public decimal? LUY_KE { get; set; }

        public string RANK { get; set; }

        public string IS_VIETNAM { get; set; }

        public string IS_FOREIGN { get; set; }

        public string CATEGORY_CODE { get; set; }
    }
}
