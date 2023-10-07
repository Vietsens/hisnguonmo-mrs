
namespace MOS.Filter
{
    public class HisCardFilter : FilterBase
    {
        public long? PATIENT_ID { get; set; }
        public string CARD_CODE__EXACT { get; set; }
        public string CARD_CODE { get; set; }
        public string CODE__EXACT { get; set; }

        public HisCardFilter()
            : base()
        {

        }
    }
}
