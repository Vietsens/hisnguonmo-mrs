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
    
    public partial class V_HIS_ANTIBIOTIC_NEW_REG
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
        public string DOSAGE { get; set; }
        public string PERIOD { get; set; }
        public string USE_FORM { get; set; }
        public Nullable<decimal> USE_DAY { get; set; }
        public long ANTIBIOTIC_REQUEST_ID { get; set; }
        public long ACTIVE_INGREDIENT_ID { get; set; }
        public string CONCENTRA { get; set; }
        public string ACTIVE_INGREDIENT_CODE { get; set; }
        public string ACTIVE_INGREDIENT_NAME { get; set; }
        public Nullable<short> IS_APPROVAL_REQUIRED { get; set; }
    }
}
