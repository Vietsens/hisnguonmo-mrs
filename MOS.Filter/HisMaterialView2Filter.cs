
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMaterialView2Filter : FilterBase
    {
        public string MATERIAL_TYPE_CODE__EXACT { get; set; }
        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_TIME_TO { get; set; }

        public HisMaterialView2Filter()
            : base()
        {
        }
    }
}
