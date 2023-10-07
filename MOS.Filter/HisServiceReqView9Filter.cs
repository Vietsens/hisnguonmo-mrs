
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisServiceReqView9Filter : FilterBase
    {
        public string SERVICE_REQ_CODE { get; set; }
        public string TREATMENT_CODE__EXACT { get; set; }
        public bool? IS_NO_EXECUTE { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? TDL_PATIENT_ID { get; set; }
        public long? SERVICE_REQ_TYPE_ID { get; set; }
        public List<long> SERVICE_REQ_STT_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public List<long> NULL_OR_NOT_IN_EXP_MEST_TYPE_IDs { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> SERVICE_REQ_TYPE_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> TRACKING_IDs { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public long? START_TIME_FROM { get; set; }
        public long? START_TIME_TO { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? TRACKING_ID { get; set; }
        public long? INTRUCTION_DATE_FROM { get; set; }
        public long? INTRUCTION_DATE_TO { get; set; }

        public HisServiceReqView9Filter()
            : base()
        {
        }
    }
}
