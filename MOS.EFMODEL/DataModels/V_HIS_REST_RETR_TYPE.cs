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
    
    public partial class V_HIS_REST_RETR_TYPE
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
        public long REHA_SERVICE_TYPE_ID { get; set; }
        public long REHA_TRAIN_TYPE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public Nullable<long> NUM_ORDER { get; set; }
        public Nullable<short> IS_LEAF { get; set; }
        public string REHA_TRAIN_TYPE_CODE { get; set; }
        public string REHA_TRAIN_TYPE_NAME { get; set; }
        public long REHA_TRAIN_UNIT_ID { get; set; }
        public string REHA_TRAIN_UNIT_CODE { get; set; }
        public string REHA_TRAIN_UNIT_NAME { get; set; }
    }
}
