using System.Collections.Generic;

namespace MOS.Filter
{
    /// <summary>
    /// Filter dac biet, ko extend tu filterBase
    /// </summary>
    public class HisMaterialStockViewFilter
    {
        public List<long> IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public short? IS_LEAF { get; set; }
        public long? ID { get; set; }
        public string ORDER_FIELD { get; set; }
        public string ORDER_DIRECTION { get; set; }
        public string KEY_WORD { get; set; }
        public bool INCLUDE_EMPTY { get; set; }

        public HisMaterialStockViewFilter()
        {
        }
    }
}
