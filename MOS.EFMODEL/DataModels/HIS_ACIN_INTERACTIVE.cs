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
    
    public partial class HIS_ACIN_INTERACTIVE
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
        public long ACTIVE_INGREDIENT_ID { get; set; }
        public long CONFLICT_ID { get; set; }
        public string DESCRIPTION { get; set; }
        public string INSTRUCTION { get; set; }
        public string CONSEQUENCE { get; set; }
        public string MECHANISM { get; set; }
        public Nullable<long> INTERACTIVE_GRADE_ID { get; set; }
    
        public virtual HIS_ACTIVE_INGREDIENT HIS_ACTIVE_INGREDIENT { get; set; }
        public virtual HIS_ACTIVE_INGREDIENT HIS_ACTIVE_INGREDIENT1 { get; set; }
        public virtual HIS_INTERACTIVE_GRADE HIS_INTERACTIVE_GRADE { get; set; }
    }
}
