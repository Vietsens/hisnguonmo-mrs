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
    
    public partial class V_HIS_SERVICE_TYPE
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
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public Nullable<long> NUM_ORDER { get; set; }
        public Nullable<long> EXE_SERVICE_MODULE_ID { get; set; }
        public Nullable<short> IS_AUTO_SPLIT_REQ { get; set; }
        public Nullable<short> IS_NOT_DISPLAY_ASSIGN { get; set; }
        public string EXE_SERVICE_MODULE_NAME { get; set; }
        public string MODULE_LINK { get; set; }
    }
}
