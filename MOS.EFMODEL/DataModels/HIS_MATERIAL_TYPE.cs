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
    
    public partial class HIS_MATERIAL_TYPE
    {
        public HIS_MATERIAL_TYPE()
        {
            this.HIS_ANTICIPATE_MATY = new HashSet<HIS_ANTICIPATE_MATY>();
            this.HIS_BID_MATERIAL_TYPE = new HashSet<HIS_BID_MATERIAL_TYPE>();
            this.HIS_DEBT_GOODS = new HashSet<HIS_DEBT_GOODS>();
            this.HIS_EMTE_MATERIAL_TYPE = new HashSet<HIS_EMTE_MATERIAL_TYPE>();
            this.HIS_EQUIPMENT_SET_MATY = new HashSet<HIS_EQUIPMENT_SET_MATY>();
            this.HIS_EXP_MEST_MATY_REQ = new HashSet<HIS_EXP_MEST_MATY_REQ>();
            this.HIS_MACHINE_SERV_MATY = new HashSet<HIS_MACHINE_SERV_MATY>();
            this.HIS_MATERIAL = new HashSet<HIS_MATERIAL>();
            this.HIS_MATERIAL_TYPE1 = new HashSet<HIS_MATERIAL_TYPE>();
            this.HIS_MATY_MATY = new HashSet<HIS_MATY_MATY>();
            this.HIS_MATY_MATY1 = new HashSet<HIS_MATY_MATY>();
            this.HIS_MEDI_CONTRACT_MATY = new HashSet<HIS_MEDI_CONTRACT_MATY>();
            this.HIS_MEDI_STOCK_MATY = new HashSet<HIS_MEDI_STOCK_MATY>();
            this.HIS_MEST_MATY_DEPA = new HashSet<HIS_MEST_MATY_DEPA>();
            this.HIS_MEST_PERIOD_MATY = new HashSet<HIS_MEST_PERIOD_MATY>();
            this.HIS_METY_MATY = new HashSet<HIS_METY_MATY>();
            this.HIS_QC_NORMATION = new HashSet<HIS_QC_NORMATION>();
            this.HIS_SERE_SERV_MATY = new HashSet<HIS_SERE_SERV_MATY>();
            this.HIS_SERVICE_MATY = new HashSet<HIS_SERVICE_MATY>();
            this.HIS_SERVICE_REQ_MATY = new HashSet<HIS_SERVICE_REQ_MATY>();
            this.HIS_TEST_INDEX = new HashSet<HIS_TEST_INDEX>();
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
        public string MATERIAL_TYPE_CODE { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public long SERVICE_ID { get; set; }
        public Nullable<long> PARENT_ID { get; set; }
        public Nullable<short> IS_LEAF { get; set; }
        public Nullable<long> NUM_ORDER { get; set; }
        public string CONCENTRA { get; set; }
        public Nullable<long> PACKING_TYPE_ID__DELETE { get; set; }
        public Nullable<long> MANUFACTURER_ID { get; set; }
        public long TDL_SERVICE_UNIT_ID { get; set; }
        public Nullable<long> TDL_GENDER_ID { get; set; }
        public string NATIONAL_NAME { get; set; }
        public Nullable<decimal> IMP_PRICE { get; set; }
        public Nullable<decimal> IMP_VAT_RATIO { get; set; }
        public Nullable<decimal> INTERNAL_PRICE { get; set; }
        public Nullable<long> ALERT_EXPIRED_DATE { get; set; }
        public Nullable<decimal> ALERT_MIN_IN_STOCK { get; set; }
        public Nullable<decimal> ALERT_MAX_IN_PRESCRIPTION { get; set; }
        public Nullable<short> IS_CHEMICAL_SUBSTANCE { get; set; }
        public Nullable<short> IS_AUTO_EXPEND { get; set; }
        public Nullable<short> IS_STENT { get; set; }
        public Nullable<short> IS_IN_KTC_FEE { get; set; }
        public Nullable<short> IS_ALLOW_ODD { get; set; }
        public Nullable<short> IS_ALLOW_EXPORT_ODD { get; set; }
        public Nullable<short> IS_STOP_IMP { get; set; }
        public Nullable<short> IS_REQUIRE_HSD { get; set; }
        public Nullable<short> IS_SALE_EQUAL_IMP_PRICE { get; set; }
        public Nullable<short> IS_BUSINESS { get; set; }
        public Nullable<short> IS_RAW_MATERIAL { get; set; }
        public Nullable<short> IS_MUST_PREPARE { get; set; }
        public string DESCRIPTION { get; set; }
        public Nullable<long> MEMA_GROUP_ID { get; set; }
        public string PACKING_TYPE_NAME { get; set; }
        public Nullable<short> IS_REUSABLE { get; set; }
        public Nullable<long> MAX_REUSE_COUNT { get; set; }
        public string MATERIAL_GROUP_BHYT { get; set; }
        public Nullable<long> MATERIAL_TYPE_MAP_ID { get; set; }
        public Nullable<decimal> LAST_EXP_PRICE { get; set; }
        public Nullable<decimal> LAST_EXP_VAT_RATIO { get; set; }
        public Nullable<decimal> LAST_IMP_PRICE { get; set; }
        public Nullable<decimal> LAST_IMP_VAT_RATIO { get; set; }
        public Nullable<long> FILM_SIZE_ID { get; set; }
        public Nullable<short> IS_FILM { get; set; }
        public Nullable<long> LAST_EXPIRED_DATE { get; set; }
        public string RECORDING_TRANSACTION { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public Nullable<short> IS_CONSUMABLE { get; set; }
        public Nullable<short> IS_OUT_HOSPITAL { get; set; }
        public Nullable<long> IMP_UNIT_ID { get; set; }
        public Nullable<decimal> IMP_UNIT_CONVERT_RATIO { get; set; }
        public Nullable<short> IS_DRUG_STORE { get; set; }
        public Nullable<short> IS_NOT_SHOW_TRACKING { get; set; }
        public string LOCKING_REASON { get; set; }
        public Nullable<decimal> ALERT_MAX_IN_DAY { get; set; }
        public string MODEL_CODE { get; set; }
    
        public virtual ICollection<HIS_ANTICIPATE_MATY> HIS_ANTICIPATE_MATY { get; set; }
        public virtual ICollection<HIS_BID_MATERIAL_TYPE> HIS_BID_MATERIAL_TYPE { get; set; }
        public virtual ICollection<HIS_DEBT_GOODS> HIS_DEBT_GOODS { get; set; }
        public virtual ICollection<HIS_EMTE_MATERIAL_TYPE> HIS_EMTE_MATERIAL_TYPE { get; set; }
        public virtual ICollection<HIS_EQUIPMENT_SET_MATY> HIS_EQUIPMENT_SET_MATY { get; set; }
        public virtual ICollection<HIS_EXP_MEST_MATY_REQ> HIS_EXP_MEST_MATY_REQ { get; set; }
        public virtual HIS_FILM_SIZE HIS_FILM_SIZE { get; set; }
        public virtual ICollection<HIS_MACHINE_SERV_MATY> HIS_MACHINE_SERV_MATY { get; set; }
        public virtual HIS_MANUFACTURER HIS_MANUFACTURER { get; set; }
        public virtual ICollection<HIS_MATERIAL> HIS_MATERIAL { get; set; }
        public virtual HIS_PACKING_TYPE HIS_PACKING_TYPE { get; set; }
        public virtual HIS_SERVICE_UNIT HIS_SERVICE_UNIT { get; set; }
        public virtual HIS_SERVICE HIS_SERVICE { get; set; }
        public virtual ICollection<HIS_MATERIAL_TYPE> HIS_MATERIAL_TYPE1 { get; set; }
        public virtual HIS_MATERIAL_TYPE HIS_MATERIAL_TYPE2 { get; set; }
        public virtual HIS_MEMA_GROUP HIS_MEMA_GROUP { get; set; }
        public virtual HIS_MATERIAL_TYPE_MAP HIS_MATERIAL_TYPE_MAP { get; set; }
        public virtual ICollection<HIS_MATY_MATY> HIS_MATY_MATY { get; set; }
        public virtual ICollection<HIS_MATY_MATY> HIS_MATY_MATY1 { get; set; }
        public virtual ICollection<HIS_MEDI_CONTRACT_MATY> HIS_MEDI_CONTRACT_MATY { get; set; }
        public virtual ICollection<HIS_MEDI_STOCK_MATY> HIS_MEDI_STOCK_MATY { get; set; }
        public virtual ICollection<HIS_MEST_MATY_DEPA> HIS_MEST_MATY_DEPA { get; set; }
        public virtual ICollection<HIS_MEST_PERIOD_MATY> HIS_MEST_PERIOD_MATY { get; set; }
        public virtual ICollection<HIS_METY_MATY> HIS_METY_MATY { get; set; }
        public virtual ICollection<HIS_QC_NORMATION> HIS_QC_NORMATION { get; set; }
        public virtual ICollection<HIS_SERE_SERV_MATY> HIS_SERE_SERV_MATY { get; set; }
        public virtual ICollection<HIS_SERVICE_MATY> HIS_SERVICE_MATY { get; set; }
        public virtual ICollection<HIS_SERVICE_REQ_MATY> HIS_SERVICE_REQ_MATY { get; set; }
        public virtual ICollection<HIS_TEST_INDEX> HIS_TEST_INDEX { get; set; }
    }
}
