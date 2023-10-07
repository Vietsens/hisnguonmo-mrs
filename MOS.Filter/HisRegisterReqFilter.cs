
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisRegisterReqFilter : FilterBase
    {
        public List<long> REGISTER_GATE_IDs { get; set; }
        public long? REGISTER_GATE_ID { get; set; }
        public long? REGISTER_TIME_FROM { get; set; }
        public long? REGISTER_TIME_TO { get; set; }
        public long? PATIENT_ID { get; set; }
        public string CARD_CODE__EXACT { get; set; }
        public string SERVICE_CODE__EXACT
        { get; set; }

        public HisRegisterReqFilter()
            : base()
        {
        }
    }
}
