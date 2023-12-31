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
    
    public partial class HIS_SERE_SERV_RATION
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
        public long SERVICE_ID { get; set; }
        public long SERVICE_REQ_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal PRICE { get; set; }
        public Nullable<decimal> DISCOUNT { get; set; }
        public Nullable<decimal> VAT_RATIO { get; set; }
        public string INSTRUCTION_NOTE { get; set; }
        public Nullable<long> OTHER_PAY_SOURCE_ID { get; set; }
        public Nullable<long> PRIMARY_PATIENT_TYPE_ID { get; set; }
        public Nullable<decimal> ACTUAL_PRICE { get; set; }
    
        public virtual HIS_OTHER_PAY_SOURCE HIS_OTHER_PAY_SOURCE { get; set; }
        public virtual HIS_PATIENT_TYPE HIS_PATIENT_TYPE { get; set; }
        public virtual HIS_PATIENT_TYPE HIS_PATIENT_TYPE1 { get; set; }
        public virtual HIS_SERVICE HIS_SERVICE { get; set; }
        public virtual HIS_SERVICE_REQ HIS_SERVICE_REQ { get; set; }
    }
}
