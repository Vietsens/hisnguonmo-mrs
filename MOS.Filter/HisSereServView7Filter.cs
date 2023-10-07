using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisSereServView7Filter : FilterBase
    {
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> HEIN_APPROVAL_IDs { get; set; }
        public List<long> TDL_TREATMENT_IDs { get; set; }
        public long? HEIN_APPROVAL_ID { get; set; }
        public long? ID__NOT_EQUAL { get; set; }
        public long? PARENT_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public string HEIN_CARD_NUMBER__EXACT { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public bool? IS_SPECIMEN { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public long? INVOICE_ID { get; set; }
        public bool? HAS_INVOICE { get; set; }
        public bool? HAS_EXECUTE { get; set; }
        public bool? IS_EXPEND { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }

        public long? INTRUCTION_DATE_FROM { get; set; }
        public long? INTRUCTION_DATE_TO { get; set; }
        public long? INTRUCTION_TIME_FROM { get; set; }
        public long? INTRUCTION_TIME_TO { get; set; }

        public long? TDL_INTRUCTION_DATE__EQUAL { get; set; }

        public HisSereServView7Filter()
            : base()
        {

        }
    }
}
