
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisTreatmentView3Filter : FilterBase
    {
        public List<long> END_ROOM_IDs { get; set; }
        public long? IN_DATE_FROM { get; set; }
        public long? IN_DATE_TO { get; set; }
        public long? CAREER_ID { get; set; }
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
        public long? IN_TIME_FROM { get; set; }
        public long? IN_TIME_TO { get; set; }
        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public bool? IS_LOCK_HEIN { get; set; }
        public bool? IS_PAUSE { get; set; }
        public bool? IS_OUT { get; set; }
        public long? PATIENT_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public string TREATMENT_CODE__EXACT { get; set; }

        public List<string> ICD_CODEs { get; set; }
        public string ICD_CODE { get; set; }
        public long? KSK_CONTRACT_ID { get; set; }
        public List<long> KSK_CONTRACT_IDs { get; set; }

        public HisTreatmentView3Filter()
            : base()
        {
        }
    }
}
