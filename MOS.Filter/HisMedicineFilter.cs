
namespace MOS.Filter
{
    public class HisMedicineFilter : FilterBase
    {
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? BID_ID { get; set; }
        
        public HisMedicineFilter()
            : base()
        {
        }
    }
}
