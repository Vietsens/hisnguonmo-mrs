using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisPatientViewFilter : FilterBase
    {
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_NAME__EXACT { get; set; }
        public string PATIENT_CODE__EXACT { get; set; }
        public short? GENDER_ID { get; set; }
        public long? DOB { get; set; }
        public List<long> GENDER_IDs { get; set; }
        public long? DOB_FROM { get; set; }
        public long? DOB_TO { get; set; }

        public HisPatientViewFilter()
            : base()
        {

        }
    }
}
