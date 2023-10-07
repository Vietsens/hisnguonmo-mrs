
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisDataStoreFilter : FilterBase
    {
        //Neu isTree = true thi moi thuc hien tim kiem theo "node" (parent_id == node)
        public bool? IsTree { get; set; }
        public long? Node { get; set; }
        public long? PARENT_ID { get; set; }
        public long? ROOM_ID { get; set; }
        public List<long> ROOM_IDs { get; set; }

        public HisDataStoreFilter()
            : base()
        {
        }
    }
}
