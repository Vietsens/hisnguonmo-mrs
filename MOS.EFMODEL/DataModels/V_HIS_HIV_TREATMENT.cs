//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOS.EFMODEL.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_HIS_HIV_TREATMENT
    {
        public long ID { get; set; }
        public Nullable<long> CREATE_TIME { get; set; }
        public Nullable<long> MODIFY_TIME { get; set; }
        public string CREATOR { get; set; }
        public string MODIFIER { get; set; }
        public string APP_CREATOR { get; set; }
        public string APP_MODIFIER { get; set; }
        public Nullable<short> IS_ACTIVE { get; set; }
        public Nullable<short> IS_DELETE { get; set; }
        public string GROUP_CODE { get; set; }
        public long TREATMENT_ID { get; set; }
        public Nullable<short> HIV_PATIENT_TYPE { get; set; }
        public string HIV_PATIENT_STATUS { get; set; }
        public Nullable<short> HIV_TREATMENT_REASON { get; set; }
        public Nullable<long> HIV_INFECTION_DATE { get; set; }
        public Nullable<long> ARV_PATIENT_BEGIN { get; set; }
        public string BEGIN_REGIMEN_HIV_CODE { get; set; }
        public Nullable<short> BEGIN_REGIMEN_LEVEL { get; set; }
        public string REGIMEN_HIV_CODE { get; set; }
        public Nullable<short> REGIMEN_LEVEL { get; set; }
        public Nullable<short> TUBERCULOSIS_TREATMENT_TYPE { get; set; }
        public Nullable<short> TUBERCULOSIS_REGIMEN { get; set; }
        public Nullable<long> TUBERCULOSIS_TREATMENT_BEGIN { get; set; }
        public Nullable<long> TUBERCULOSIS_TREATMENT_END { get; set; }
        public Nullable<long> DATE_OF_PREGNANCY { get; set; }
        public Nullable<long> CTX_TREATMENT_BEGIN { get; set; }
        public Nullable<short> TEST_PCR_TIMES { get; set; }
        public Nullable<long> TEST_PCR_DATE { get; set; }
        public Nullable<long> TEST_PCR_RESULT_DATE { get; set; }
        public Nullable<short> TEST_PCR_RESULT { get; set; }
        public Nullable<short> TEST_PCR_RNA_REASON { get; set; }
        public Nullable<long> TEST_PCR_RNA_DATE { get; set; }
        public Nullable<short> TEST_PCR_RNA_RESULT { get; set; }
        public Nullable<long> TEST_PCR_RNA_RESULT_DATE { get; set; }
        public Nullable<short> HIV_TREATMENT_CODE { get; set; }
        public Nullable<long> ARV_TREATMEN_BEGIN { get; set; }
        public Nullable<long> ARV_TREATMEN_END { get; set; }
        public Nullable<short> PRESCRIPTION_ARV_DAY { get; set; }
        public long PATIENT_ID { get; set; }
        public long IN_TIME { get; set; }
    }
}
