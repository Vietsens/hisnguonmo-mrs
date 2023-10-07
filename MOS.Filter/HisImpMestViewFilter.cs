using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisImpMestViewFilter : FilterBase
    {
        public List<long> IMP_MEST_TYPE_IDs { get; set; }
        public List<long> IMP_MEST_STT_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> MEDI_STOCK_PERIOD_IDs { get; set; }
        public List<long> REQ_ROOM_IDs { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public List<long> CHMS_EXP_MEST_IDs { get; set; }
        public List<long> AGGR_IMP_MEST_IDs { get; set; }
        public List<long> MOBA_EXP_MEST_IDs { get; set; }
        public List<long> TDL_TREATMENT_IDs { get; set; }
        //public List<long> BID_IDs { get; set; }
        public List<long> SUPPLIER_IDs { get; set; }

        public long? IMP_MEST_TYPE_ID { get; set; }
        public long? IMP_MEST_STT_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public long? REQ_ROOM_ID { get; set; }
        public long? REQ_DEPARTMENT_ID { get; set; }
        public long? CHMS_EXP_MEST_ID { get; set; }
        public long? AGGR_IMP_MEST_ID { get; set; }
        public long? MOBA_EXP_MEST_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        //public long? BID_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }

        public long? IMP_TIME_FROM { get; set; }
        public long? IMP_DATE_FROM { get; set; }
        public long? APPROVAL_TIME_FROM { get; set; }
        public long? CREATE_DATE_FROM { get; set; }
        public long? DOCUMENT_DATE_FROM { get; set; }

        public long? IMP_TIME_TO { get; set; }
        public long? IMP_DATE_TO { get; set; }
        public long? APPROVAL_TIME_TO { get; set; }
        public long? CREATE_DATE_TO { get; set; }
        public long? DOCUMENT_DATE_TO { get; set; }

        public string IMP_MEST_CODE__EXACT { get; set; }
        public string DOCUMENT_NUMBER__EXACT { get; set; }
        public bool? HAS_DOCUMENT_NUMBER { get; set; }
        public bool? HAS_AGGR { get; set; }
        public bool? HAS_MEDI_STOCK_PERIOD { get; set; }

        public string TDL_MOBA_EXP_MEST_CODE__EXACT { get; set; }
        public string TDL_CHMS_EXP_MEST_CODE__EXACT { get; set; }
        public string TDL_AGG_IMP_MEST_CODE__EXACT { get; set; }

        public HisImpMestViewFilter()
            : base()
        {
        }
    }
}
