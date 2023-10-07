
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisBloodViewFilter : FilterBase
    {
        public List<long> BLOOD_TYPE_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> BLOOD_ABO_IDs { get; set; }
        public List<long> BLOOD_RH_IDs { get; set; }
        public List<long> BLOOD_VOLUME_IDs { get; set; }

        public short? BLOOD_TYPE_IS_ACTIVE { get; set; }
        public long? BLOOD_TYPE_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? BLOOD_ABO_ID { get; set; }
        public long? BLOOD_RH_ID { get; set; }
        public long? BLOOD_VOLUME_ID { get; set; }

        public string BLOOD_CODE__EXACT { get; set; }

        public HisBloodViewFilter()
            : base()
        {
        }
    }
}
