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
    
    public partial class V_HIS_EMTE_MEDICINE_TYPE
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
        public long EXP_MEST_TEMPLATE_ID { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public Nullable<long> MEDICINE_TYPE_ID { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public Nullable<long> SERVICE_UNIT_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public string TUTORIAL { get; set; }
        public Nullable<short> IS_EXPEND { get; set; }
        public Nullable<short> IS_OUT_MEDI_STOCK { get; set; }
        public Nullable<long> HTU_ID { get; set; }
        public Nullable<long> DAY_COUNT { get; set; }
        public string MORNING { get; set; }
        public string NOON { get; set; }
        public string AFTERNOON { get; set; }
        public string EVENING { get; set; }
        public string EXP_MEST_TEMPLATE_CODE { get; set; }
        public string EXP_MEST_TEMPLATE_NAME { get; set; }
        public Nullable<long> REMEDY_COUNT { get; set; }
        public Nullable<short> PRESCRIPTION_TYPE_ID { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public Nullable<long> SERVICE_ID { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public Nullable<decimal> CONVERT_RATIO { get; set; }
        public string CONVERT_UNIT_CODE { get; set; }
        public string CONVERT_UNIT_NAME { get; set; }
    }
}
