
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisExpMestMetyReqViewFilter : FilterBase
    {
        public long? EXP_MEST_ID { get; set; }
        public long? MEDICINE_TYPE_ID { get; set; }
        public long? TDL_MEDI_STOCK_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }


        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public List<long> TDL_MEDI_STOCK_IDs { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }

        public HisExpMestMetyReqViewFilter()
            : base()
        {
        }
    }
}
