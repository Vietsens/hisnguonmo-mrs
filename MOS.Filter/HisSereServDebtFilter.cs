
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisSereServDebtFilter : FilterBase
    {
        public long? DEBT_ID { get; set; }
        public long? SERE_SERV_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public long? TDL_SERVICE_ID { get; set; }

        public List<long> DEBT_IDs { get; set; }
        public List<long> SERE_SERV_IDs { get; set; }
        public List<long> TDL_TREATMENT_IDs { get; set; }
        public List<long> TDL_SERVICE_IDs { get; set; }

        public bool? IS_CANCEL { get; set; }

        public HisSereServDebtFilter()
            : base()
        {
        }
    }
}
