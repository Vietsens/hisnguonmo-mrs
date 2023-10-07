
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisTreatmentView1Filter : FilterBase
    {
        public List<long> END_ROOM_IDs { get; set; }
        public string KEY_WORD__VIR_PATIENT_NAME__PATIENT_CODE__TREATMENT_CODE__GENDER_NAME { get; set; }
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
        public long? DOB_FROM { get; set; }
        public long? DOB_TO { get; set; }
        //public long? GENDER_ID { get; set; }//review lai
        public long? IN_TIME_FROM { get; set; }
        public long? IN_TIME_TO { get; set; }
        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public bool? IS_LOCK_HEIN { get; set; }
        public bool? IS_PAUSE { get; set; }
        public bool? IS_OUT { get; set; }
        public long? PATIENT_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public bool? HAS_PATY_ALTER_BHYT { get; set; }
        public bool? HAS_HEIN_APPROVAL { get; set; }
        public bool? HAS_NO_XML_URL_HEIN_APPROVAL { get; set; }
        public string TREATMENT_CODE__EXACT { get; set; }
        public long? BRANCH_ID { get; set; }
        public string TDL_HEIN_CARD_NUMBER__EXACT { get; set; }
        public long? IN_DATE_FROM { get; set; }
        public long? IN_DATE_TO { get; set; }

        public List<string> TDL_HEIN_CARD_NUMBER_PREFIXs { get; set; }
        public List<string> TDL_HEIN_CARD_NUMBER_PREFIX__NOT_INs { get; set; }

        public List<string> ICD_CODEs { get; set; }
        public string ICD_CODE { get; set; }
        public long? KSK_CONTRACT_ID { get; set; }
        public List<long> KSK_CONTRACT_IDs { get; set; }

        public HisTreatmentView1Filter()
            : base()
        {
        }
    }
}
