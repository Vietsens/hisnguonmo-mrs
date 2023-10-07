using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00382
{
    class Mrs00382RDO : AbstractCloneable
    {
        //thong tin chi tiet
        public string ACTIVE_INGR_BHYT_CODE { get; set; }
        public string ACTIVE_INGR_BHYT_NAME { get; set; }
        public string AFTERNOON { get; set; }
        public long? AGGR_EXP_MEST_ID { get; set; }
        public decimal AMOUNT { get; set; }
        public long? APPROVAL_DATE { get; set; }
        public string APPROVAL_LOGINNAME { get; set; }
        public long? APPROVAL_TIME { get; set; }
        public string APPROVAL_USERNAME { get; set; }
        public decimal? BCS_REQ_AMOUNT { get; set; }
        public long? BID_ID { get; set; }
        public string BID_NAME { get; set; }
        public string BID_NUMBER { get; set; }
        public decimal? BK_AMOUNT { get; set; }
        public string BREATH_SPEED { get; set; }
        public string BREATH_TIME { get; set; }
        public string BYT_NUM_ORDER { get; set; }
        public long? CK_IMP_MEST_MEDICINE_ID { get; set; }
        public string CONCENTRA { get; set; }
        public decimal? CONVERT_RATIO { get; set; }
        public string CONVERT_UNIT_CODE { get; set; }
        public string CONVERT_UNIT_NAME { get; set; }
        public long? CREATE_TIME { get; set; }
        public string CREATOR { get; set; }
        public long? DAY_COUNT { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal? DISCOUNT { get; set; }
        public string EVENING { get; set; }
        public long? EXP_DATE { get; set; }
        public string EXP_LOGINNAME { get; set; }
        public string EXP_MEST_CODE { get; set; }
        public long? EXP_MEST_ID { get; set; }
        public long? EXP_MEST_METY_REQ_ID { get; set; }
        public long EXP_MEST_STT_ID { get; set; }
        public long EXP_MEST_TYPE_ID { get; set; }
        public long? EXP_TIME { get; set; }
        public string EXP_USERNAME { get; set; }
        public long? EXPEND_TYPE_ID { get; set; }
        public long? EXPIRED_DATE { get; set; }
        public string GROUP_CODE { get; set; }
        public long? HTU_ID { get; set; }
        public string HTU_NAME { get; set; }
        public long ID { get; set; }
        public decimal IMP_PRICE { get; set; }
        public long? IMP_TIME { get; set; }
        public decimal IMP_VAT_RATIO { get; set; }
        public decimal? INTERNAL_PRICE { get; set; }
        public short? IS_ACTIVE { get; set; }
        public short? IS_ALLOW_ODD { get; set; }
        public short? IS_CREATED_BY_APPROVAL { get; set; }
        public short? IS_DELETE { get; set; }
        public short? IS_EXPEND { get; set; }
        public short? IS_EXPORT { get; set; }
        public short? IS_FUNCTIONAL_FOOD { get; set; }
        public short? IS_NOT_PRES { get; set; }
        public short? IS_OUT_HOSPITAL { get; set; }
        public short? IS_OUT_PARENT_FEE { get; set; }
        public short? IS_USE_CLIENT_PRICE { get; set; }
        public short? IS_USED { get; set; }
        public string MANUFACTURER_CODE { get; set; }
        public long? MANUFACTURER_ID { get; set; }
        public string MANUFACTURER_NAME { get; set; }
        public long? MATERIAL_NUM_ORDER { get; set; }
        public long MEDI_STOCK_ID { get; set; }
        public long? MEDI_STOCK_PERIOD_ID { get; set; }
        public string MEDICINE_BYT_NUM_ORDER { get; set; }
        public string MEDICINE_GROUP_CODE { get; set; }
        public long? MEDICINE_GROUP_ID { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }
        public long? MEDICINE_GROUP_NUM_ORDER { get; set; }
        public long? MEDICINE_ID { get; set; }
        public long? MEDICINE_NUM_ORDER { get; set; }
        public string MEDICINE_REGISTER_NUMBER { get; set; }
        public string MEDICINE_TCY_NUM_ORDER { get; set; }
        public string MEDICINE_TYPE_CODE { get; set; }
        public long MEDICINE_TYPE_ID { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public long? MEDICINE_TYPE_NUM_ORDER { get; set; }
        public string MEDICINE_USE_FORM_CODE { get; set; }
        public long? MEDICINE_USE_FORM_ID { get; set; }
        public string MEDICINE_USE_FORM_NAME { get; set; }
        public long? MEDICINE_USE_FORM_NUM_ORDER { get; set; }
        public long? MEMA_GROUP_ID { get; set; }
        public string MODIFIER { get; set; }
        public long? MODIFY_TIME { get; set; }
        public string MORNING { get; set; }
        public string NATIONAL_NAME { get; set; }
        public string NOON { get; set; }
        public long? NUM_ORDER { get; set; }
        public long? OTHER_PAY_SOURCE_ID { get; set; }
        public string PACKAGE_NUMBER { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public long? PREVIOUS_USING_COUNT { get; set; }
        public decimal PRICE { get; set; }
        public string RECORDING_TRANSACTION { get; set; }
        public string REGISTER_NUMBER { get; set; }
        public long REQ_DEPARTMENT_ID { get; set; }
        public long REQ_ROOM_ID { get; set; }
        public string REQ_USER_TITLE { get; set; }
        public long? SERE_SERV_PARENT_ID { get; set; }
        public long? SERVICE_CONDITION_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_UNIT_CODE { get; set; }
        public long SERVICE_UNIT_ID { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public decimal? SPEED { get; set; }
        public string SUPPLIER_CODE { get; set; }
        public long? SUPPLIER_ID { get; set; }
        public string SUPPLIER_NAME { get; set; }
        public decimal? TAX_RATIO { get; set; }
        public string TCY_NUM_ORDER { get; set; }
        public long? TDL_AGGR_EXP_MEST_ID { get; set; }
        public long? TDL_INTRUCTION_TIME { get; set; }
        public long? TDL_MEDI_STOCK_ID { get; set; }
        public long? TDL_MEDICINE_TYPE_ID { get; set; }
        public string TDL_PRES_REQ_USER_TITLE { get; set; }
        public long? TDL_SERVICE_REQ_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public long? TDL_VACCINATION_ID { get; set; }
        public decimal? TH_AMOUNT { get; set; }
        public string TUTORIAL { get; set; }
        public short? USE_ORIGINAL_UNIT_FOR_PRES { get; set; }
        public long? USE_TIME_TO { get; set; }
        public long? VACCINATION_RESULT_ID { get; set; }
        public decimal VAT_RATIO { get; set; }
        public decimal? VIR_PRICE { get; set; }
        //het thong tin chi tiet
        public string MEDI_MATE_TYPE_CODE { get; set; }
        public string MEDI_MATE_TYPE_NAME { get;  set;  }
        public decimal? EXP_PRICE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }
        public decimal? MOBA_AMOUNT { get;  set;  }

        public string IMP_MEDI_STOCK_NAME { get; set; }

        public string EXP_MEDI_STOCK_NAME { get; set; }

        public string EXP_MEST_TYPE_NAME { get; set; }

        public string EXP_MEST_TYPE_CODE { get; set; }

        public string IMP_MEST_TYPE_NAME { get; set; }

        public string IMP_MEST_TYPE_CODE { get; set; }

        public string EXP_MEDI_STOCK_CODE { get; set; }

        public string IMP_MEDI_STOCK_CODE { get; set; }

        public string IMP_MEDI_STOCK_CODE_NEW { get; set; }

        public string IMP_MEDI_STOCK_NAME_NEW { get; set; }
        public string EXPIRED_DATE_STR { get; set; }



        public string EXP_TIME_STR { get; set; }

        public long IMP_MEST_TYPE_ID { get; set; }

        public string SERVICE_NAME { get; set; }

        public string PATIENT_NAME { get; set; }

        public string PATIENT_CODE { get; set; }

        public decimal TOTAL_IMP_PRICE { get; set; }

        public string PATIENT_ADDRESS { get; set; }

        public string FEE_LOCK_TIME_STR { get; set; }

        public long STATUS { get; set; }

        public string REQUEST_USERNAME { get; set; }

        public string TREATMENT_CODE { get; set; }

        public decimal? MATERIAL_PRICE { get; set; }

        public decimal? MEDICINE_PRICE { get; set; }

        public string TDL_AGGR_EXP_MEST_CODE { get; set; }
        public decimal? PRICE_MEDI_MATE { set; get; }



        public decimal TOTAL_PRICE_MEDI_MATE { get; set; }

        public decimal TOTAL_AMOUNT { get; set; }

        public string TYPE { get; set; }

        public string PARENT_MEDI_MATE_TYPE_CODE { get; set; }

        public string PARENT_MEDI_MATE_TYPE_NAME { get; set; }

        public decimal EXPEND_AMOUNT { get; set; }

        public decimal TOTAL_EXPEND_PRICE { get; set; }

        public decimal RETURN_AMOUNT { get; set; }

        public decimal HPKP_AMOUNT { get; set; }

        public decimal RETURN_HPKP_AMOUNT { get; set; }

        public long TDL_TREATMENT_TYPE_ID { get; set; }

        public string PATIENT_DOB_STR { get; set; }

        public string ICD_CODE { get; set; }

        public string ICD_NAME { get; set; }

        public string TDL_INTRUCTION_TIME_STR { get; set; }
        public long IS_CABINET { get; set; }

        public string REQ_ROOM_NAME { get; set; }

        public string BED_NAME { get; set; }

        public string TREATMENT_TYPE_CODE { get; set; }

        public string TREATMENT_TYPE_NAME { get; set; }

        public decimal BHYT_EXP_AMOUNT { get; set; }

        public decimal BHYT_RETURN_AMOUNT { get; set; }

        public decimal VP_RETURN_AMOUNT { get; set; }

        public decimal FREE_RETURN_AMOUNT { get; set; }

        public decimal VP_EXP_AMOUNT { get; set; }

        public decimal FREE_EXP_AMOUNT { get; set; }

        public string TH_FOR_CABINET { get; set; }

        public string IMP_TIME_STR { get; set; }

        public string IMP_MEST_CODE { get; set; }

        public string NOT_DTT { get; set; }

        public string IS_CABINET_VALUE { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public string TDL_PATIENT_TYPE_NAME { get; set; }

        public long? TDL_EXP_MEST_REASON_ID { get; set; }

        public string TDL_EXP_MEST_REASON_NAME { get; set; }

        public string TDL_EXP_MEST_REASON_CODE { get; set; }

        public string OTHER_PAY_SOURCE_NAME { get; set; }

        public string OTHER_PAY_SOURCE_CODE { get; set; }

        public decimal TOTAL_REUSABLE_EXP { get; set; }
    }

    public abstract class AbstractCloneable : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class ParentService : HIS_SERVICE
    {
        public string CHILD_SERVICE_CODE { get; set; }
        
    }

    public class MATERIAL_REUSABLING
    {
        public long MATERIAL_ID { get; set; }
        public short IS_REUSABLING { get; set; }
    }
}
