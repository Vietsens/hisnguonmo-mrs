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
    
    public partial class V_HIS_SERVICE_FOLLOW
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
        public long FOLLOW_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public Nullable<short> IS_EXPEND { get; set; }
        public Nullable<decimal> CONDITIONED_AMOUNT { get; set; }
        public string TREATMENT_TYPE_IDS { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SPECIALITY_CODE { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string FOLLOW_CODE { get; set; }
        public string FOLLOW_NAME { get; set; }
        public long FOLLOW_TYPE_ID { get; set; }
        public string FOLLOW_TYPE_CODE { get; set; }
        public string FOLLOW_TYPE_NAME { get; set; }
    }
}
