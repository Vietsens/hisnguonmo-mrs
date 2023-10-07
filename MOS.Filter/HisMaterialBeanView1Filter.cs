using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisMaterialBeanView1Filter : FilterBase
    {
        public long? MEDI_STOCK_ID {get;set;}
        public long? MATERIAL_TYPE_ID { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? MATERIAL_TYPE_IS_ACTIVE { get; set; }
        public long? MATERIAL_IS_ACTIVE { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> MATERIAL_TYPE_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> SOURCE_IDs { get; set; }

        /// <summary>
        /// Bean co dang o trong kho hay khong (medi_stock_id null or != null)
        /// </summary>
        public enum InStockEnum
        {
            YES,
            NO,
        }
        public InStockEnum? IN_STOCK { get; set; }

        public HisMaterialBeanView1Filter()
            : base()
        {
        }
    }
}
