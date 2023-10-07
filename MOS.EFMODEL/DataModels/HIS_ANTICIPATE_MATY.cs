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
    
    public partial class HIS_ANTICIPATE_MATY
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
        public long ANTICIPATE_ID { get; set; }
        public long MATERIAL_TYPE_ID { get; set; }
        public Nullable<long> SUPPLIER_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public Nullable<decimal> IMP_PRICE { get; set; }
        public Nullable<long> BID_ID { get; set; }
    
        public virtual HIS_ANTICIPATE HIS_ANTICIPATE { get; set; }
        public virtual HIS_MATERIAL_TYPE HIS_MATERIAL_TYPE { get; set; }
        public virtual HIS_SUPPLIER HIS_SUPPLIER { get; set; }
        public virtual HIS_BID HIS_BID { get; set; }
    }
}
