
namespace MOS.Filter
{
    public class HisMediStockViewFilter : FilterBase
    {
        //Neu IsTree = true thi moi thuc hien tim kiem theo "Node" (parent_id == node)
        public bool? IsTree { get; set; }
        public long? Node { get; set; }

        public string MEDI_STOCK_CODE { get; set; }
        public bool? IS_ALLOW_IMP_SUPPLIER { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public bool? IS_BUSINESS { get; set; }

        public HisMediStockViewFilter()
            : base()
        {
        }
    }
}
