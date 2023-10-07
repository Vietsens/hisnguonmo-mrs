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
    
    public partial class HIS_IMP_MEST_PAY
    {
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
        public long IMP_MEST_PROPOSE_ID { get; set; }
        public long PAY_TIME { get; set; }
        public decimal AMOUNT { get; set; }
        public Nullable<long> PAY_FORM_ID { get; set; }
        public string PAYER_LOGINNAME { get; set; }
        public string PAYER_USERNAME { get; set; }
        public Nullable<long> NEXT_PAY_TIME { get; set; }
        public Nullable<decimal> NEXT_AMOUNT { get; set; }
        public string DESCRIPTION { get; set; }
        public string STANDING_ORDER_CODE { get; set; }
        public string PAYSLIP_CODE { get; set; }
    
        public virtual HIS_IMP_MEST_PROPOSE HIS_IMP_MEST_PROPOSE { get; set; }
        public virtual HIS_PAY_FORM HIS_PAY_FORM { get; set; }
    }
}