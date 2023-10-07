using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00662
{
    class Mrs00662RDO : MOS.EFMODEL.DataModels.HIS_SERE_SERV
    {
        public long? SV_PTTT_GROUP_ID { get; set; }
        public long? PTTT_GROUP_ID { get; set; }
        public long? BEGIN_TIME { get; set; }
        public long? END_TIME { get; set; }
        public long? END_DATE { get; set; }
        public long? MALE_AGE { get; set; }
        public long? FEMALE_AGE { get; set; }
        public string END_TIME_STR { get; set; }
        public string END_DATE_STR { get; set; }
        public string BEGIN_TIME_STR { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_NAME { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_CODE { get; set; }
        public string TDL_EXECUTE_ROOM_NAME { get; set; }
        public string TDL_EXECUTE_ROOM_CODE { get; set; }
        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }
        public string TDL_REQUEST_DEPARTMENT_CODE { get; set; }
        public string TDL_REQUEST_ROOM_NAME { get; set; }
        public string TDL_REQUEST_ROOM_CODE { get; set; }
        public Dictionary<string, string> DICR_EXECUTE_USERNAME { get; set; }
        public Dictionary<string, string> DICR_EXECUTE_FIRSTNAME { get; set; }
        public string PTTT_PRIORITY_NAME { get; set; }
        public string PTTT_PRIORITY_CODE { get; set; }
        public string PTTT_TABLE_NAME { get; set; }
        public string EMOTIONLESS_RESULT_NAME { get; set; }
        public string SURG_PPVC_2 { get; set; }
        public string BEFORE_MISU { get; set; }
        public string AFTER_MISU { get; set; }
        public string MISU_PPPT { get; set; }
        public string REAL_MISU_PPPT { get; set; }
        public string MISU_PPVC { get; set; }
        public string MISU_TYPE_NAME { get; set; }
        public string DEFAULT_MISU_TYPE_NAME { get; set; }
        public string MISU_TYPE_CODE { get; set; }
        public string DEFAULT_MISU_TYPE_CODE { get; set; }
        public string EXECUTE_DOCTOR { get; set; }
        public string ANESTHESIA_DOCTOR { get; set; }
        public string IS_BHYT { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_FIRST_NAME { get; set; }
        public string TDL_PATIENT_LAST_NAME { get; set; }
        public long? TDL_PATIENT_CLASSIFY_ID { get; set; }
        public string PATIENT_CLASSIFY_NAME { get; set; }
        public MOS.EFMODEL.DataModels.V_HIS_PATIENT PATIENT { get; set; }
        public MOS.EFMODEL.DataModels.V_HIS_TREATMENT TREATMENT { get; set; }
        public short? FEE_TYPE { get; set; }

        public string IN_CODE { get; set; }
        public string PTTT_CATASTROPHE_NAME { get; set; }
        public string PTTT_CONDITION_NAME { get; set; }
        public string DEATH_WITHIN_NAME { get; set; }

        public string MANNER { get; set; }

        public string PRIMARY_PATIENT_TYPE_NAME { get; set; }

        public string PRIMARY_PATIENT_TYPE_CODE { get; set; }

        public long? PTTT_METHOD_ID { get; set; }

        public long? REAL_PTTT_METHOD_ID { get; set; }

        public long? PTTT_PRIORITY_ID { get; set; }

        public long? PTTT_TABLE_ID { get; set; }

        public long? EMOTIONLESS_RESULT_ID { get; set; }

        public long? EMOTIONLESS_METHOD_SECOND_ID { get; set; }

        public long? EMOTIONLESS_METHOD_ID { get; set; }

        public string BEFORE_PTTT_ICD_NAME { get; set; }

        public string AFTER_PTTT_ICD_NAME { get; set; }

        public long? PTTT_CATASTROPHE_ID { get; set; }

        public long? PTTT_CONDITION_ID { get; set; }

        public long? DEATH_WITHIN_ID { get; set; }

        public string SR_TREATMENT_TYPE_CODE { get; set; }

        public short IS_CC { get; set; }

        public string MAIN_EXECUTE { get; set; }
        public string EXTRA_EXECUTE { get; set; }
        public string HELPING { get; set; }
        public string JSON_PTTT_PRIORITY_AMOUNT { get; set; }
        public string JSON_PTTT_CATASTROPHE_AMOUNT { get; set; }
        public string JSON_DEATH_WITHIN_AMOUNT { get; set; }

        public string IS_PT { get; set; }
        public string IS_TT { get; set; }
        public string PT_TT_NAME { get; set; }


        public decimal MEDICINE_HPKP { get; set; }

        public decimal MEDICINE_CLIPPING { get; set; }

        public decimal MATERIAL_HPKP { get; set; }

        public decimal MATERIAL_CLIPPING { get; set; }

        public long SERE_SERV_ID { get; set; }

        public long? CHILD_TREATMENT_ID { get; set; }

        public string IS_MAIN_PTTT { get; set; }

        public decimal MEDICINE_HP_PRICE { get; set; }
        public decimal MATERIAL_HP_PRICE { get; set; }
        public decimal MEDICINE_DK_PRICE { get; set; }
        public decimal MATERIAL_DK_PRICE { get; set; }

        public long? TDL_SERVICE_REQ_ID { get; set; }

        public string DEBATE_TYPE_CODE { get; set; }

        public string DEBATE_TYPE_NAME { get; set; }

        public long SERVICE_REQ_STT_ID { get; set; }
        public string NOTE { get; set; }
        public string NOTICE { get; set; }
        public long INTRUCTION_TIME { get; set; }

        public string INTRUCTION_TIME_STR { get; set; }

        public string MALE_BIRTH { get; set; }

        public string FEMALE_BIRTH { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string ICD_TEXT { get; set; }

        public string TYPE_I { get; set; }

        public string TYPE_II { get; set; }

        public string TYPE_III { get; set; }

        public string TYPE_DB { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public string TDL_TREATMENT_TYPE_CODE { get; set; }
        public string TDL_TREATMENT_TYPE_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get;  set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }

    }
    public class Mrs00662RDOPTTM : HIS_SERE_SERV
    {
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long? TEST_COVID_TYPE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal TEST_CV_PRICE { get; set; }
        public decimal MAIN_PT_PRICE { get; set; }
        public decimal PTYC_PRICE { get; set; }
        public decimal CDHA_EXPEND_PRICE { get; set; }
        public decimal DEPARTMENT_EXPEND_PRICE { get; set; }
        public decimal XN_EXPEND_PRICE { get; set; }
        public decimal KHAC_EXPEND_PRICE { get; set; }

        public decimal HOICHAN_PRICE { get; set; }
        public decimal HOICHAN_HEIN_PRICE { get; set; }

        public decimal EXAM_PRICE1 { get; set; }
        public decimal EXAM_HEIN_PRICE1 { get; set; }

        public decimal PTYC_PRICE1 { get; set; }
        public decimal PTYC_HEIN_PRICE1 { get; set; }
        public string NOTE { get; set; }

        public string DOCTOR { get; set; }
        public string TDL_PATIENT_WORK_PLACE { get; set; }

        public decimal HOICHAN_PRICE_KH { get; set; }

        public decimal HOICHAN_HEIN_PRICE_KH { get; set; }

        public decimal HOICHAN_PRICE_KHAC { get; set; }
        public decimal HOICHAN_HEIN_PRICE_KHAC { get; set; }

        public decimal BLOOD_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SERVICE_CODE { get; set; }

        public decimal OTHER_PRICE { get; set; }

        public string SERVICE_TYPE_CODE { get; set; }

        public string REQUEST_LOGINNAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }

        public Dictionary<string, decimal> DIC_SVT_CODE { get; set; }
        public Dictionary<string, decimal> DIC_HEIN_SVT_CODE { get; set; }

        public decimal EXAM_HEIN_PRICE { get; set; }
        public decimal BED_HEIN_PRICE { get; set; }
        public decimal MEDICINE_HEIN_PRICE { get; set; }
        public decimal MATERIAL_HEIN_PRICE { get; set; }
        public decimal TEST_HEIN_PRICE { get; set; }
        public decimal DIIM_HEIN_PRICE { get; set; }
        public decimal TEST_CV_HEIN_PRICE { get; set; }
        public decimal MAIN_PT_HEIN_PRICE { get; set; }
        public decimal PTYC_HEIN_PRICE { get; set; }
    }
}
