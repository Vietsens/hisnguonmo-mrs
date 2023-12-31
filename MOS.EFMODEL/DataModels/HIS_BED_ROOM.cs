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
    
    public partial class HIS_BED_ROOM
    {
        public HIS_BED_ROOM()
        {
            this.HIS_BED = new HashSet<HIS_BED>();
            this.HIS_TREATMENT_BED_ROOM = new HashSet<HIS_TREATMENT_BED_ROOM>();
            this.HIS_TREATMENT_ROOM = new HashSet<HIS_TREATMENT_ROOM>();
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
        public string BED_ROOM_CODE { get; set; }
        public string BED_ROOM_NAME { get; set; }
        public long ROOM_ID { get; set; }
        public Nullable<short> IS_SURGERY { get; set; }
        public string TREATMENT_TYPE_IDS { get; set; }
    
        public virtual ICollection<HIS_BED> HIS_BED { get; set; }
        public virtual HIS_ROOM HIS_ROOM { get; set; }
        public virtual ICollection<HIS_TREATMENT_BED_ROOM> HIS_TREATMENT_BED_ROOM { get; set; }
        public virtual ICollection<HIS_TREATMENT_ROOM> HIS_TREATMENT_ROOM { get; set; }
    }
}
