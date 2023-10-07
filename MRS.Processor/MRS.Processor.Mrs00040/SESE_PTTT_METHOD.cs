using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00040
{
    public class SESE_PTTT_METHOD
    {
        public decimal? AMOUNT { get; set; }
        //public short? IS_ACTIVE { get; set; }
        //public short? IS_DELETE { get; set; }
        public long? PTTT_METHOD_ID { get; set; }
        public long? SERE_SERV_PTTT_ID { get; set; }
        public long? TDL_SERE_SERV_ID { get; set; }
        public long? TDL_SERVICE_REQ_ID { get; set; }
        public long? EKIP_ID { get; set; }

        public long? PTTT_GROUP_ID { get; set; }
    }
}
