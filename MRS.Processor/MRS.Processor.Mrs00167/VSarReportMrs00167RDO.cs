using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00167
{
    public class VSarReportMRS00167RDO
    {
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal? PRICE { get; set; }
        public decimal? AMOUNT { get; set; }
        public decimal? TOTAL_PRICE { get; set; }
    }

}
