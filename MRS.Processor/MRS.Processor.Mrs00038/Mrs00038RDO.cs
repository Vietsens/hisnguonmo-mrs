using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00038
{
    public class Mrs00038RDO
    {
        public string IN_CODE { get; set; }
        public string OUT_CODE { get; set; }
        public string STORE_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string EXAM_END_TYPE_CODE { get; set; }
        public string EXAM_END_TYPE_NAME { get; set; }
        public string PATIENT_CODE { get; set; }//mã bệnh nhân
        public string PATIENT_NAME { get; set; }//tên bệnh nhân
        public string PATIENT_SDT { get; set; }//số điện thoại bệnh nhân
        public string DOB { get; set; }// năm sinh
        public string JOB { get; set; }//nghề nghiệp
        public string GENDER { get; set; }//giới tính
        public string ETHNIC_NAME { get; set; }//dân tộc
        public string NATIONAL { get; set; }//dân tộc
        public string VIR_ADDRESS { get; set; }
        public string GIOITHIEU { get; set; }
        public string DATE_IN_STR { get; set; }
        public string TIME_IN_STR { get; set; }
        public long IN_TIME { get; set; }
        public string DATE_TRIP_STR { get; set; }
        public string DATE_OUT_STR { get; set; }
        public string DATE_DEAD_STR { get; set; }
        public string INTRUCTION_TIME_STR { get; set; }
        public string DIAGNOSE_TUYENDUOI { get; set; }
        public string DIAGNOSE_KKB { get; set; }
        public string DIAGNOSE_KDT { get; set; }
        public string DIAGNOSE_KGBP { get; set; }

        public string ICD_CODE_TUYENDUOI { get; set; }
        public string ICD_NAME_TUYENDUOI { get; set; }
        public string ICD_CODE_KKB { get; set; }
        public string ICD_NAME_KKB { get; set; }
        public string ICD_CODE_KDT { get; set; }
        public string ICD_CODE_KGBP { get; set; }

        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public decimal? MALE_AGE { get; set; }
        public decimal? FEMALE_AGE { get; set; }
        public decimal? AGE { get; set; }// tuổi
        public decimal TOTAL_DATE_TREATMENT { get; set; }
        public decimal TREATMENT_DAY_COUNT { get; set; }
        public string DOB_STR { get; set; }

        public string IS_OFFICER { get; set; }
        public string IS_BHYT { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }//mã bhyt
        public string IS_CITY { get; set; }
        public string IS_COUNTRYSIDE { get; set; }
        public string IS_DUOI_12THANG { get; set; }
        public string IS_1DEN15TUOI { get; set; }
        public string IS_DEAD_IN_24H { get; set; }
        public string IS_CURED { get; set; }
        public string IS_ABATEMENT { get; set; }
        public string IS_AGGRAVATION { get; set; }
        public string IS_UNCHANGED { get; set; }
        public string TIME_TRIP_STR { get; set; }
        public string TIME_OUT_STR { get; set; }
        public string TIME_DEAD_STR { get; set; }
        public string IS_CAPCUU { get; set; }


        public long START_DEPARTMENT_ID { get; set; }
        public string START_DEPARTMENT_NAME { get; set; }

        public long FX_IN_DEPARTMENT_ID { get; set; }
        public string FX_IN_DEPARTMENT_NAME { get; set; }

        public long RX_IN_DEPARTMENT_ID { get; set; }
        public string RX_IN_DEPARTMENT_NAME { get; set; }

        public long END_DEPARTMENT_ID { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }

        public long END_ROOM_ID { get; set; }
        public string END_ROOM_NAME { get; set; }

        public string DEPARTMENT_TRAN_TIME_STR { get; set; }

        public string PHONE { get; set; }

        public string TDL_PATIENT_RELATIVE_PHONE { get; set; }

        public string RELATIVE { get; set; }

        public string WORK_PLACE_NAME { get; set; }

        public string PATIENT_CASE_NAME { get; set; }
        public string RIGHT_ROUTE_TYPE_CODE { get; set; }
        public string RIGHT_ROUTE_CODE { get; set; }

        public string EXAM_ROOM_CODE { get; set; }
        public string EXAM_ROOM_NAME { get; set; }//phòng khám
        public string EXECUTE_ROOM_CODE { set; get; }//phòng thực hiện
        public string EXECUTE_ROOM_NAME { set; get; }

        public string REQUEST_LOGINNAME { get; set; }//
        public string REQUEST_USERNAME { get; set; }//bác sý chỉ định
        public string EXECUTE_LOGINNAME { set; get; }// bác sỹ thực hiện
        public string EXECUTE_USERNAME { set; get; }

        public string CREATOR { get; set; }// người tiếp đón
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }// đối tượng
        public string ROUTE_CODE { get; set; }
        public string ROUTE_NAME { get; set; }

        public string ICD_SUB_CODE { get; set; }

        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }

        public string EXECUTE_DEPARTMENT_CODE { get; set; }

        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public long TREATMENT_TYPE_ID { set; get; }
        public string TREATMENT_TYPE_NAME { set; get; }
        public string TREATMENT_TYPE_CODE { set; get; }

        public string DEPARTMENT_IN_TIME_STR { get; set; }

        public string DEPARTMENT_TRAN_NAME { get; set; }
        public string DEPARTMENT_TRAN_CODE { get; set; }

        public string BED_ROOM_CODE { get; set; }

        public string BED_ROOM_NAME { get; set; }

        public string TDL_PATIENT_CAREER_NAME { get; set; }
        public string MALE_YEAR { get; set; }

        public string FEMALE_YEAR { get; set; }

        public string IS_EMERGENCY { get; set; }

        public long FX_IN_ROOM_ID { get; set; }

        public string FX_IN_ROOM_NAME { get; set; }

        public long? CLINICAL_IN_TIME { get; set; }

        public decimal? TOTAL_PRICE { get; set; }

        public decimal? TOTAL_HEIN_PRICE { get; set; }
    }
    public class PATIENT_TREATMENT_EXAMREQ
    {
        public short? IS_EMERGENCY { get; set; }

        public long? TREATMENT_ID { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }

        public long? REQUEST_ROOM_ID { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }

        public string REQUEST_LOGINNAME { get; set; }

        public string REQUEST_USERNAME { get; set; }

        public string EXECUTE_LOGINNAME { get; set; }

        public string EXECUTE_USERNAME { get; set; }

        public long? INTRUCTION_TIME { get; set; }

        public long? ID { get; set; }

        public string ICD_CAUSE_CODE { get; set; }

        public string ICD_CAUSE_NAME { get; set; }

        public long IN_TIME { get; set; }

        public string TREATMENT_CREATOR { get; set; }

        public long? TREATMENT_END_TYPE_ID { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public string ICD_TEXT { get; set; }

        public string ICD_SUB_CODE { get; set; }

        public string TDL_PATIENT_CAREER_NAME { get; set; }

        public long? TDL_PATIENT_DOB { get; set; }

        public string TDL_PATIENT_ADDRESS { get; set; }

        public string TDL_PATIENT_COMMUNE_CODE { get; set; }

        public string TDL_PATIENT_CODE { get; set; }

        public string TDL_PATIENT_GENDER_NAME { get; set; }

        public string TDL_PATIENT_PHONE { get; set; }

        public string TDL_PATIENT_RELATIVE_PHONE { get; set; }
        

        public long? PATIENT_ID { get; set; }

        public string ETHNIC_NAME { get; set; }

        public string NATIONAL_NAME { get; set; }

        public string RELATIVE_TYPE { get; set; }

        public string RELATIVE_NAME { get; set; }

        public long? WORK_PLACE_ID { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public string TREATMENT_CODE { get; set; }

        public string STORE_CODE { get; set; }

        public string IN_CODE { get; set; }

        public string OUT_CODE { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }

        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }

        public string TRANSFER_IN_ICD_NAME { get; set; }

        public string TRANSFER_IN_ICD_CODE { get; set; }

        public string TRANSFER_IN_MEDI_ORG_NAME { get; set; }

        public long? TDL_PATIENT_GENDER_ID { get; set; }

        public long? TREATMENT_RESULT_ID { get; set; }

        public long? OUT_TIME { get; set; }

        public long? DEATH_WITHIN_ID { get; set; }

        public long? END_DEPARTMENT_ID { get; set; }

        public long? LAST_DEPARTMENT_ID { get; set; }

        public long? END_ROOM_ID { get; set; }

        public long? CLINICAL_IN_TIME { get; set; }

        public long? PATIENT_CASE_ID { get; set; }

        public string ICD_NAME { get; set; }

        public string ICD_CODE { get; set; }

        public string IN_ICD_NAME { get; set; }

        public string IN_ICD_CODE { get; set; }

        public long? IN_DEPARTMENT_ID { get; set; }
        public long? HOSPITALIZE_DEPARTMENT_ID { get; set; }

        public decimal? TREATMENT_DAY_COUNT { get; set; }

        public long? IN_ROOM_ID { get; set; }

        public decimal? TOTAL_PRICE { get; set; }

        public decimal? TOTAL_HEIN_PRICE { get; set; }
    }
}
