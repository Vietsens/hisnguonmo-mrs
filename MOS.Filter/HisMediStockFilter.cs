
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMediStockFilter : FilterBase
    {
        public long? ROOM_ID { get; set; }
        public long? PARENT_ID { get; set; }
        public bool? IS_ALLOW_IMP_SUPPLIER { get; set; }
        public bool? IS_BUSINESS { get; set; }

        //Neu IsTree = true thi moi thuc hien tim kiem theo "Node" (parent_id == node)
        public bool? IsTree { get; set; }
        public long? Node { get; set; }
        public List<long> ROOM_IDs { get; set; }

        public HisMediStockFilter()
            : base()
        {
        }
    }
}
