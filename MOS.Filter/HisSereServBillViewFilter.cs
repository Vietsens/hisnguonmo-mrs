
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisSereServBillViewFilter : FilterBase
    {
        public long? SERE_SERV_ID { get; set; }
        public long? BILL_ID { get; set; }
        public bool? IS_NOT_CANCEL { get; set; }
        public List<long> SERE_SERV_IDs { get; set; }
        public List<long> BILL_IDs { get; set; }
        public List<long> TDL_TREATMENT_IDs { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }

        public HisSereServBillViewFilter()
            : base()
        {
        }
    }
}
