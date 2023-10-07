using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00255
{
    public class Mrs00255RDO
    {
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string HAS_PARRENT { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal VIR_PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public decimal MEDICINE_VIR_TOTAL_PRICE { get; set; }
        public decimal MATERIAL_VIR_TOTAL_PRICE { get; set; }
        public decimal MEDICINE_FEE_VIR_TOTAL_PRICE { get; set; }
        public decimal MATERIAL_FEE_VIR_TOTAL_PRICE { get; set; }
        public string BEFORE_PTTT_ICD_NAME { get; set; }
        public string AFTER_PTTT_ICD_NAME { get; set; }
        public string PTTT_METHOL_NAME { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
        public string SV_PTTT_GROUP_NAME { get; set; }
        public string PTTT_GROUP_CODE { get; set; }
        public string SV_PTTT_GROUP_CODE { get; set; }
        public string START_TIME { get; set; }
        public long SURG_TIME_TOTAL { get; set; }
        public string TIME_TOTAL_STR { get; set; }
        public string EXECUTE_ROLE_MAIN { get; set; }
        public string EXECUTE_ROLE_MAIN_LOGINNAME { get; set; }
        public string EXECUTE_ROLE_ANESTHETIST { get; set; }
        public string EXECUTE_ROLE_PM1 { get; set; }
        public string EXECUTE_ROLE_PM2 { get; set; }
        public string EXECUTE_ROLE_PMe1 { get; set; }
        public string EXECUTE_ROLE_PMe2 { get; set; }
        public string EXECUTE_ROLE_NURSE { get; set; }
        public string EXECUTE_ROLE_DCVPTTT { get; set; }
        public string NOTE { get; set; }

        public string PATIENT_CODE { get; set; }
        public string IN_CODE { get; set; }
        public string TREATMENT_PATIENT_TYPE_NAME { get; set; }
        public decimal BHYT_RATIO { get; set; }
        public string INTRUCTION_TIME { get; set; }
        public string BEGIN_TIME { get; set; }
        public string END_TIME { get; set; }
        public string IN_TIME { get; set; }
        public string OUT_TIME { get; set; }
        public long PTTT_NUM_ORDER { get; set; }
        public long? EKIP_ID { get; set; }
        public long AGE { get; set; }

        public string DEATH_WITHIN_NAME { get; set; }
        public string EMOTIONLESS_METHOD_NAME { get; set; }
        public string PTTT_CATASTROPHE_NAME { get; set; }
        public string PTTT_CONDITION_NAME { get; set; }
        public string REAL_PTTT_METHOD_NAME { get; set; }

        public long? TDL_TREATMENT_ID { get; set; }

        public long? ID { get; set; }

        public long? REQUEST_DEPARTMENT_ID { get; set; }

        public long? REQUEST_ROOM_ID { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }

        public long? PARENT_ID { get; set; }

        public long? PTTT_METHOD_ID { get; set; }

        public long? PTTT_GROUP_ID { get; set; }

        public long? SV_PTTT_GROUP_ID { get; set; }

        public long? DEATH_WITHIN_ID { get; set; }

        public long? EMOTIONLESS_METHOD_ID { get; set; }

        public long? PTTT_CATASTROPHE_ID { get; set; }

        public long? PTTT_CONDITION_ID { get; set; }

        public long? REAL_PTTT_METHOD_ID { get; set; }

        public long? EMOTIONLESS_METHOD_SECOND_ID { get; set; }

        public long SERVICE_REQ_ID { get; set; }

        public long? L_START_TIME { get; set; }

        public long? L_FINISH_TIME { get; set; }

        public decimal ORIGINAL_PRICE { get; set; }

        public decimal? PRIMARY_PRICE { get; set; }

        public decimal PRICE { get; set; }

        public decimal VAT_RATIO { get; set; }

        public decimal? HEIN_LIMIT_PRICE { get; set; }

        public long? DOB { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public long? L_BEGIN_TIME { get; set; }

        public long? L_END_TIME { get; set; }

        public long? L_IN { get; set; }

        public long? L_OUT { get; set; }

        public long? L_INTRUCTION { get; set; }

        public Dictionary<string, string> DICR_EXECUTE_USERNAME { get; set; }

        public string CATEGORY_CODE { get; set; }

        public string CATEGORY_NAME { get; set; }
        public long? PTTT_PRIORITY_ID { get; set; }
        public string PTTT_PRIORITY_CODE { get; set; }
        public string PTTT_PRIORITY_NAME { set; get; }

        public string EXECUTE_ROLE_CODEs { get; set; }

        public string USERNAMEs { get; set; }

        public int IS_EKIP_AGAIN { get; set; }
        public decimal REMUNERATION_PRICE { get; set; }

        public string KEY_ORDER { get; set; }
    }
    public class MEMA_FOLLOW
    {
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string MEMA_CODE { get; set; }
        public string MEMA_NAME { get; set; }
        public string MEMA_UNIT_NAME { get; set; }
        public string MEMA_UNIT_CODE { get; set; }
        public string MEMA_TYPE { get; set; }
        public decimal MEMA_AMOUNT { get; set; }
        public string PARENT_IDs { get; set; }
        public string TREATMENT_CODE { get; set; }

        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string MAIN_USERNAME { get; set; }
        public string MAIN_LOGINNAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal VIR_PRICE_NO_EXPEND { get; set; }

        public string PATIENT_NAME { get; set; }

        public string MEMA_REQUEST_LOGINNAME { get; set; }

        public string MEMA_REQUEST_USERNAME { get; set; }
        public long PARENT_ID { get; set; }
    }
    public class PTTT_COUNT {
        //phẫu thuật
        public decimal TOTAL_PT { get; set; }
        public decimal PT_TYPE_0_KH { get; set; }
        public decimal PT_TYPE_1_KH { get; set; }
        public decimal PT_TYPE_2_KH { get; set; }
        public decimal PT_TYPE_3_KH { get; set; }
        public decimal PT_TYPE_DB_KH { get; set; }

        public decimal PT_TYPE_0_CC { get; set; }
        public decimal PT_TYPE_1_CC { get; set; }
        public decimal PT_TYPE_2_CC { get; set; }
        public decimal PT_TYPE_3_CC { get; set; }
        public decimal PT_TYPE_DB_CC { get; set; }

        public decimal PT_TYPE_0_GMHS { get; set; }
        public decimal PT_TYPE_1_GMSH { get; set; }
        public decimal PT_TYPE_2_GMHS { get; set; }
        public decimal PT_TYPE_3_GMHS { get; set; }
        public decimal PT_TYPE_DB_GMHS { get; set; }

        public decimal PT_TYPE_0_NK { get; set; }
        public decimal PT_TYPE_1_NK { get; set; }
        public decimal PT_TYPE_2_NK { get; set; }
        public decimal PT_TYPE_3_NK { get; set; }
        public decimal PT_TYPE_DB_NK { get; set; }

        public decimal PT_TYPE_0_TB { get; set; }
        public decimal PT_TYPE_1_TB { get; set; }
        public decimal PT_TYPE_2_TB { get; set; }
        public decimal PT_TYPE_3_TB { get; set; }
        public decimal PT_TYPE_DB_TB { get; set; }

        public decimal PT_TYPE_0_DIE_24 { get; set; }
        public decimal PT_TYPE_1_DIE_24 { get; set; }
        public decimal PT_TYPE_2_DIE_24 { get; set; }
        public decimal PT_TYPE_3_DIE_24 { get; set; }
        public decimal PT_TYPE_DB_DIE_24 { get; set; }

        public decimal PT_TYPE_0_DIE { get; set; }
        public decimal PT_TYPE_1_DIE { get; set; }
        public decimal PT_TYPE_2_DIE{ get; set; }
        public decimal PT_TYPE_3_DIE { get; set; }
        public decimal PT_TYPE_DB_DIE { get; set; }
        //thủ thuật
        public decimal TOTAL_TT { get; set; }
        public decimal TT_TYPE_0_KH { get; set; }
        public decimal TT_TYPE_1_KH { get; set; }
        public decimal TT_TYPE_2_KH { get; set; }
        public decimal TT_TYPE_3_KH { get; set; }
        public decimal TT_TYPE_DB_KH { get; set; }
                       
        public decimal TT_TYPE_0_CC { get; set; }
        public decimal TT_TYPE_1_CC { get; set; }
        public decimal TT_TYPE_2_CC { get; set; }
        public decimal TT_TYPE_3_CC { get; set; }
        public decimal TT_TYPE_DB_CC { get; set; }
                       
        public decimal TT_TYPE_0_GMHS { get; set; }
        public decimal TT_TYPE_1_GMSH { get; set; }
        public decimal TT_TYPE_2_GMHS { get; set; }
        public decimal TT_TYPE_3_GMHS { get; set; }
        public decimal TT_TYPE_DB_GMHS { get; set; }
                       
        public decimal TT_TYPE_0_NK { get; set; }
        public decimal TT_TYPE_1_NK { get; set; }
        public decimal TT_TYPE_2_NK { get; set; }
        public decimal TT_TYPE_3_NK { get; set; }
        public decimal TT_TYPE_DB_NK { get; set; }
                       
        public decimal TT_TYPE_0_TB { get; set; }
        public decimal TT_TYPE_1_TB { get; set; }
        public decimal TT_TYPE_2_TB { get; set; }
        public decimal TT_TYPE_3_TB { get; set; }
        public decimal TT_TYPE_DB_TB { get; set; }
                       
        public decimal TT_TYPE_0_DIE_24 { get; set; }
        public decimal TT_TYPE_1_DIE_24 { get; set; }
        public decimal TT_TYPE_2_DIE_24 { get; set; }
        public decimal TT_TYPE_3_DIE_24 { get; set; }
        public decimal TT_TYPE_DB_DIE_24 { get; set; }
                       
        public decimal TT_TYPE_0_DIE { get; set; }
        public decimal TT_TYPE_1_DIE { get; set; }
        public decimal TT_TYPE_2_DIE { get; set; }
        public decimal TT_TYPE_3_DIE { get; set; }
        public decimal TT_TYPE_DB_DIE { get; set; }  
    }
}
