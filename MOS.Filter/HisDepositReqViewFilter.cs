
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisDepositReqViewFilter : FilterBase
    {
        public long? TREATMENT_ID { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> BRANCH_IDs { get; set; }
        public bool? HAS_DEPOSIT { get; set; }
        public long? BRANCH_ID { get; set; }
        public string DEPOSIT_REQ_CODE__EXACT { get; set; }

        public HisDepositReqViewFilter()
            : base()
        {
        }
    }
}
