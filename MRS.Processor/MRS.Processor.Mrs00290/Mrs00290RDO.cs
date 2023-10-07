using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00290
{
    public class Mrs00290RDO : HIS_SERE_SERV
    {
        public long? BRANCH_ID { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? FEE_LOCK_TIME { get; set; }
        public long? START_TIME { get; set; }
        public long REPORT_TIME { get; set; }
        public long? REPORT_DATE { get; set; }
        public long? INVOICE_NUM_ORDER { get; set; }
        public string EINVOICE_NUM_ORDER { get; set; }
        public decimal TOTAL_BILL_PRICE { get; set; }

        //các thông tin hồ sơ điều trị bổ sung
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public string TREA_ICD_CODE { get; set; }
        public string TREA_ICD_NAME { get; set; }
        public string LAST_DEPARTMENT_CODE { get; set; }
        public string LAST_DEPARTMENT_NAME { get; set; }
        public string FIRST_DEPARTMENT_CODE { get; set; }
        public string FIRST_DEPARTMENT_NAME { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_RESULT_NAME { get; set; }
        public decimal? TREATMENT_DAY_COUNT { get; set; }
        public string TDL_PATIENT_CAREER_NAME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }

        public string TEST_SAMPLE_TYPE_NAME { get; set; }


        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string DOB { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string SERVICE_NAME { get; set; }
        public string ICD_TEXT { get; set; }
        public string REQ_USERNAME { get; set; }
        public string REQ_LOGINNAME { get; set; }
        public string INSTRUCTION_TIME { get; set; }
        public string ROOM_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string WORK_PLACE_NAME { get; set; }
        public string ADDRESS { get; set; }
        public string TDL_PATIENT_PHONE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }
        public string START_TIME_STR { get; set; }
        public string TDL_PATIENT_NATIONAL_NAME { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public decimal PAY_RATE_RATIO { get; set; }
        public short ROUTE_TYPE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long COUNT_TREATMENT { get; set; }
        public long COUNT_REQ { get; set; }

        public string ACCEPT_HEIN_MEDI_ORG_CODE { get; set; }

        public string TREA_HEIN_MEDI_ORG_CODE { get; set; }

        public string BRANCH_HEIN_PROVINCE_CODE { get; set; }

        public Dictionary<string, decimal> DIC_SVT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_SV_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SVT_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_CATE_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_PAR_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_AMOUNT { get; set; }

        public Dictionary<string, decimal> DIC_SV_AMOUNT { get; set; }

        public decimal AMOUNT_1 { get; set; }

        public decimal AMOUNT_2 { get; set; }

        public decimal AMOUNT_3 { get; set; }

        public decimal AMOUNT_4 { get; set; }

        public decimal AMOUNT_5 { get; set; }

        public decimal AMOUNT_6 { get; set; }

        public decimal AMOUNT_7 { get; set; }

        public decimal AMOUNT_8 { get; set; }

        public decimal AMOUNT_9 { get; set; }

        public decimal AMOUNT_10 { get; set; }

        public decimal AMOUNT_11 { get; set; }

        public decimal AMOUNT_12 { get; set; }

        public decimal AMOUNT_13 { get; set; }

        public decimal AMOUNT_14 { get; set; }

        public decimal AMOUNT_15 { get; set; }

        public decimal AMOUNT_16 { get; set; }

        public decimal AMOUNT_17 { get; set; }

        public decimal AMOUNT_18 { get; set; }

        public decimal AMOUNT_19 { get; set; }

        public decimal AMOUNT_20 { get; set; }

        public decimal AMOUNT_21 { get; set; }

        public decimal AMOUNT_22 { get; set; }

        public decimal AMOUNT_23 { get; set; }

        public decimal AMOUNT_24 { get; set; }

        public decimal AMOUNT_25 { get; set; }

        public decimal AMOUNT_26 { get; set; }

        public decimal AMOUNT_27 { get; set; }

        public decimal AMOUNT_28 { get; set; }

        public decimal AMOUNT_29 { get; set; }

        public decimal AMOUNT_30 { get; set; }

        public decimal AMOUNT_31 { get; set; }

        public decimal AMOUNT_32 { get; set; }

        public decimal AMOUNT_33 { get; set; }

        public decimal AMOUNT_34 { get; set; }

        public decimal AMOUNT_35 { get; set; }

        public decimal AMOUNT_36 { get; set; }

        public decimal AMOUNT_37 { get; set; }

        public decimal AMOUNT_38 { get; set; }

        public decimal AMOUNT_39 { get; set; }

        public decimal AMOUNT_40 { get; set; }

        public decimal AMOUNT_41 { get; set; }

        public decimal AMOUNT_42 { get; set; }

        public decimal AMOUNT_43 { get; set; }

        public decimal AMOUNT_44 { get; set; }

        public decimal AMOUNT_45 { get; set; }

        public decimal AMOUNT_46 { get; set; }

        public decimal AMOUNT_47 { get; set; }

        public decimal AMOUNT_48 { get; set; }

        public decimal AMOUNT_49 { get; set; }

        public decimal AMOUNT_50 { get; set; }

        public Mrs00290RDO(HIS_SERE_SERV r)
        {
            Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00290RDO>(this, r);
            DIC_SVT_PRICE = new Dictionary<string, decimal>();
            DIC_PAR_PRICE = new Dictionary<string, decimal>();
            DIC_CATE_PRICE = new Dictionary<string, decimal>();
            DIC_HSVT_PRICE = new Dictionary<string, decimal>();
            DIC_SV_PRICE = new Dictionary<string, decimal>();

            //so luong
            DIC_SVT_AMOUNT = new Dictionary<string, decimal>();
            DIC_PAR_AMOUNT = new Dictionary<string, decimal>();
            DIC_CATE_AMOUNT = new Dictionary<string, decimal>();
            DIC_HSVT_AMOUNT = new Dictionary<string, decimal>();
            DIC_SV_AMOUNT = new Dictionary<string, decimal>();
        }

        public Mrs00290RDO()
        {
            // TODO: Complete member initialization
        }

        public string SERVICE_TYPE_CODE { get; set; }

        public string HEIN_SERVICE_TYPE_CODE { get; set; }

        public long EXAM_ROOM_ID { get; set; }

        public string EXAM_ROOM_NAME { get; set; }

        public string EXAM_ROOM_CODE { get; set; }

        public string CATEGORY_CODE { get; set; }

        public Dictionary<string, int> DIC_SVT_TREATMENT { get; set; }

        public Dictionary<string, int> DIC_CATE_TREATMENT { get; set; }

        public Dictionary<string, int> DIC_PAR_TREATMENT { get; set; }

        public Dictionary<string, int> DIC_HSVT_TREATMENT { get; set; }

        public Dictionary<string, int> DIC_SV_TREATMENT { get; set; }

        public string REPORT_PERIOD { get; set; }

        public Dictionary<string, string> DIC_SVT_ICD { get; set; }

        public Dictionary<string, string> DIC_CATE_ICD { get; set; }

        public Dictionary<string, string> DIC_PAR_ICD { get; set; }

        public Dictionary<string, string> DIC_HSVT_ICD { get; set; }

        public Dictionary<string, string> DIC_SV_ICD { get; set; }

        public Dictionary<string, string> DIC_EXECUTE_ROLE { get; set; }

        public short? FEE_TYPE { get; set; }
    }
    public class SERVICE_NAME
    {
        public decimal SERVICE_NAME_1 { get; set; }

        public decimal SERVICE_NAME_2 { get; set; }

        public decimal SERVICE_NAME_3 { get; set; }

        public decimal SERVICE_NAME_4 { get; set; }

        public decimal SERVICE_NAME_5 { get; set; }

        public decimal SERVICE_NAME_6 { get; set; }

        public decimal SERVICE_NAME_7 { get; set; }

        public decimal SERVICE_NAME_8 { get; set; }

        public decimal SERVICE_NAME_9 { get; set; }

        public decimal SERVICE_NAME_10 { get; set; }

        public decimal SERVICE_NAME_11 { get; set; }

        public decimal SERVICE_NAME_12 { get; set; }

        public decimal SERVICE_NAME_13 { get; set; }

        public decimal SERVICE_NAME_14 { get; set; }

        public decimal SERVICE_NAME_15 { get; set; }

        public decimal SERVICE_NAME_16 { get; set; }

        public decimal SERVICE_NAME_17 { get; set; }

        public decimal SERVICE_NAME_18 { get; set; }

        public decimal SERVICE_NAME_19 { get; set; }

        public decimal SERVICE_NAME_20 { get; set; }

        public decimal SERVICE_NAME_21 { get; set; }

        public decimal SERVICE_NAME_22 { get; set; }

        public decimal SERVICE_NAME_23 { get; set; }

        public decimal SERVICE_NAME_24 { get; set; }

        public decimal SERVICE_NAME_25 { get; set; }

        public decimal SERVICE_NAME_26 { get; set; }

        public decimal SERVICE_NAME_27 { get; set; }

        public decimal SERVICE_NAME_28 { get; set; }

        public decimal SERVICE_NAME_29 { get; set; }

        public decimal SERVICE_NAME_30 { get; set; }

        public decimal SERVICE_NAME_31 { get; set; }

        public decimal SERVICE_NAME_32 { get; set; }

        public decimal SERVICE_NAME_33 { get; set; }

        public decimal SERVICE_NAME_34 { get; set; }

        public decimal SERVICE_NAME_35 { get; set; }

        public decimal SERVICE_NAME_36 { get; set; }

        public decimal SERVICE_NAME_37 { get; set; }

        public decimal SERVICE_NAME_38 { get; set; }

        public decimal SERVICE_NAME_39 { get; set; }

        public decimal SERVICE_NAME_40 { get; set; }

        public decimal SERVICE_NAME_41 { get; set; }

        public decimal SERVICE_NAME_42 { get; set; }

        public decimal SERVICE_NAME_43 { get; set; }

        public decimal SERVICE_NAME_44 { get; set; }

        public decimal SERVICE_NAME_45 { get; set; }

        public decimal SERVICE_NAME_46 { get; set; }

        public decimal SERVICE_NAME_47 { get; set; }

        public decimal SERVICE_NAME_48 { get; set; }

        public decimal SERVICE_NAME_49 { get; set; }

        public decimal SERVICE_NAME_50 { get; set; }
    }


}
