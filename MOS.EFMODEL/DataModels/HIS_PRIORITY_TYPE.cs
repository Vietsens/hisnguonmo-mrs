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
    
    public partial class HIS_PRIORITY_TYPE
    {
        public HIS_PRIORITY_TYPE()
        {
            this.HIS_EXP_MEST = new HashSet<HIS_EXP_MEST>();
            this.HIS_SERVICE_REQ = new HashSet<HIS_SERVICE_REQ>();
        }
    
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
        public string PRIORITY_TYPE_CODE { get; set; }
        public string PRIORITY_TYPE_NAME { get; set; }
        public Nullable<long> AGE_FROM { get; set; }
        public Nullable<long> AGE_TO { get; set; }
        public string BHYT_PREFIXS { get; set; }
        public Nullable<short> IS_FOR_PRESCRIPTION { get; set; }
        public Nullable<short> IS_FOR_EXAM_SUBCLINICAL { get; set; }
    
        public virtual ICollection<HIS_EXP_MEST> HIS_EXP_MEST { get; set; }
        public virtual ICollection<HIS_SERVICE_REQ> HIS_SERVICE_REQ { get; set; }
    }
}
