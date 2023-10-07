
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisMaterialTypeStockViewFilter
    {
        public long? ID { get; set; }
        public long? IS_ACTIVE { get; set; }
        public long? PARENT_ID { get; set; }
        //Bao cao ton kho, bat buoc phai co medi_stock_id
        public long? MEDI_STOCK_ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public bool? IS_LEAF { get; set; }
        public List<long> IDs { get; set; }
        public string ORDER_FIELD { get; set; }
        public string ORDER_DIRECTION { get; set; }
        public bool? IS_GOODS_RESTRICT { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public HisMaterialTypeStockViewFilter()
        {
        }
    }
}
