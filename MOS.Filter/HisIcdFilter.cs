
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisIcdFilter: FilterBase
    {
        public long? ICD_GROUP_ID { get; set; }
        public List<string> ICD_CODEs { get; set; }

        public HisIcdFilter()
            : base()
        {

        }
    }
}
