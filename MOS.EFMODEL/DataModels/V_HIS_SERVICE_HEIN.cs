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
    
    public partial class V_HIS_SERVICE_HEIN
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
        public long SERVICE_ID { get; set; }
        public long BRANCH_ID { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public Nullable<decimal> RATIO { get; set; }
        public Nullable<long> PRIORITY { get; set; }
        public Nullable<long> AGE_FROM { get; set; }
        public Nullable<long> AGE_TO { get; set; }
        public string ICD_CODES { get; set; }
        public Nullable<long> FROM_TIME { get; set; }
        public Nullable<long> TO_TIME { get; set; }
        public Nullable<long> TREATMENT_FROM_TIME { get; set; }
        public Nullable<long> TREATMENT_TO_TIME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
    }
}
