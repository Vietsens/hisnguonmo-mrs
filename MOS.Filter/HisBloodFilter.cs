using System;
using System.Collections.Generic;
using System.Text;

namespace MOS.Filter
{
    public class HisBloodFilter : FilterBase
    {
        public List<long> BLOOD_TYPE_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> BLOOD_ABO_IDs { get; set; }
        public List<long> BLOOD_RH_IDs { get; set; }

        public long? BLOOD_TYPE_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? BLOOD_ABO_ID { get; set; }
        public long? BLOOD_RH_ID { get; set; }
        public long? BID_ID { get; set; }

        public HisBloodFilter()
            : base()
        {
        }
    }
}
