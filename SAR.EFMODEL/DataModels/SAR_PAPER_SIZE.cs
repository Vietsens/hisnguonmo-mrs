//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SAR.EFMODEL.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class SAR_PAPER_SIZE
    {
        public SAR_PAPER_SIZE()
        {
            this.SAR_REPORT = new HashSet<SAR_REPORT>();
            this.SAR_REPORT_TEMPLATE = new HashSet<SAR_REPORT_TEMPLATE>();
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
        public string PAPER_SIZE_CODE { get; set; }
        public string PAPER_SIZE_NAME { get; set; }
    
        public virtual ICollection<SAR_REPORT> SAR_REPORT { get; set; }
        public virtual ICollection<SAR_REPORT_TEMPLATE> SAR_REPORT_TEMPLATE { get; set; }
    }
}