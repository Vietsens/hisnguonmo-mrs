
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisSeseDepoRepayFilter : FilterBase
    {
        public long? REPAY_ID { get; set; }
        public long? SERE_SERV_DEPOSIT_ID { get; set; }

        public List<long> REPAY_IDs { get; set; }
        public List<long> SERE_SERV_DEPOSIT_IDs { get; set; }

        public long? TDL_TREATMENT_ID { get; set; }
        public long? TDL_SERVICE_ID { get; set; }
        public List<long> TDL_TREATMENT_IDs { get; set; }
        public List<long> TDL_SERVICE_IDs { get; set; }

        public bool? IS_CANCEL { get; set; }

        public HisSeseDepoRepayFilter()
            : base()
        {
        }
    }
}
