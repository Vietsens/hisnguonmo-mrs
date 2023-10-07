
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisPatientProgramFilter : FilterBase
    {
        public long? ID__NOT_EQUAL { get; set; }
        public long? PROGRAM_ID { get; set; }
        public long? PATIENT_ID { get; set; }

        public List<long> PROGRAM_IDs { get; set; }
        public List<long> PATIENT_IDs { get; set; }

        public HisPatientProgramFilter()
            : base()
        {
        }
    }
}
