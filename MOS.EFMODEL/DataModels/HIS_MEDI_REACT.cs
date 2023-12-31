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
    
    public partial class HIS_MEDI_REACT
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
        public long MEDI_REACT_TYPE_ID { get; set; }
        public long MEDI_REACT_SUM_ID { get; set; }
        public Nullable<long> MEDICINE_ID { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public Nullable<long> EXECUTE_TIME { get; set; }
        public string CHECK_LOGINNAME { get; set; }
        public string CHECK_USERNAME { get; set; }
        public Nullable<long> CHECK_TIME { get; set; }
        public string CHECK_RESULT { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public Nullable<long> EXPIRED_DATE { get; set; }
    
        public virtual HIS_MEDI_REACT_TYPE HIS_MEDI_REACT_TYPE { get; set; }
        public virtual HIS_MEDICINE HIS_MEDICINE { get; set; }
        public virtual HIS_MEDI_REACT_SUM HIS_MEDI_REACT_SUM { get; set; }
    }
}
