
namespace MOS.Filter
{
    public class HisMedicineTypeTutFilter : FilterBase
    {
        public long? MEDICINE_TYPE_ID { get; set; }
        public string LOGINNAME__EXACT { get; set; }

        public HisMedicineTypeTutFilter()
            : base()
        {
        }
    }
}
