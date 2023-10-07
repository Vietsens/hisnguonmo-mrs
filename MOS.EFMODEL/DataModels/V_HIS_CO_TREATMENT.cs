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
    
    public partial class V_HIS_CO_TREATMENT
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
        public long DEPARTMENT_TRAN_ID { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public Nullable<long> START_TIME { get; set; }
        public Nullable<long> FINISH_TIME { get; set; }
        public long TDL_TREATMENT_ID { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_TEXT { get; set; }
        public string COTREATMENT_REQUEST { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_FIRST_NAME { get; set; }
        public string TDL_PATIENT_LAST_NAME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public Nullable<long> TDL_HEIN_CARD_FROM_TIME { get; set; }
        public Nullable<long> TDL_HEIN_CARD_TO_TIME { get; set; }
        public string TREA_ICD_CODE { get; set; }
        public string TREA_ICD_NAME { get; set; }
        public string TREA_ICD_SUB_CODE { get; set; }
        public string TREA_ICD_TEXT { get; set; }
        public long CURRENT_DEPARTMENT_ID { get; set; }
        public string CURRENT_DEPARTMENT_CODE { get; set; }
        public string CURRENT_DEPARTMENT_NAME { get; set; }
    }
}