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
    
    public partial class HIS_EXP_MEST_TEMPLATE
    {
        public HIS_EXP_MEST_TEMPLATE()
        {
            this.HIS_EMTE_MATERIAL_TYPE = new HashSet<HIS_EMTE_MATERIAL_TYPE>();
            this.HIS_EMTE_MEDICINE_TYPE = new HashSet<HIS_EMTE_MEDICINE_TYPE>();
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
        public string EXP_MEST_TEMPLATE_CODE { get; set; }
        public string EXP_MEST_TEMPLATE_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<short> IS_PUBLIC { get; set; }
        public Nullable<short> IS_KIDNEY { get; set; }
        public Nullable<short> PRESCRIPTION_TYPE_ID { get; set; }
        public Nullable<long> REMEDY_COUNT { get; set; }
    
        public virtual ICollection<HIS_EMTE_MATERIAL_TYPE> HIS_EMTE_MATERIAL_TYPE { get; set; }
        public virtual ICollection<HIS_EMTE_MEDICINE_TYPE> HIS_EMTE_MEDICINE_TYPE { get; set; }
        public virtual ICollection<HIS_SERVICE_REQ> HIS_SERVICE_REQ { get; set; }
    }
}
