
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMedicineView2Filter : FilterBase
    {
        public string MEDICINE_TYPE_CODE__EXACT { get; set; }
        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_TIME_TO { get; set; }

        public HisMedicineView2Filter()
            : base()
        {
        }
    }
}
