
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMedicineTypeViewFilter : FilterBase
    {
        //Neu IsTree = true thi moi thuc hien tim kiem theo "Node" (parent_id == node)
        public bool? IsTree { get; set; }
        public long? Node { get; set; }

        public bool? IS_LEAF { get; set; }
        public bool? IS_STOP_IMP { get; set; }
        public long? SERVICE_ID { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public string NATIONAL_NAME__EXACT { get; set; }
        public bool? IS_MISS_BHYT_INFO { get; set; }

        public HisMedicineTypeViewFilter()
            : base()
        {
        }
    }
}
