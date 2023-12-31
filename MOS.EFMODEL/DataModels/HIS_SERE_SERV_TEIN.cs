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
    
    public partial class HIS_SERE_SERV_TEIN
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
        public long SERE_SERV_ID { get; set; }
        public Nullable<long> TEST_INDEX_ID { get; set; }
        public string VALUE { get; set; }
        public string RESULT_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<long> TDL_TREATMENT_ID { get; set; }
        public Nullable<long> MACHINE_ID { get; set; }
        public string BACTERIUM_CODE { get; set; }
        public string BACTERIUM_NAME { get; set; }
        public string BACTERIUM_NOTE { get; set; }
        public string BACTERIUM_AMOUNT { get; set; }
        public string BACTERIUM_DENSITY { get; set; }
        public string ANTIBIOTIC_RESISTANCE_CODE { get; set; }
        public string ANTIBIOTIC_RESISTANCE_NAME { get; set; }
        public string SRI_CODE { get; set; }
        public string NOTE { get; set; }
        public string LEAVEN { get; set; }
        public Nullable<long> EXP_MEST_ID { get; set; }
        public Nullable<long> TDL_SERVICE_REQ_ID { get; set; }
        public string OLD_VALUE { get; set; }
        public string RESULT_DESCRIPTION { get; set; }
    
        public virtual HIS_EXP_MEST HIS_EXP_MEST { get; set; }
        public virtual HIS_MACHINE HIS_MACHINE { get; set; }
        public virtual HIS_SERE_SERV HIS_SERE_SERV { get; set; }
        public virtual HIS_TEST_INDEX HIS_TEST_INDEX { get; set; }
    }
}
