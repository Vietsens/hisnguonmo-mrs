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
    
    public partial class HIS_EXP_MEST_MATY_REQ
    {
        public HIS_EXP_MEST_MATY_REQ()
        {
            this.HIS_BCS_MATY_REQ_DT = new HashSet<HIS_BCS_MATY_REQ_DT>();
            this.HIS_BCS_MATY_REQ_REQ = new HashSet<HIS_BCS_MATY_REQ_REQ>();
            this.HIS_BCS_MATY_REQ_REQ1 = new HashSet<HIS_BCS_MATY_REQ_REQ>();
            this.HIS_EXP_MEST_MATERIAL = new HashSet<HIS_EXP_MEST_MATERIAL>();
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
        public long EXP_MEST_ID { get; set; }
        public long MATERIAL_TYPE_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public Nullable<long> NUM_ORDER { get; set; }
        public string DESCRIPTION { get; set; }
        public long TDL_MEDI_STOCK_ID { get; set; }
        public Nullable<decimal> DD_AMOUNT { get; set; }
        public Nullable<long> TREATMENT_ID { get; set; }
        public Nullable<decimal> BCS_REQ_AMOUNT { get; set; }
    
        public virtual ICollection<HIS_BCS_MATY_REQ_DT> HIS_BCS_MATY_REQ_DT { get; set; }
        public virtual ICollection<HIS_BCS_MATY_REQ_REQ> HIS_BCS_MATY_REQ_REQ { get; set; }
        public virtual ICollection<HIS_BCS_MATY_REQ_REQ> HIS_BCS_MATY_REQ_REQ1 { get; set; }
        public virtual HIS_EXP_MEST HIS_EXP_MEST { get; set; }
        public virtual ICollection<HIS_EXP_MEST_MATERIAL> HIS_EXP_MEST_MATERIAL { get; set; }
        public virtual HIS_MATERIAL_TYPE HIS_MATERIAL_TYPE { get; set; }
        public virtual HIS_TREATMENT HIS_TREATMENT { get; set; }
    }
}
