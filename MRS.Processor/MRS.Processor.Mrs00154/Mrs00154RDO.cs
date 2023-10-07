using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00154
{
    public class Mrs00154RDO
    {
        public long PARENT_ID { get; set; }
        public string PARENT_NAME { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string NATIONAL_NAME { get; set; }
        public decimal? PRICE { get; set; }
        public decimal? AMOUNT { get; set; }
        public decimal? TOTAL_PRICE { get; set; }

        public decimal? IMP_PRICE { get; set; }
        public decimal? IMP_VAT_RATIO { get; set; }
        public decimal? VAT_RATIO { get; set; }
    }


}
