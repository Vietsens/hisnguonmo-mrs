
using System.Collections.Generic;
namespace MOS.Filter
{
    /// <summary>
    /// Filter dac biet, ko extend tu filterBase
    /// </summary>
    public class HisBloodTypeStockViewFilter
    {
        //Phuc vu lay du lieu ton kho => nguoi dung bat buoc phai chon kho
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public bool? IS_LEAF { get; set; }
        public short? IS_ACTIVE { get; set; }
        public long? ID { get; set; }
        public List<long> IDs { get; set; }
        public string ORDER_DIRECTION { get; set; }
        public string ORDER_FIELD { get; set; }

        public HisBloodTypeStockViewFilter()
        {
        }
    }
}
