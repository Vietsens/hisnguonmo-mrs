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
    
    public partial class HIS_MR_CHECKLIST
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
        public long MR_CHECK_ITEM_ID { get; set; }
        public Nullable<short> IS_SELF_CHECK { get; set; }
        public Nullable<short> IS_CHECKER_CHECK { get; set; }
        public string NOTE { get; set; }
        public long MR_CHECK_SUMMARY_ID { get; set; }
        public Nullable<short> IS_CHECKER_NOT_USED { get; set; }
    
        public virtual HIS_MR_CHECK_ITEM HIS_MR_CHECK_ITEM { get; set; }
        public virtual HIS_MR_CHECK_SUMMARY HIS_MR_CHECK_SUMMARY { get; set; }
    }
}
