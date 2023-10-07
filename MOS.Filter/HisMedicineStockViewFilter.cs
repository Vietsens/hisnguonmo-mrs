using System.Collections.Generic;

namespace MOS.Filter
{
    /// <summary>
    /// Filter dac biet, ko extend tu FilterBase, do co nhieu field cua filterBase ko duoc dung den
    /// </summary>
    public class HisMedicineStockViewFilter
    {
        public long? MEDI_STOCK_ID { get; set; }
        public long? ID { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> IDs { get; set; }
        public short? IS_LEAF { get; set; }
        public string ORDER_FIELD { get; set; }
        public string ORDER_DIRECTION { get; set; }
        public string KEY_WORD { get; set; }
        public bool INCLUDE_EMPTY { get; set; } //co bao gom cac loai thuoc ko co trong kho hay khong

        public HisMedicineStockViewFilter()
        {
        }
    }
}
