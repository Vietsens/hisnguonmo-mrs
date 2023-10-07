using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestMedicineFilter : FilterBase
    {
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> MEDICINE_IDs { get; set; }
        public List<long> TDL_MEDI_STOCK_IDs { get; set; }
        public List<long> TDL_MEDICINE_TYPE_IDs { get; set; }
        public List<long> TDL_AGGR_EXP_MEST_IDs { get; set; }

        public long? EXP_MEST_ID { get; set; }
        public long? TDL_AGGR_EXP_MEST_ID { get; set; }
        public long? TDL_AGGR_EXP_MEST_ID__OR__EXP_MEST_ID { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? TDL_MEDI_STOCK_ID { get; set; }
        public long? TDL_MEDICINE_TYPE_ID { get; set; }

        public long? APPROVAL_TIME_FROM { get; set; }
        public long? APPROVAL_DATE_FROM { get; set; }
        public long? EXP_TIME_FROM { get; set; }
        public long? EXP_DATE_FROM { get; set; }

        public long? APPROVAL_TIME_TO { get; set; }
        public long? APPROVAL_DATE_TO { get; set; }
        public long? EXP_TIME_TO { get; set; }
        public long? EXP_DATE_TO { get; set; }

        public bool? IS_EXPEND { get; set; }
        public bool? IS_EXPORT { get; set; }
        public bool? IS_APPROVED { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> TDL_SERVICE_REQ_IDs { get; set; }

        public HisExpMestMedicineFilter()
            : base()
        {
        }
    }
}
