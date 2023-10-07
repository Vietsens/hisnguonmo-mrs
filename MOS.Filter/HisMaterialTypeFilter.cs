
namespace MOS.Filter
{
    public class HisMaterialTypeFilter : FilterBase
    {
        public bool? IS_LEAF { get; set; }
        public bool? IS_STOP_IMP { get; set; }

        //Neu IsTree = true thi moi thuc hien tim kiem theo "Node" (parent_id == node)
        public bool? IsTree { get; set; }
        public long? Node { get; set; }

        public long? PARENT_ID { get; set; }
        public long? MANUFACTURER_ID { get; set; }
        public bool? IS_STENT { get; set; }
        public long? MEMA_GROUP_ID { get; set; }

        public HisMaterialTypeFilter()
            : base()
        {
        }
    }
}
