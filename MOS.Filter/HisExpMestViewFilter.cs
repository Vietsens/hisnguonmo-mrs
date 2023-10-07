using System.Collections.Generic;

namespace MOS.Filter
{
    public class HisExpMestViewFilter : FilterBase
    {
        public List<long> BILL_IDs { get; set; }
        public List<long> PRESCRIPTION_IDs { get; set; }
        public List<long> MANU_IMP_MEST_IDs { get; set; }
        public List<long> SUPPLIER_IDs { get; set; }
        public List<long> AGGR_USE_TIMEs { get; set; }
        public List<long> AGGR_EXP_MEST_IDs { get; set; }
        public List<long> IMP_MEDI_STOCK_IDs { get; set; }
        public List<long> TDL_TREATMENT_IDs { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> EXP_MEST_REASON_IDs { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> EXP_MEST_STT_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public List<long> SALE_PATIENT_TYPE_IDs { get; set; }
        public List<long> TDL_PATIENT_IDs { get; set; }

        public long? BILL_ID { get; set; }
        public long? PRESCRIPTION_ID { get; set; }
        public long? MANU_IMP_MEST_ID { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public long? AGGR_USE_TIME { get; set; }
        public long? AGGR_EXP_MEST_ID { get; set; }
        public long? IMP_MEDI_STOCK_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? EXP_MEST_REASON_ID { get; set; }
        public long? REQ_DEPARTMENT_ID { get; set; }
        public long? MEDI_STOCK_ID { get; set; }
        public long? EXP_MEST_STT_ID { get; set; }
        public long? EXP_MEST_TYPE_ID { get; set; }
        public long? SALE_PATIENT_TYPE_ID { get; set; }
        public long? TDL_PATIENT_ID { get; set; }
        public long? IMP_OR_EXP_MEDI_STOCK_ID { get; set; }

        public long? CREATE_DATE_FROM { get; set; }
        public long? CREATE_DATE_TO { get; set; }
        public long? FINISH_DATE_FROM { get; set; }
        public long? FINISH_DATE_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }

        //Co thuoc phieu tong hop nao hay khong
        public bool? HAS_AGGR { get; set; }
        public bool? IS_NOT_TAKEN { get; set; }
        public bool? IS_CABINET { get; set; }
        public string EXP_MEST_CODE__EXACT { get; set; }
        public string TDL_PATIENT_CODE__EXACT { get; set; }

        public long? TDL_INTRUCTION_DATE_FROM { get; set; }
        public long? TDL_INTRUCTION_DATE_TO { get; set; }
        public long? REQ_ROOM_ID { get; set; }
        public List<long> REQ_ROOM_IDs { get; set; }

        public bool? HAS_XBTT_EXP_MEST_ID { get; set; }

        public HisExpMestViewFilter()
            : base()
        {
        }
    }
}
