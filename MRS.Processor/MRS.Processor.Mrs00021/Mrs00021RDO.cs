using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00021
{
    class Mrs00021RDO
    {
        public MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL HEIN_APPROVAL { get; set; }

        public long PATIENT_ID { get; set; }
        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long? TOTAL_DATE { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }

        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string MEDIORG_CODE { get; set; }
        public string MEDIORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string ICD_SUB_CODE { get; set; }
        public string ICD_CODE_EXTRA { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string FIRST_CHAR_CARD { get; set; }
        public string HEIN_TREATMENT_TYPE_CODE { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal FUEX_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO_UT { get; set; }
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal MATERIAL_PRICE_RATIO_TT { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TT_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public decimal EXAM_PRICE { get; set; }

        //TheoThoi gian vao
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string HEIN_CARD_FROM_TIME_STR { get; set; }
        public string HEIN_CARD_TO_TIME_STR { get; set; }
        public string HEIN_ADDRESS { get; set; }
        public long? MAIN_DAY { get; set; }

        public long END_DEPARTMENT_ID { get; set; }
        public int COUNT_TREATMENT { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string END_DEPARTMENT_CODE { get; set; }
        public string END_ROOM_DEPARTMENT_NAME { get; set; }
        public string END_ROOM_DEPARTMENT_CODE { get; set; }
        public long TREATMENT_ID { get; set; }
        public string SENDER_HEIN_MEDI_ORG_CODE { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public string PATIENT_CASE_NAME { get; set; }
        public long? END_ROOM_ID { get; set; }
        public string END_ROOM_NAME { get; set; }
        public string END_ROOM_CODE { get; set; }
        public string END_USERNAME { set; get; }
        public string END_LOGINNAME { set; get; }

        public long ACCUM_TREATMENT { get; set; }
        public string HOSPITALIZATION_REASON { get; set; }
        public string TDL_HEIN_MEDI_ORG_NAME { get; set; }

        public Mrs00021RDO() { }

        public Mrs00021RDO(MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval)
        {
            this.HEIN_APPROVAL = heinApproval;
            this.HEIN_CARD_NUMBER = heinApproval.HEIN_CARD_NUMBER;
            this.FIRST_CHAR_CARD = (heinApproval.HEIN_CARD_NUMBER ?? " ").Substring(0, 1);
            this.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
            this.TREATMENT_ID = heinApproval.TREATMENT_ID;
            this.PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
            this.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME.TrimEnd(' ');
            this.MEDIORG_NAME = heinApproval.HEIN_MEDI_ORG_NAME;
            this.MEDIORG_CODE = heinApproval.HEIN_MEDI_ORG_CODE;
            this.RIGHT_ROUTE_CODE = heinApproval.RIGHT_ROUTE_CODE;
            if (heinApproval.RIGHT_ROUTE_CODE == "DT")
            {   
                this.RIGHT_ROUTE_NAME = "Đúng tuyến";
            }
            if (heinApproval.RIGHT_ROUTE_CODE == "TT")
            {
                this.RIGHT_ROUTE_NAME = "Trái tuyến";
            }
            this.SetExtendField();
        }

        public void SetExtendField()
        {
            if (this.HEIN_APPROVAL.TDL_PATIENT_DOB > 0)
            {
                try
                {
                    this.DOB = Inventec.Common.TypeConvert.Parse.ToInt64(HEIN_APPROVAL.TDL_PATIENT_DOB.ToString().Substring(0, 8));
                    if (this.HEIN_APPROVAL.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        this.DOB_MALE = long.Parse((this.HEIN_APPROVAL.TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                    else if (this.HEIN_APPROVAL.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.DOB_FEMALE = long.Parse((this.HEIN_APPROVAL.TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }
        }


        public string REASON_INPUT_CODE { get; set; }

        public long DOB { get; set; }

        public string DEPARTMENT_CODE { get; set; }

        public string DEPARTMENT_NAME { get; set; }

        public string REQUEST_DEPARTMENT_CODE { get; set; }

        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public long INSURANCE_YEAR { get; set; }

        public long INSURANCE_MONTH { get; set; }

        public string CURRENT_MEDI_ORG_CODE { get; set; }

        public object[] END_ROOM_DEPARTMENT_ID { get; set; }

        public object EXECUTE_DATE { get; set; }

        public decimal? MEDICINE_PRICE_NDM { get; set; }

        public decimal? TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public decimal? HEIN_RATIO { get; set; }
        public string RIGHT_ROUTE_CODE { get; set; }
        public string RIGHT_ROUTE_NAME { get; set; }
    }
}
