using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestView1Filter : FilterBase
    {
        public List<long> EXP_MEST_STT_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public List<long> REQ_ROOM_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
        public long? EXP_MEST_STT_ID { get; set; }

        public long? MEDI_STOCK_ID { get; set; }
        public long? REQ_DEPARTMENT_ID { get; set; }
        public long? REQ_ROOM_ID { get; set; }
        public long? AGGR_EXP_MEST_ID { get; set; }

        //Co thuoc phieu tong hop nao hay khong
        public bool? HAS_AGGR { get; set; }
        public string EXP_MEST_CODE__EXACT { get; set; }

        public long? FINISH_DATE_FROM { get; set; }
        public long? FINISH_DATE_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }

        public HisExpMestView1Filter()
            : base()
        {
        }
    }
}
