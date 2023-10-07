
using System.Collections.Generic;
namespace MOS.Filter
{
    /// <summary>
    /// Filter dac biet, ko extend tu filterBase
    /// </summary>
    public class HisMedicineTypeStockViewFilter
    {
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> IDs { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public bool? IS_LEAF { get; set; }
        public long? IS_ACTIVE { get; set; }
        public long? ID { get; set; }
        public bool? IS_GOODS_RESTRICT { get; set; }
        public string ORDER_DIRECTION { get; set; }
        public string ORDER_FIELD { get; set; }

        public HisMedicineTypeStockViewFilter()
        {
        }
    }
}
