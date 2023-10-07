
using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMaterialTypeViewFilter : FilterBase
    {
        //Neu IsTree = true thi moi thuc hien tim kiem theo "Node" (parent_id == node)
        public bool? IsTree { get; set; }
        public long? Node { get; set; }

        public bool? IS_LEAF { get; set; }
        public bool? IS_STOP_IMP { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public HisMaterialTypeViewFilter()
            : base()
        {
        }
    }
}
