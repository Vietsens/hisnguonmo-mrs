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
    
    public partial class HIS_IMP_MEST_MATERIAL
    {
        public HIS_IMP_MEST_MATERIAL()
        {
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
        public long IMP_MEST_ID { get; set; }
        public long MATERIAL_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public Nullable<decimal> VAT_RATIO { get; set; }
        public Nullable<decimal> BK_AMOUNT { get; set; }
        public Nullable<long> TH_EXP_MEST_MATERIAL_ID { get; set; }
        public Nullable<decimal> VIR_PRICE { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public Nullable<long> REMAIN_REUSE_COUNT { get; set; }
        public Nullable<long> DOCUMENT_PRICE { get; set; }
        public Nullable<decimal> IMP_UNIT_AMOUNT { get; set; }
        public Nullable<decimal> IMP_UNIT_PRICE { get; set; }
        public Nullable<long> TDL_IMP_UNIT_ID { get; set; }
        public Nullable<decimal> TDL_IMP_UNIT_CONVERT_RATIO { get; set; }
        public Nullable<decimal> CONTRACT_PRICE { get; set; }
        public Nullable<decimal> REQ_AMOUNT { get; set; }
        public string NOTE { get; set; }
    
        public virtual ICollection<HIS_EXP_MEST_MATERIAL> HIS_EXP_MEST_MATERIAL { get; set; }
        public virtual HIS_EXP_MEST_MATERIAL HIS_EXP_MEST_MATERIAL1 { get; set; }
        public virtual HIS_IMP_MEST HIS_IMP_MEST { get; set; }
        public virtual HIS_MATERIAL HIS_MATERIAL { get; set; }
    }
}
