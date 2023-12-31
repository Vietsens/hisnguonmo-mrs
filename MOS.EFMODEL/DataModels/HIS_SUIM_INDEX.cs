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
    
    public partial class HIS_SUIM_INDEX
    {
        public HIS_SUIM_INDEX()
        {
            this.HIS_SERE_SERV_SUIN = new HashSet<HIS_SERE_SERV_SUIN>();
            this.HIS_SERVICE = new HashSet<HIS_SERVICE>();
            this.HIS_SUIM_SETY_SUIN = new HashSet<HIS_SUIM_SETY_SUIN>();
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
        public string SUIM_INDEX_CODE { get; set; }
        public string SUIM_INDEX_NAME { get; set; }
        public Nullable<long> SUIM_INDEX_UNIT_ID { get; set; }
        public Nullable<long> NUM_ORDER { get; set; }
    
        public virtual ICollection<HIS_SERE_SERV_SUIN> HIS_SERE_SERV_SUIN { get; set; }
        public virtual ICollection<HIS_SERVICE> HIS_SERVICE { get; set; }
        public virtual HIS_SUIM_INDEX_UNIT HIS_SUIM_INDEX_UNIT { get; set; }
        public virtual ICollection<HIS_SUIM_SETY_SUIN> HIS_SUIM_SETY_SUIN { get; set; }
    }
}
