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
    
    public partial class V_HIS_BID_MATERIAL_TYPE
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
        public long BID_ID { get; set; }
        public Nullable<long> MATERIAL_TYPE_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public Nullable<decimal> IMP_PRICE { get; set; }
        public Nullable<decimal> IMP_VAT_RATIO { get; set; }
        public Nullable<decimal> INTERNAL_PRICE { get; set; }
        public string BID_NUM_ORDER { get; set; }
        public long SUPPLIER_ID { get; set; }
        public string BID_GROUP_CODE { get; set; }
        public string BID_PACKAGE_CODE { get; set; }
        public Nullable<long> EXPIRED_DATE { get; set; }
        public string CONCENTRA { get; set; }
        public Nullable<long> MANUFACTURER_ID { get; set; }
        public string NATIONAL_NAME { get; set; }
        public Nullable<decimal> TDL_CONTRACT_AMOUNT { get; set; }
        public string BID_MATERIAL_TYPE_CODE { get; set; }
        public string BID_MATERIAL_TYPE_NAME { get; set; }
        public string JOIN_BID_MATERIAL_TYPE_CODE { get; set; }
        public Nullable<long> MATERIAL_TYPE_MAP_ID { get; set; }
        public Nullable<long> MONTH_LIFESPAN { get; set; }
        public Nullable<long> DAY_LIFESPAN { get; set; }
        public Nullable<long> HOUR_LIFESPAN { get; set; }
        public string NOTE { get; set; }
        public Nullable<decimal> IMP_MORE_RATIO { get; set; }
        public Nullable<decimal> ADJUST_AMOUNT { get; set; }
        public string BID_NUMBER { get; set; }
        public string BID_NAME { get; set; }
        public Nullable<long> BID_TYPE_ID { get; set; }
        public string BID_YEAR { get; set; }
        public Nullable<long> VALID_FROM_TIME { get; set; }
        public Nullable<long> VALID_TO_TIME { get; set; }
        public string BID_EXTRA_CODE { get; set; }
        public string MATERIAL_TYPE_CODE { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public Nullable<long> SERVICE_ID { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public string MATERIAL_TYPE_MAP_CODE { get; set; }
        public string MATERIAL_TYPE_MAP_NAME { get; set; }
        public Nullable<long> SERVICE_UNIT_ID { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public string MANUFACTURER_CODE { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public Nullable<decimal> IN_AMOUNT { get; set; }
    }
}