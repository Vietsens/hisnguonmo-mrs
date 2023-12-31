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
    
    public partial class V_HIS_MEDICINE_2
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
        public long MEDICINE_TYPE_ID { get; set; }
        public Nullable<long> SUPPLIER_ID { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public Nullable<long> EXPIRED_DATE { get; set; }
        public decimal AMOUNT { get; set; }
        public Nullable<long> IMP_SOURCE_ID { get; set; }
        public Nullable<long> IMP_TIME { get; set; }
        public decimal IMP_PRICE { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }
        public Nullable<decimal> INTERNAL_PRICE { get; set; }
        public Nullable<long> BID_ID { get; set; }
        public string TDL_BID_NUMBER { get; set; }
        public string TDL_BID_NUM_ORDER { get; set; }
        public string TDL_BID_GROUP_CODE { get; set; }
        public string TDL_BID_PACKAGE_CODE { get; set; }
        public string TDL_BID_YEAR { get; set; }
        public string MEDICINE_REGISTER_NUMBER { get; set; }
        public string MEDICINE_BYT_NUM_ORDER { get; set; }
        public string MEDICINE_TCY_NUM_ORDER { get; set; }
        public Nullable<short> MEDICINE_IS_STAR_MARK { get; set; }
        public Nullable<short> IS_PREGNANT { get; set; }
        public Nullable<short> IS_SALE_EQUAL_IMP_PRICE { get; set; }
        public long TDL_SERVICE_ID { get; set; }
        public Nullable<decimal> VIR_IMP_PRICE { get; set; }
        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public Nullable<long> DOCUMENT_PRICE { get; set; }
        public string NATIONAL_NAME { get; set; }
        public Nullable<long> MANUFACTURER_ID { get; set; }
        public string CONCENTRA { get; set; }
        public string TDL_IMP_MEST_CODE { get; set; }
        public string TDL_IMP_MEST_SUB_CODE { get; set; }
        public Nullable<decimal> IMP_UNIT_AMOUNT { get; set; }
        public Nullable<decimal> IMP_UNIT_PRICE { get; set; }
        public Nullable<long> TDL_IMP_UNIT_ID { get; set; }
        public Nullable<decimal> TDL_IMP_UNIT_CONVERT_RATIO { get; set; }
        public Nullable<long> MEDICAL_CONTRACT_ID { get; set; }
        public Nullable<decimal> CONTRACT_PRICE { get; set; }
        public Nullable<decimal> PROFIT_RATIO { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public string HEIN_SERVICE_BHYT_NAME { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public Nullable<long> MEDICINE_USE_FORM_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public string BID_NAME { get; set; }
        public string BID_NUMBER { get; set; }
        public string BID_YEAR { get; set; }
        public Nullable<long> VALID_FROM_TIME { get; set; }
        public Nullable<long> VALID_TO_TIME { get; set; }
    }
}
