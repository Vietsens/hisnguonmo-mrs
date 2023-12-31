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
    
    public partial class HIS_HTU
    {
        public HIS_HTU()
        {
            this.HIS_EMTE_MEDICINE_TYPE = new HashSet<HIS_EMTE_MEDICINE_TYPE>();
            this.HIS_EXP_MEST_MEDICINE = new HashSet<HIS_EXP_MEST_MEDICINE>();
            this.HIS_MEDICINE_TYPE = new HashSet<HIS_MEDICINE_TYPE>();
            this.HIS_MEDICINE_TYPE_TUT = new HashSet<HIS_MEDICINE_TYPE_TUT>();
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
        public string HTU_NAME { get; set; }
        public Nullable<long> NUM_ORDER { get; set; }
        public string HTU_CODE { get; set; }
    
        public virtual ICollection<HIS_EMTE_MEDICINE_TYPE> HIS_EMTE_MEDICINE_TYPE { get; set; }
        public virtual ICollection<HIS_EXP_MEST_MEDICINE> HIS_EXP_MEST_MEDICINE { get; set; }
        public virtual ICollection<HIS_MEDICINE_TYPE> HIS_MEDICINE_TYPE { get; set; }
        public virtual ICollection<HIS_MEDICINE_TYPE_TUT> HIS_MEDICINE_TYPE_TUT { get; set; }
    }
}
