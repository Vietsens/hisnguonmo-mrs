using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00165
{
    public class VSarReportMrs00165RDO
    {
        public long PARENT_ID { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal? PRICE { get; set; }
        public decimal? DISCOUNT_RATIO { get; set; }
        public decimal? AMOUNT { get; set; }
        public decimal? TOTAL_PRICE { get; set; }
        public string DOCUMENT_DATE { get; set; }
        public string DOCUMENT_NUMBER { get; set; }
    }
}
