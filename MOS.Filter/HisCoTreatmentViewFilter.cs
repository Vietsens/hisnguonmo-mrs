
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisCoTreatmentViewFilter : FilterBase
    {
        public long? DEPARTMENT_ID { get; set; }
        public long? DEPARTMENT_TRAN_ID { get; set; }
        public long? CURRENT_DEPARTMENT_ID { get; set; }

        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> DEPARTMENT_TRAN_IDs { get; set; }
        public List<long> CURRENT_DEPARTMENT_IDs { get; set; }

        public long? START_TIME_FROM { get; set; }
        public long? START_TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }

        public bool? HAS_START_TIME { get; set; }
        public bool? HAS_FINISH_TIME { get; set; }


        public HisCoTreatmentViewFilter()
            : base()
        {
        }
    }
}
