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
    
    public partial class V_HIS_ANTIBIOTIC_REQUEST
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
        public string ANTIBIOTIC_REQUEST_CODE { get; set; }
        public long TREATMENT_ID { get; set; }
        public long DHST_ID { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public long REQUEST_TIME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public Nullable<long> APPROVE_TIME { get; set; }
        public long ANTIBIOTIC_REQUEST_STT { get; set; }
        public string APPROVAL_LOGINNAME { get; set; }
        public string APPROVAL_USERNAME { get; set; }
        public string ALLERGY { get; set; }
        public Nullable<short> IS_INFECTION_SHOCK { get; set; }
        public Nullable<short> IS_COMMUNITY_PNEUMONIA { get; set; }
        public Nullable<short> IS_HOSPITAL_PNEUMONIA { get; set; }
        public Nullable<short> IS_VENTILATOR_PNEUMONIA { get; set; }
        public Nullable<short> IS_MENINGITIS { get; set; }
        public Nullable<short> IS_URINARY_INFECTION { get; set; }
        public Nullable<short> IS_ABDOMINAL_INFECTION { get; set; }
        public Nullable<short> IS_SEPSIS { get; set; }
        public Nullable<short> IS_SKIN_INFECTION { get; set; }
        public Nullable<short> IS_OTHER_INFECTION { get; set; }
        public string INFECTION_ENTRY { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_TEXT { get; set; }
        public string CLINICAL_CONDITION { get; set; }
        public string WHITE_BLOOD_CELL { get; set; }
        public string CRP { get; set; }
        public string PCT { get; set; }
        public string SUBCLINICAL_RESULT { get; set; }
        public Nullable<decimal> CRCL { get; set; }
        public string NO_MICROBIOLOGY_REASON { get; set; }
        public Nullable<short> IS_LESS_RESPONSIVE_REGIMEN { get; set; }
        public Nullable<short> IS_DRUG_RESISTANCE { get; set; }
        public string ADR_ANTIBIOTIC_NAME { get; set; }
        public string OTHER_REASON { get; set; }
        public string OTHER_OPINION { get; set; }
        public Nullable<short> IS_CONTINUOUS_DIALYSIS { get; set; }
        public Nullable<short> IS_HD_DIALYSIS { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public Nullable<short> TDL_PATIENT_IS_HAS_NOT_DAY_DOB { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public Nullable<decimal> TEMPERATURE { get; set; }
        public Nullable<decimal> BREATH_RATE { get; set; }
        public Nullable<decimal> WEIGHT { get; set; }
        public Nullable<decimal> HEIGHT { get; set; }
        public Nullable<decimal> CHEST { get; set; }
        public Nullable<decimal> BELLY { get; set; }
        public Nullable<long> BLOOD_PRESSURE_MAX { get; set; }
        public Nullable<long> BLOOD_PRESSURE_MIN { get; set; }
        public Nullable<long> PULSE { get; set; }
        public Nullable<decimal> VIR_BMI { get; set; }
        public Nullable<decimal> VIR_BODY_SURFACE_AREA { get; set; }
        public Nullable<decimal> SPO2 { get; set; }
        public Nullable<decimal> CAPILLARY_BLOOD_GLUCOSE { get; set; }
        public string NOTE { get; set; }
        public Nullable<short> INFUTION_INTO { get; set; }
        public Nullable<short> INFUTION_OUT { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
    }
}