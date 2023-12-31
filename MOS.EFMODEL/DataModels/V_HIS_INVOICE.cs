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
    
    public partial class V_HIS_INVOICE
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
        public long INVOICE_BOOK_ID { get; set; }
        public long NUM_ORDER { get; set; }
        public long INVOICE_TIME { get; set; }
        public long PAY_FORM_ID { get; set; }
        public string SELLER_NAME { get; set; }
        public string SELLER_ADDRESS { get; set; }
        public string SELLER_ACCOUNT_NUMBER { get; set; }
        public string SELLER_TAX_CODE { get; set; }
        public string SELLER_PHONE { get; set; }
        public string BUYER_NAME { get; set; }
        public string BUYER_ADDRESS { get; set; }
        public string BUYER_ACCOUNT_NUMBER { get; set; }
        public string BUYER_TAX_CODE { get; set; }
        public Nullable<short> IS_CANCEL { get; set; }
        public string CANCEL_REASON { get; set; }
        public Nullable<long> CANCEL_TIME { get; set; }
        public string CANCEL_LOGINNAME { get; set; }
        public string CANCEL_USERNAME { get; set; }
        public Nullable<decimal> DISCOUNT { get; set; }
        public string BUYER_ORGANIZATION { get; set; }
        public string VIR_NUM_ORDER { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<decimal> VAT_RATIO { get; set; }
        public string VIR_UNIQUE { get; set; }
        public string TEMPLATE_CODE { get; set; }
        public string SYMBOL_CODE { get; set; }
        public long TOTAL { get; set; }
        public long FROM_NUM_ORDER { get; set; }
        public string PAY_FORM_CODE { get; set; }
        public string PAY_FORM_NAME { get; set; }
    }
}
