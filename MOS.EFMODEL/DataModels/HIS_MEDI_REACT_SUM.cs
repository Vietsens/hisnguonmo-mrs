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
    
    public partial class HIS_MEDI_REACT_SUM
    {
        public HIS_MEDI_REACT_SUM()
        {
            this.HIS_MEDI_REACT = new HashSet<HIS_MEDI_REACT>();
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
        public long TREATMENT_ID { get; set; }
        public Nullable<long> DEPARTMENT_ID { get; set; }
        public Nullable<long> ROOM_ID { get; set; }
        public Nullable<long> ICD_ID__DELETE { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_TEXT { get; set; }
    
        public virtual HIS_DEPARTMENT HIS_DEPARTMENT { get; set; }
        public virtual ICollection<HIS_MEDI_REACT> HIS_MEDI_REACT { get; set; }
        public virtual HIS_TREATMENT HIS_TREATMENT { get; set; }
        public virtual HIS_ROOM HIS_ROOM { get; set; }
    }
}
