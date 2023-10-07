
namespace MOS.Filter
{
    public class HisCardViewFilter : FilterBase
    {
        public long? PATIENT_ID { get; set; }
        public string SERVICE_CODE__EXACT { get; set; }
        public string CODE__EXACT { get; set; }//query theo service_code hoac card_code
        public string CARD_CODE__EXACT { get; set; }

        public HisCardViewFilter()
            : base()
        {

        }
    }
}
