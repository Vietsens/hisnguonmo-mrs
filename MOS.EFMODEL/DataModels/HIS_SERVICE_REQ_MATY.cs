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
    
    public partial class HIS_SERVICE_REQ_MATY
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
        public long SERVICE_REQ_ID { get; set; }
        public Nullable<long> MATERIAL_TYPE_ID { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public string UNIT_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public Nullable<long> NUM_ORDER { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public string TUTORIAL { get; set; }
        public Nullable<short> IS_SUB_PRES { get; set; }
        public long TDL_TREATMENT_ID { get; set; }
        public Nullable<decimal> PRES_AMOUNT { get; set; }
        public string EXCEED_LIMIT_IN_PRES_REASON { get; set; }
        public string EXCEED_LIMIT_IN_DAY_REASON { get; set; }
    
        public virtual HIS_MATERIAL_TYPE HIS_MATERIAL_TYPE { get; set; }
        public virtual HIS_SERVICE_REQ HIS_SERVICE_REQ { get; set; }
    }
}
