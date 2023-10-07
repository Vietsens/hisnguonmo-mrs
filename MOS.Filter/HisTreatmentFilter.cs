
using System.Collections.Generic;
namespace MOS.Filter
{
    public class HisTreatmentFilter : FilterBase
    {
        //public List<long> PROGRAM_IDs { get; set; }
        public List<long> OWE_TYPE_IDs { get; set; }
        public List<long> DEATH_CAUSE_IDs { get; set; }
        public List<long> DEATH_WITHIN_IDs { get; set; }
        public List<long> TRAN_PATI_FORM_IDs { get; set; }
        public List<long> EMERGENCY_WTIME_IDs { get; set; }
        public List<long> TRAN_PATI_REASON_IDs { get; set; }

        public List<string> ICD_CODEs { get; set; }
        public string ICD_CODE { get; set; }

        public long? TRAN_PATI_FORM_ID { get; set; }
        public long? TRAN_PATI_REASON_ID { get; set; }
        public long? DEATH_WITHIN_ID { get; set; }
        public long? DEATH_CAUSE_ID { get; set; }
        public long? PATIENT_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
        public long? IN_TIME_FROM { get; set; }
        public long? IN_TIME_TO { get; set; }
        public long? OWE_TYPE_ID { get; set; }
        //public long? PROGRAM_ID { get; set; }
        public long? EMERGENCY_WTIME_ID { get; set; }
        public bool? IS_OUT { get; set; }
        public string STORE_CODE__END_WITH { get; set; }
        public string STORE_CODE__START_WITH { get; set; }
        public string TREATMENT_CODE__EXACT { get; set; }
        public string APPOINTMENT_CODE__EXACT { get; set; }
        public string TDL_PATIENT_CODE__EXACT { get; set; }
        public long? IN_DATE_FROM { get; set; }
        public long? IN_DATE_TO { get; set; }
        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }

        public long? APPOINTMENT_TIME_FROM { get; set; }
        public long? APPOINTMENT_TIME_TO { get; set; }
        public long? CLINICAL_IN_TIME_FROM { get; set; }
        public long? CLINICAL_IN_TIME_TO { get; set; }
        public string TDL_HEIN_CARD_NUMBER__EXACT { get; set; }
        public bool? IS_CHRONIC { get; set; }
        public List<long> PATIENT_IDs { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public string PATIENT_CODE__EXACT { get; set; }
        public List<long> DATA_STORE_IDs { get; set; }
        public bool? HAS_DATA_STORE { get; set; }
        public bool? IS_PAUSE { get; set; }
        public bool? IS_LOCK_HEIN { get; set; }
        public long? DOB_FROM { get; set; }
        public long? DOB_TO { get; set; }
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
        public List<long> END_ROOM_IDs { get; set; }
        public long? KSK_CONTRACT_ID { get; set; }
        public List<long> KSK_CONTRACT_IDs { get; set; }

        public HisTreatmentFilter()
            : base()
        {
        }
    }
}
