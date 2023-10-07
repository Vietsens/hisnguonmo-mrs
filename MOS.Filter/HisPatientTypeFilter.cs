
namespace MOS.Filter
{
    public class HisPatientTypeFilter : FilterBase
    {
        public bool? IS_COPAYMENT { get; set; }
        public string PATIENT_TYPE_CODE__EXACT { get; set; }

        public HisPatientTypeFilter()
            : base()
        {

        }
    }
}
