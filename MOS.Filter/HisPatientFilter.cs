
namespace MOS.Filter
{
    public class HisPatientFilter : FilterBase
    {
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public long? GENDER_ID { get; set; }
        public long? DOB { get; set; }
        public long? CAREER_ID { get; set; }
        public long? MILITARY_RANK_ID { get; set; }
        public long? WORK_PLACE_ID { get; set; }
        public string PATIENT_CODE__EXACT { get; set; }

        public HisPatientFilter()
            : base()
        {

        }
    }
}
