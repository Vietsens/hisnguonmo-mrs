
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisPrescriptionView1Filter : FilterBase
    {
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public List<long> TREATMENT_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> EXP_MEST_IDs { get; set; }
        public List<long> SERVICE_REQ_IDs { get; set; }
        public List<long> EXP_MEST_STT_IDs { get; set; }

        public long? EXP_MEST_STT_ID { get; set; }
        public long? EXP_TIME_FROM { get; set; }
        public long? EXP_TIME_TO { get; set; }
        public long? EXP_MEST_ID { get; set; }
        public long? SERVICE_REQ_ID { get; set; }
        public long? PATIENT_ID { get; set; }
        public long? TREATMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public bool? HAS_AGGR { get; set; }
        
        public long? MEDI_STOCK_ID { get; set; }
        public long? USE_TIME__TO { get; set; }
        public long? USE_TIME__FROM { get; set; }
        public long? INTRUCTION_TIME__TO { get; set; }
        public long? INTRUCTION_TIME__FROM { get; set; }

        public string EXP_MEST_CODE__EXACT { get; set; }
        public string PATIENT_CODE__EXACT { get; set; }
        public string TREATMENT_CODE__EXACT { get; set; }
        public string REQUEST_ROOM_CODE { get; set; }

        public HisPrescriptionView1Filter()
            : base()
        {
        }
    }
}
