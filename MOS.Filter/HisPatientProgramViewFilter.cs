
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisPatientProgramViewFilter : FilterBase
    {
        public long? PROGRAM_ID { get; set; }
        public long? PATIENT_ID { get; set; }

        public List<long> PROGRAM_IDs { get; set; }
        public List<long> PATIENT_IDs { get; set; }

        public HisPatientProgramViewFilter()
            : base()
        {
        }
    }
}
