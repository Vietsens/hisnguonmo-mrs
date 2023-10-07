
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMedicineView1Filter : FilterBase
    {
        public List<long> BID_IDs { get; set; }

        public string BID_NUMBER__EXACT { get; set; }
        public string MEDICINE_TYPE_CODE__EXACT { get; set; }
        public long? BID_ID { get; set; }
        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_TIME_TO { get; set; }
        public bool? IS_MISS_BHYT_INFO { get; set; }

        public HisMedicineView1Filter()
            : base()
        {
        }
    }
}
