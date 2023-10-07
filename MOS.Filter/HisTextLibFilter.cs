
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisTextLibFilter : FilterBase
    {
        public string HASHTAG { get; set; }
        public List<string> HASHTAGs { get; set; }
        public bool? CAN_VIEW { get; set; }

        public HisTextLibFilter()
            : base()
        {
        }
    }
}
