
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisServiceReqFilter : FilterBase
    {
        public List<long> PARENT_IDs { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> TRACKING_IDs { get; set; }
        public List<long> SERVICE_REQ_STT_IDs { get; set; }
        public List<long> PAAN_POSITION_IDs { get; set; }
        public List<long> PAAN_LIQUID_IDs { get; set; }
        public List<long> REHA_SUM_IDs { get; set; }
        public List<long> DHST_IDs { get; set; }
        public List<long> SERVICE_REQ_TYPE_IDs { get; set; }
        public List<long> NOT_IN_SERVICE_REQ_TYPE_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }

        public string SERVICE_REQ_CODE__ENDS_WITH { get; set; }
        public string BARCODE__EXACT { get; set; }
        public string SERVICE_REQ_CODE__EXACT { get; set; }
        public string TREATMENT_CODE__EXACT { get; set; }
        public string KEYWORD__SERVICE_REQ_CODE__TREATMENT_CODE__PATIENT_NAME { get; set; }
        
        public long? REQUEST_DEPARTMENT_ID__OR__EXECUTE_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID__OR__EXECUTE_ROOM_ID { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? PARENT_ID { get; set; }
        public long? TDL_PATIENT_ID { get; set; }
        public long? SERVICE_REQ_TYPE_ID { get; set; }
        public long? SERVICE_REQ_STT_ID { get; set; }
        public long? EXECUTE_GROUP_ID { get; set; }
        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? TRACKING_ID { get; set; }
        public bool? HAS_EXECUTE { get; set; }
        public long? PAAN_POSITION_ID { get; set; }
        public long? PAAN_LIQUID_ID { get; set; }
        public long? REHA_SUM_ID { get; set; }
        public long? DHST_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public long? INTRUCTION_DATE_FROM { get; set; }
        public long? INTRUCTION_DATE_TO { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }

        public long? INTRUCTION_DATE__EQUAL { get; set; }
        public bool? IS_SEND_LIS { get; set; }
        public long? START_TIME_FROM { get; set; }
        public long? START_TIME_TO { get; set; }

        public string ORDER_FIELD1 { get; set; }
        public string ORDER_DIRECTION1 { get; set; }
        public string ORDER_FIELD2 { get; set; }
        public string ORDER_DIRECTION2 { get; set; }
        public string ORDER_FIELD3 { get; set; }
        public string ORDER_DIRECTION3 { get; set; }
        public string ORDER_FIELD4 { get; set; }
        public string ORDER_DIRECTION4 { get; set; }

        public HisServiceReqFilter()
            : base()
        {
        }
    }
}
