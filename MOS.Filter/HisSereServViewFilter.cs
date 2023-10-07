using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisSereServViewFilter : FilterBase
    {
        public List<long> EXECUTE_ROOM_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public List<long> PARENT_IDs { get; set; }
        public List<long> PATIENT_IDs { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> BILL_IDs { get; set; }
        public List<long> MATERIAL_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> PACKAGE_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> EKIP_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<string> SERVICE_REQ_CODEs { get; set; }
        public List<decimal> HEIN_RATIOs { get; set; }

        public long? PACKAGE_ID { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? PATIENT_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? ID__NOT_EQUAL { get; set; }
        public long? PARENT_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_TIME_FROM { get; set; }
        public long? EXECUTE_TIME_TO { get; set; }
        public long? MATERIAL_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public long? INVOICE_ID { get; set; }

        public string HEIN_CARD_NUMBER__EXACT { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }

        public bool? IS_SPECIMEN { get; set; }
        public bool? IS_EXPEND { get; set; }
        public bool? HAS_EXECUTE { get; set; }
        public bool? HAS_INVOICE { get; set; }
        //review lai
        //public long? PRICE_POLICY_ID { get; set; }
        public decimal? HEIN_RATIO { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public long? EKIP_ID { get; set; }


        public long? INTRUCTION_DATE_FROM { get; set; }
        public long? INTRUCTION_DATE_TO { get; set; }
        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }

        public HisSereServViewFilter()
            : base()
        {

        }
    }
}
