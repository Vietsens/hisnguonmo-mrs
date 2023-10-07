//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HTC.EFMODEL.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class V_HTC_PERIOD_DEPARTMENT
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
        public long PERIOD_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public Nullable<long> LABOR_AMOUNT { get; set; }
        public Nullable<long> CLINICAL_DAY_AMOUNT { get; set; }
        public Nullable<long> CLINICAL_AMOUNT { get; set; }
        public Nullable<long> FROM_EXAM_CLINICAL_AMOUNT { get; set; }
        public Nullable<long> END_TREATMENT_AMOUNT { get; set; }
        public Nullable<decimal> VIR_FROM_OTHER_CLINICAL_AMOUNT { get; set; }
        public Nullable<decimal> VIR_NOT_END_TREATMENT_AMOUNT { get; set; }
        public string PERIOD_CODE { get; set; }
        public Nullable<long> FROM_TIME { get; set; }
        public Nullable<long> TO_TIME { get; set; }
        public string PERIOD_NAME { get; set; }
    }
}
