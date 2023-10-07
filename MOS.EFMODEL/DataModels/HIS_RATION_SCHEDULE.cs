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
    
    public partial class HIS_RATION_SCHEDULE
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
        public Nullable<long> TREATMENT_ID { get; set; }
        public Nullable<long> RATION_TIME_ID { get; set; }
        public Nullable<long> PATIENT_TYPE_ID { get; set; }
        public Nullable<long> SERVICE_ID { get; set; }
        public Nullable<long> REFECTORY_ROOM_ID { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public Nullable<long> FROM_TIME { get; set; }
        public Nullable<long> TO_TIME { get; set; }
        public Nullable<short> IS_FOR_HOMIE { get; set; }
        public Nullable<short> IS_HALF_IN_FIRST_DAY { get; set; }
        public string NOTE { get; set; }
        public Nullable<long> LAST_ASSIGN_DATE { get; set; }
        public Nullable<long> DEPARTMENT_ID { get; set; }
        public Nullable<long> VIR_FROM_DATE { get; set; }
        public Nullable<long> VIR_TO_DATE { get; set; }
    
        public virtual HIS_PATIENT_TYPE HIS_PATIENT_TYPE { get; set; }
        public virtual HIS_RATION_TIME HIS_RATION_TIME { get; set; }
        public virtual HIS_REFECTORY HIS_REFECTORY { get; set; }
        public virtual HIS_SERVICE HIS_SERVICE { get; set; }
    }
}
