
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisPatientTypeAlterViewFilter : FilterBase
    {
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? LOG_TIME_TO { get; set; }
        public long? TDL_PATIENT_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }

        public long? DEPARTMENT_TRAN_ID { get; set; }
        public List<long> DEPARTMENT_TRAN_IDs { get; set; }
        
        public HisPatientTypeAlterViewFilter()
            : base()
        {
        }
    }
}
