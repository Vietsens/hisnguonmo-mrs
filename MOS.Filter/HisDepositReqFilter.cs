
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisDepositReqFilter : FilterBase
    {
        public long? TREATMENT_ID { get; set; }
        public List<long> TREATMENT_IDs { get; set; }

        public bool? HAS_DEPOSIT { get; set; }
        public string DEPOSIT_REQ_CODE__EXACT { get; set; }

        public HisDepositReqFilter()
            : base()
        {
        }
    }
}
