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
    
    public partial class HIS_DEBATE_TEMP
    {
        public HIS_DEBATE_TEMP()
        {
            this.HIS_DEBATE_INVITE_USER = new HashSet<HIS_DEBATE_INVITE_USER>();
            this.HIS_DEBATE_USER = new HashSet<HIS_DEBATE_USER>();
        }
    
        public long ID { get; set; }
        public string DEBATE_TEMP_CODE { get; set; }
        public string DEBATE_TEMP_NAME { get; set; }
        public Nullable<long> CREATE_TIME { get; set; }
        public Nullable<long> MODIFY_TIME { get; set; }
        public string CREATOR { get; set; }
        public string MODIFIER { get; set; }
        public string APP_CREATOR { get; set; }
        public string APP_MODIFIER { get; set; }
        public Nullable<short> IS_ACTIVE { get; set; }
        public Nullable<short> IS_DELETE { get; set; }
        public string GROUP_CODE { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_TEXT { get; set; }
        public string TREATMENT_TRACKING { get; set; }
        public string TREATMENT_METHOD { get; set; }
        public string LOCATION { get; set; }
        public string REQUEST_CONTENT { get; set; }
        public string PATHOLOGICAL_HISTORY { get; set; }
        public string HOSPITALIZATION_STATE { get; set; }
        public string BEFORE_DIAGNOSTIC { get; set; }
        public string DIAGNOSTIC { get; set; }
        public string CARE_METHOD { get; set; }
        public string CONCLUSION { get; set; }
        public string DISCUSSION { get; set; }
        public string MEDICINE_TUTORIAL { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_CONCENTRA { get; set; }
        public Nullable<short> IS_PUBLIC { get; set; }
        public Nullable<long> DEPARTMENT_ID { get; set; }
    
        public virtual ICollection<HIS_DEBATE_INVITE_USER> HIS_DEBATE_INVITE_USER { get; set; }
        public virtual HIS_DEPARTMENT HIS_DEPARTMENT { get; set; }
        public virtual ICollection<HIS_DEBATE_USER> HIS_DEBATE_USER { get; set; }
    }
}
