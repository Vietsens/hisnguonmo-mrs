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
    
    public partial class V_HIS_MEDICINE_TYPE_TUT
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
        public string LOGINNAME { get; set; }
        public Nullable<long> HTU_ID { get; set; }
        public Nullable<long> MEDICINE_USE_FORM_ID { get; set; }
        public Nullable<long> DAY_COUNT { get; set; }
        public string MORNING { get; set; }
        public string NOON { get; set; }
        public string AFTERNOON { get; set; }
        public string EVENING { get; set; }
        public string TUTORIAL { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_USE_FORM_CODE { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public string HTU_NAME { get; set; }
    }
}