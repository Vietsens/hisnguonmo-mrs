
using System.Collections.Generic;
namespace TYT.Filter
{
    public class TytUninfectFilter : FilterBase
    {
        public TytUninfectFilter()
            : base()
        {
        }

        public long? DIAGNOSIS_TIME_FROM { get; set; }
        public long? DIAGNOSIS_TIME_TO { get; set; }
        public string ICD_CODE { get; set; }
        public List<string> ICD_CODEs { get; set; }
    }
}
