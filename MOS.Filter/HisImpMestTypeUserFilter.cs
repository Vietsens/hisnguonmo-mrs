
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisImpMestTypeUserFilter : FilterBase
    {
        public List<long> IMP_MEST_TYPE_IDs { get; set; }
        public List<string> LOGINNAMEs { get; set; }

        public long? IMP_MEST_TYPE_ID { get; set; }
        public string LOGINNAME { get; set; }

        public HisImpMestTypeUserFilter()
            : base()
        {
        }
    }
}
