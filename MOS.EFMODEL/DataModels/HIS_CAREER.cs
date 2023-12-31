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
    
    public partial class HIS_CAREER
    {
        public HIS_CAREER()
        {
            this.HIS_APPOINTMENT = new HashSet<HIS_APPOINTMENT>();
            this.HIS_BHYT_WHITELIST = new HashSet<HIS_BHYT_WHITELIST>();
            this.HIS_BLOOD_GIVER = new HashSet<HIS_BLOOD_GIVER>();
            this.HIS_PATIENT = new HashSet<HIS_PATIENT>();
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
        public string CAREER_CODE { get; set; }
        public string CAREER_NAME { get; set; }
    
        public virtual ICollection<HIS_APPOINTMENT> HIS_APPOINTMENT { get; set; }
        public virtual ICollection<HIS_BHYT_WHITELIST> HIS_BHYT_WHITELIST { get; set; }
        public virtual ICollection<HIS_BLOOD_GIVER> HIS_BLOOD_GIVER { get; set; }
        public virtual ICollection<HIS_PATIENT> HIS_PATIENT { get; set; }
    }
}
