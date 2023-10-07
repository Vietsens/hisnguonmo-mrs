
namespace MOS.Filter
{
    public class HisPatientTypeAllowFilter : FilterBase
    {
        public long? PATIENT_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID__OR__PATIENT_TYPE_ALLOW_ID { get; set; }

        public HisPatientTypeAllowFilter()
            : base()
        {
        }
    }
}
