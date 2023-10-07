
namespace MOS.Filter
{
    public class HisPatientAdvanceFilter
    {
        public long? GENDER_ID { get; set; }
        public long? DOB { get; set; }
        public string VIR_PATIENT_NAME__EXACT { get; set; }

        public string PATIENT_PROGRAM_CODE__EXACT { get; set; }
        public string APPOINTMENT_CODE__EXACT { get; set; }
        public string CARD_CODE__EXACT { get; set; }
        public string HEIN_CARD_NUMBER__EXACT { get; set; }
        public string PATIENT_CODE__EXACT { get; set; }
        public string TREATMENT_CODE__EXACT { get; set; }

        public HisPatientAdvanceFilter()
            : base()
        {

        }
    }
}
