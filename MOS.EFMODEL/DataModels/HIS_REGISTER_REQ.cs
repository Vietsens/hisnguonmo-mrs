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
    
    public partial class HIS_REGISTER_REQ
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
        public long REGISTER_GATE_ID { get; set; }
        public long REGISTER_TIME { get; set; }
        public long NUM_ORDER { get; set; }
        public Nullable<long> PATIENT_ID { get; set; }
        public string CARD_CODE { get; set; }
        public string SERVICE_CODE { get; set; }
        public Nullable<short> IS_UPDATE_FROM { get; set; }
        public Nullable<long> REGISTER_DATE { get; set; }
        public Nullable<long> CALL_TIME { get; set; }
        public string CALL_PLACE { get; set; }
        public Nullable<long> VIR_CALL_DATE { get; set; }
    
        public virtual HIS_PATIENT HIS_PATIENT { get; set; }
        public virtual HIS_REGISTER_GATE HIS_REGISTER_GATE { get; set; }
    }
}
