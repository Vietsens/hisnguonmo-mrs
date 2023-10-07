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
    
    public partial class HIS_NONE_MEDI_SERVICE
    {
        public HIS_NONE_MEDI_SERVICE()
        {
            this.HIS_BILL_GOODS = new HashSet<HIS_BILL_GOODS>();
            this.HIS_DEBT_GOODS = new HashSet<HIS_DEBT_GOODS>();
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
        public string NONE_MEDI_SERVICE_CODE { get; set; }
        public string NONE_MEDI_SERVICE_NAME { get; set; }
        public Nullable<decimal> PRICE { get; set; }
        public long SERVICE_UNIT_ID { get; set; }
        public Nullable<long> NUM_ORDER { get; set; }
    
        public virtual ICollection<HIS_BILL_GOODS> HIS_BILL_GOODS { get; set; }
        public virtual ICollection<HIS_DEBT_GOODS> HIS_DEBT_GOODS { get; set; }
        public virtual HIS_SERVICE_UNIT HIS_SERVICE_UNIT { get; set; }
    }
}