using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00042
{
    public class Mrs00042RDO
    {
        public long SERVICE_ID { get; set; }
        public long SERVICE_REQ_ID { get; set; }
        public long? EKIP_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public Decimal? PRICE { get; set; }//1
        public decimal? VIR_TOTAL_PRICE { get; set; }//1
        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }//1
        public long? TDL_PATIENT_DOB { get; set; }//2
        public string REQUEST_USERNAME { get; set; }//2
        public string REQUEST_LOGINNAME { get; set; }//2
        public long? START_TIME { get; set; }
        public string INTRUCTION_TIME { get; set; } //3
        public long? INTRUCTION_TIME_1 { get; set; } //3
        public long INTRUCTION_TIME_NUM { get; set; } //3
        public string FINISH_TIME_STR { get; set; }//4
        public long? FINISH_TIME { get; set; }//4
        public long? TREATMENT_ID { get; set; }//5
        public string TREATMENT_CODE { get; set; }//5
        public long IN_TIME { get; set; }//5
        public long? OUT_TIME { get; set; }//5
        public string STORE_CODE { get; set; }//so luu tru//6
        public string IN_CODE { get; set; }//so vo vien//7
        public string HEIN_CARD_NUMBER { get; set; }//8
        public string EXECUTE_ROOM { get; set; }//9
        public string PATIENT_NAME { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string IS_BHYT { get; set; }
        public string ICD_DIIM { get; set; }
        public string REQUEST_ROOM { get; set; }
        public string SERVICE_NAME { get; set; }
        public string SERVICE_CODE { get; set; }
        public string DIIM_RESULT { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public string IS_SIZE_FILM_13_18 { get; set; }
        public string IS_SIZE_FILM_18_24 { get; set; }
        public string IS_SIZE_FILM_24_30 { get; set; }
        public string IS_SIZE_FILM_30_40 { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }//đối tượng thanh toán
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }//đối tượng bệnh nhân
        public string PATIENT_TYPE_NAME_01 { get; set; }
        public string PATIENT_TYPE_NAME_02 { get; set; }
        public string TIME_DIIM_STR { get; set; }
        public string FEMALE_YEAR { get; set; }
        public string MALE_YEAR { get; set; }
        public string SUBCLINICAL_PRES_USERNAME {set;get;}
        public decimal? MALE_AGE { get; set; }
        public decimal? FEMALE_AGE { get; set; }

        public string MEDI_ORG_CODE { get; set; }
        

        public string IS_FILM_SIZE_1 { get; set; }
        public string IS_FILM_SIZE_2 { get; set; }
        public string IS_FILM_SIZE_3 { get; set; }
        public string IS_FILM_SIZE_4 { get; set; }
        public string IS_FILM_SIZE_5 { get; set; }
        public string IS_FILM_SIZE_6 { get; set; }
        public string IS_FILM_SIZE_7 { get; set; }
        public string IS_FILM_SIZE_8 { get; set; }
        public string IS_FILM_SIZE_9 { get; set; }
        public string IS_FILM_SIZE_10 { get; set; }

        public string IS_FILM_SIZE_11 { get; set; }
        public string IS_FILM_SIZE_12 { get; set; }
        public string IS_FILM_SIZE_13 { get; set; }
        public string IS_FILM_SIZE_14 { get; set; }
        public string IS_FILM_SIZE_15 { get; set; }
        public string IS_FILM_SIZE_16 { get; set; }
        public string IS_FILM_SIZE_17 { get; set; }
        public string IS_FILM_SIZE_18 { get; set; }
        public string IS_FILM_SIZE_19 { get; set; }
        public string IS_FILM_SIZE_20 { get; set; }

        public long SS_ID { get; set; }
        public string DIIM_TYPE_NAME { get; set; }
        public decimal AMOUNT { get; set; }
        public decimal AMOUNT_TT { get; set; }
        public decimal AMOUNT_TTS { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public long? BEGIN_TIME { get; set; }
        public long? MACHINE_ID { get; set; }
        public long? END_TIME { get; set; }
        public string DE_AREA { get; set; }
        public decimal? NUMBER_OF_FILM { get; set; }

        public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }
        public string TDL_HEIN_SERVICE_BHYT_NAME { get;  set; }
        public string TDL_HEIN_SERVICE_BHYT_CODE { get;  set; }
        public string TDL_HEIN_CARD_NUMBER { get;  set; }
        public long? TDL_PATIENT_ID { get;  set; }
        public long? REQUEST_ROOM_ID { get;  set; }
        public long? EXECUTE_ROOM_ID { get;  set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public string ICD_NAME { get;  set; }
        public string ICD_TEXT { get;  set; }
        public long? TDL_PATIENT_GENDER_ID { get;  set; }
        public long? FILM_SIZE_ID { get;  set; }
        public long? PARENT_SERVICE_ID { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string PTTT_GROUP_NAME { set; get; }

        public string MACHINE_CODE { get; set; }
        public string MACHINE_NAME { get; set; }
        public string EXECUTE_MACHINE_NAME { get; set; }
        public string EXECUTE_MACHINE_CODE { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }

        public string EXECUTE_ROLE_CODE { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }

        public string EXECUTE_DATE_STR { get; set; }
        public long? EXECUTE_TIME { get; set; }

        public string DEBATE_TYPE_NAME { get; set; }
        public long INTRUCTION_DATE_NUM { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_SUB_CODE { get; set; }
        
        public Dictionary<string, string> DIC_EXECUTE_USERNAME { get; set; }

        public string ICD_DIIM_CODE { get; set; }

        public decimal IS_WEEK_DAY { get; set; }

        public decimal IS_HOLIDAY { get; set; }

        public string TIME_NUM { get; set; }

        public decimal CHECK_TIME { get; set; }

        public DateTime? INTRUCTION_TIME_DT { get; set; }

        public string END_TIME_STR { get; set; }

        public decimal MEDICINE_PRICE_EXPEND { get; set; }

        public decimal MATERIAL_PRICE_EXPEND { get; set; }

        public decimal MEDICINE_PRICE { get; set; }

        public decimal MATERIAL_PRICE { get; set; }

        public string MEDICINE_EXPENDs { get; set; }

        public string MATERIAL_EXPENDs { get; set; }

        public string MEDICINEs { get; set; }

        public string MATERIALs { get; set; }
    }

    public class REQUEST_FILM_SIZE
    {
        public long SERVICE_REQ_ID { get; set; }
        public long FILM_SIZE_ID { get; set; }
    }

    public class MEDI_MATE_EXPEND : Mrs00042RDO
    {
        public string MEDICINE_TYPE_CODE { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public decimal? MEDICINE_AMOUNT { get; set; }
        public decimal? MEDICINE_PRICE { get; set; }
        public decimal? MEDICINE_TOTAL_PRICE { get; set; }

        public string MATERIAL_TYPE_CODE { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public decimal? MATERIAL_AMOUNT { get; set; }
        public decimal? MATERIAL_PRICE { get; set; }
        public decimal? MATERIAL_TOTAL_PRICE { get; set; }

        public string FILM_CODE { get; set; }
        public string FILM_NAME { get; set; }
        public decimal? FILM_AMOUNT { get; set; }
        public decimal? FILM_PRICE { get; set; }
        public decimal? FILM_TOTAL_PRICE { get; set; }
    }

    public class DEBATE : HIS_DEBATE_TYPE
    {
        public long DEPARTMENT_ID { get; set; }
        public long TREATMENT_ID { get; set; }
    }
}
