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
    
    public partial class V_HIS_SERE_SERV_RATION
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
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public long SERVICE_UNIT_ID { get; set; }
        public Nullable<long> RATION_GROUP_ID { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string OTHER_PAY_SOURCE_NAME { get; set; }
        public string OTHER_PAY_SOURCE_CODE { get; set; }
        public Nullable<long> RATION_TIME_ID { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public long TREATMENT_ID { get; set; }
        public Nullable<long> TRACKING_ID { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public long EXECUTE_ROOM_ID { get; set; }
        public long REQUEST_ROOM_ID { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string RATION_TIME_CODE { get; set; }
        public string RATION_TIME_NAME { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public Nullable<short> IS_RATION { get; set; }
        public string PRIMARY_PATIENT_TYPE_CODE { get; set; }
        public string PRIMARY_PATIENT_TYPE_NAME { get; set; }
        public Nullable<long> TDL_PATIENT_CLASSIFY_ID { get; set; }
        public long IN_TIME { get; set; }
    }
}