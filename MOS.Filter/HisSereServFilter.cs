using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisSereServFilter : FilterBase
    {
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> PACKAGE_IDs { get; set; }
        public List<long> SERE_SERV_CALENDAR_IDs { get; set; }
        public List<long> EKIP_IDs { get; set; }
        public List<long> PARENT_IDs { get; set; }
        public List<long> HEIN_APPROVAL_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }

        public string HEIN_CARD_NUMBER__EXACT { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }

        public long? HEIN_APPROVAL_ID { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? PARENT_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? INVOICE_ID { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? PACKAGE_ID { get; set; }
        public long? EKIP_ID { get; set; }

        public bool? HAS_HEIN_APPROVAL { get; set; }
        public bool? HAS_INVOICE { get; set; }
        public bool? IS_EXPEND { get; set; }
        public bool? HAS_EXECUTE { get; set; }
        public bool? IS_SPECIMEN { get; set; }

        public long? TDL_INTRUCTION_TIME_FROM { get; set; }
        public long? TDL_INTRUCTION_TIME_TO { get; set; }
        public long? TDL_PATIENT_ID { get; set; }
        public long? TDL_SERVICE_TYPE_ID { get; set; }
        public List<long> TDL_SERVICE_TYPE_IDs { get; set; }
        public long? TREATMENT_ID__NOT_EQUAL { get; set; }

        public decimal? AMOUNT_FROM { get; set; }
        public decimal? AMOUNT_TO { get; set; }

        public long? TDL_REQUEST_DEPARTMENT_ID { get; set; }
        public List<long> TDL_REQUEST_DEPARTMENT_IDs { get; set; }
        public long? TDL_EXECUTE_DEPARTMENT_ID { get; set; }
        public List<long> TDL_EXECUTE_DEPARTMENT_IDs { get; set; }

        public long? EXECUTE_TIME_FROM { get; set; }
        public long? EXECUTE_TIME_TO { get; set; }

        public long? TDL_REQUEST_ROOM_ID { get; set; }
        public List<long> TDL_REQUEST_ROOM_IDs { get; set; }
        public long? TDL_EXECUTE_ROOM_ID { get; set; }
        public List<long> TDL_EXECUTE_ROOM_IDs { get; set; }

        public HisSereServFilter()
            : base()
        {

        }
    }
}
