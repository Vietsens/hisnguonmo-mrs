using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00346
{
    public class Mrs00346RDO : HIS_TREATMENT
    {
        public string YEAR_STR { get; set; }
        public string AGE_STR { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_MEDI_ORG_CODE { get; set; }
        public string IN_TIME_STR { get; set; }

        public decimal? TOTAL_TEST { get; set; }
        public decimal? TOTAL_DIIM_FUEX { get; set; }
        public decimal? TOTAL_MEDICINE { get; set; }
        public decimal? TOTAL_BLOOD { get; set; }
        public decimal? TOTAL_SURG_MISU { get; set; }
        public decimal? TOTAL_MATERIAL { get; set; }
        public decimal? TOTAL_HIGHTECH { get; set; }
        public decimal? TOTAL_MEDICINE_CANCER { get; set; }
        public decimal? TOTAL_EXAM { get; set; }
        public decimal? TOTAL_BED { get; set; }
        public decimal? TOTAL_TRAN { get; set; }
        public decimal? TOTAL_OTHER { get; set; }
        //public decimal? TOTAL_DISCOUNT { get; set; }

        public decimal? DEPOSIT_AMOUNT { get; set; }
        public decimal? REPAY_AMOUNT { get; set; }
        //public decimal? TOTAL_HEIN_PRICE { get; set; }
        public decimal? SUB_AMOUNT { get; set; }
        public decimal? EXPEND_PTTT_AMOUNT { get; set; }
        public decimal? EXPEND_BED_AMOUNT { get; set; }
        public decimal? TOTAL_PRICE_EXPEND { get; set; }
        public decimal? BILL_AMOUNT { get; set; }
        public decimal? OTHER_AMOUNT { get; set; }
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }

        public Mrs00346RDO() { }

        public Mrs00346RDO(HIS_TREATMENT treatment)
        {
            if (treatment != null)
            {
                this.APPOINTMENT_CODE = treatment.APPOINTMENT_CODE;
                this.APPOINTMENT_DATE = treatment.APPOINTMENT_DATE;
                this.APPOINTMENT_DESC = treatment.APPOINTMENT_DESC;
                this.APPOINTMENT_SURGERY = treatment.APPOINTMENT_SURGERY;
                this.APPOINTMENT_TIME = treatment.APPOINTMENT_TIME;
                this.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME;
                this.CLINICAL_NOTE = treatment.CLINICAL_NOTE;
                this.COVID_PATIENT_CODE = treatment.COVID_PATIENT_CODE;
                this.DEATH_CERT_NUM = treatment.DEATH_CERT_NUM;
                this.DEATH_DOCUMENT_DATE = treatment.DEATH_DOCUMENT_DATE;
                this.DEATH_DOCUMENT_NUMBER = treatment.DEATH_DOCUMENT_NUMBER;
                this.DEATH_DOCUMENT_PLACE = treatment.DEATH_DOCUMENT_PLACE;
                this.DEATH_DOCUMENT_TYPE = treatment.DEATH_DOCUMENT_TYPE;
                this.DEATH_PLACE = treatment.DEATH_PLACE;
                this.DEATH_TIME = treatment.DEATH_TIME;
                this.DEATH_WITHIN_ID = treatment.DEATH_WITHIN_ID;
                this.DOCTOR_LOGINNAME = treatment.DOCTOR_LOGINNAME;
                this.DOCTOR_USERNAME = treatment.DOCTOR_USERNAME;
                this.DOCUMENT_VIEW_COUNT = treatment.DOCUMENT_VIEW_COUNT;
                this.END_CODE = treatment.END_CODE;
                this.END_DEPARTMENT_ID = treatment.END_DEPARTMENT_ID;
                this.END_LOGINNAME = treatment.END_LOGINNAME;
                this.END_ROOM_ID = treatment.END_ROOM_ID;
                this.EXTRA_END_CODE = treatment.EXTRA_END_CODE;
                this.EXTRA_END_CODE_SEED_CODE = treatment.EXTRA_END_CODE_SEED_CODE;
                this.EYE_TENSION_LEFT = treatment.EYE_TENSION_LEFT;
                this.EYE_TENSION_RIGHT = treatment.EYE_TENSION_RIGHT;
                this.EYESIGHT_GLASS_LEFT = treatment.EYESIGHT_GLASS_LEFT;
                this.EYESIGHT_GLASS_RIGHT = treatment.EYESIGHT_GLASS_RIGHT;
                this.EYESIGHT_LEFT = treatment.EYESIGHT_LEFT;
                this.EYESIGHT_RIGHT = treatment.EYESIGHT_RIGHT;
                this.FEE_LOCK_DEPARTMENT_ID = treatment.FEE_LOCK_DEPARTMENT_ID;
                this.FEE_LOCK_LOGINNAME = treatment.FEE_LOCK_LOGINNAME;
                this.FEE_LOCK_ORDER = treatment.FEE_LOCK_ORDER;
                this.FEE_LOCK_ROOM_ID = treatment.FEE_LOCK_ROOM_ID;
                this.FEE_LOCK_TIME = treatment.FEE_LOCK_TIME;
                this.FEE_LOCK_USERNAME = treatment.FEE_LOCK_USERNAME;
                this.HEIN_LOCK_TIME = treatment.HEIN_LOCK_TIME;
                this.HOSPITALIZATION_REASON = treatment.HOSPITALIZATION_REASON;
                this.HOSPITALIZE_DEPARTMENT_ID = treatment.HOSPITALIZE_DEPARTMENT_ID;
                this.ICD_CAUSE_CODE = treatment.ICD_CAUSE_CODE;
                this.ICD_CAUSE_NAME = treatment.ICD_CAUSE_NAME;
                this.ICD_CODE = treatment.ICD_CODE;
                this.ICD_NAME = treatment.ICD_NAME;
                this.ICD_SUB_CODE = treatment.ICD_SUB_CODE;
                this.ICD_TEXT = treatment.ICD_TEXT;
                this.ID = treatment.ID;
                this.IN_CODE = treatment.IN_CODE;
                this.IN_CODE_SEED_CODE = treatment.IN_CODE_SEED_CODE;
                this.IN_DATE = treatment.IN_DATE;
                this.IN_DEPARTMENT_ID = treatment.IN_DEPARTMENT_ID;
                this.IN_ICD_CODE = treatment.IN_ICD_CODE;
                this.IN_ICD_NAME = treatment.IN_ICD_NAME;
                this.IN_ICD_SUB_CODE = treatment.IN_ICD_SUB_CODE;
                this.IN_ICD_TEXT = treatment.IN_ICD_TEXT;
                this.IN_LOGINNAME = treatment.IN_LOGINNAME;
                this.IN_ROOM_ID = treatment.IN_ROOM_ID;
                this.IN_TIME = treatment.IN_TIME;
                this.IN_TREATMENT_TYPE_ID = treatment.IN_TREATMENT_TYPE_ID;
                this.IN_USERNAME = treatment.IN_USERNAME;
                this.IS_ACTIVE = treatment.IS_ACTIVE;
                this.IS_CHRONIC = treatment.IS_CHRONIC;
                this.IS_CREATING_TRANSACTION = treatment.IS_CREATING_TRANSACTION;
                this.IS_DELETE = treatment.IS_DELETE;
                this.IS_EARLY_NEWBORN_CARE = treatment.IS_EARLY_NEWBORN_CARE;
                this.IS_EMERGENCY = treatment.IS_EMERGENCY;
                this.IS_END_CODE_REQUEST = treatment.IS_END_CODE_REQUEST;
                this.IS_HAS_AUPOPSY = treatment.IS_HAS_AUPOPSY;
                this.IS_HOLD_BHYT_CARD = treatment.IS_HOLD_BHYT_CARD;
                this.IS_IN_CODE_REQUEST = treatment.IS_IN_CODE_REQUEST;
                this.IS_INTEGRATE_HIS_SENT = treatment.IS_INTEGRATE_HIS_SENT;
                this.IS_KSK_APPROVE = treatment.IS_KSK_APPROVE;
                this.IS_LOCK_FEE = treatment.IS_LOCK_FEE;
                this.IS_LOCK_HEIN = treatment.IS_LOCK_HEIN;
                this.IS_NOT_CHECK_LHMP = treatment.IS_NOT_CHECK_LHMP;
                this.IS_NOT_CHECK_LHSP = treatment.IS_NOT_CHECK_LHSP;
                this.IS_OLD_TEMP_BED = treatment.IS_OLD_TEMP_BED;
                this.IS_OUT_CODE_REQUEST = treatment.IS_OUT_CODE_REQUEST;
                this.IS_PAUSE = treatment.IS_PAUSE;
                this.IS_REMOTE = treatment.IS_REMOTE;
                this.IS_SYNC_EMR = treatment.IS_SYNC_EMR;
                this.IS_TEMPORARY_LOCK = treatment.IS_TEMPORARY_LOCK;
                this.IS_TEST_BLOOD_SUGAR = treatment.IS_TEST_BLOOD_SUGAR;
                this.IS_TRANSFER_IN = treatment.IS_TRANSFER_IN;
                this.IS_TUBERCULOSIS = treatment.IS_TUBERCULOSIS;
                this.IS_YDT_UPLOAD = treatment.IS_YDT_UPLOAD;
                this.JSON_FORM_ID = treatment.JSON_FORM_ID;
                this.KSK_ORDER = treatment.KSK_ORDER;
                this.LAST_DEPARTMENT_ID = treatment.LAST_DEPARTMENT_ID;
                this.MAIN_CAUSE = treatment.MAIN_CAUSE;
                this.MEDI_ORG_CODE = treatment.MEDI_ORG_CODE;
                this.MEDI_ORG_NAME = treatment.MEDI_ORG_NAME;
                this.MEDI_RECORD_ID = treatment.MEDI_RECORD_ID;
                this.MEDI_RECORD_TYPE_ID = treatment.MEDI_RECORD_TYPE_ID;
                this.NEED_SICK_LEAVE_CERT = treatment.NEED_SICK_LEAVE_CERT;
                this.NEWBORN_CARE_AT_HOME = treatment.NEWBORN_CARE_AT_HOME;
                this.NEXT_EXAM_FROM_TIME = treatment.NEXT_EXAM_FROM_TIME;
                this.NEXT_EXAM_NUM_ORDER = treatment.NEXT_EXAM_NUM_ORDER;
                this.NEXT_EXAM_TO_TIME = treatment.NEXT_EXAM_TO_TIME;
                this.NUM_ORDER_ISSUE_ID = treatment.NUM_ORDER_ISSUE_ID;
                this.NUMBER_OF_FULL_TERM_BIRTH = treatment.NUMBER_OF_FULL_TERM_BIRTH;
                this.NUMBER_OF_MISCARRIAGE = treatment.NUMBER_OF_MISCARRIAGE;
                this.NUMBER_OF_PREMATURE_BIRTH = treatment.NUMBER_OF_PREMATURE_BIRTH;
                this.NUMBER_OF_TESTS = treatment.NUMBER_OF_TESTS;
                this.OTHER_PAY_SOURCE_ID = treatment.OTHER_PAY_SOURCE_ID;
                this.OUT_CODE = treatment.OUT_CODE;
                this.OUT_DATE = treatment.OUT_DATE;
                this.OUT_TIME = treatment.OUT_TIME;
                this.OUTPATIENT_DATE_FROM = treatment.OUTPATIENT_DATE_FROM;
                this.OUTPATIENT_DATE_TO = treatment.OUTPATIENT_DATE_TO;
                this.OWE_MODIFY_TIME = treatment.OWE_MODIFY_TIME;
                this.OWE_TYPE_ID = treatment.OWE_TYPE_ID;
                this.PATIENT_CONDITION = treatment.PATIENT_CONDITION;
                this.PATIENT_ID = treatment.PATIENT_ID;
                this.PERMISION_UPDATE = treatment.PERMISION_UPDATE;
                this.PROGRAM_ID = treatment.PROGRAM_ID;
                this.PROVISIONAL_DIAGNOSIS = treatment.PROVISIONAL_DIAGNOSIS;
                this.RECORD_INSPECTION_REJECT_NOTE = treatment.RECORD_INSPECTION_REJECT_NOTE;
                this.RECORD_INSPECTION_STT_ID = treatment.RECORD_INSPECTION_STT_ID;
                this.REJECT_STORE_REASON = treatment.REJECT_STORE_REASON;
                this.SHOW_ICD_CODE = treatment.SHOW_ICD_CODE;
                this.SHOW_ICD_NAME = treatment.SHOW_ICD_NAME;
                this.SHOW_ICD_SUB_CODE = treatment.SHOW_ICD_SUB_CODE;
                this.SHOW_ICD_TEXT = treatment.SHOW_ICD_TEXT;
                this.SICK_HEIN_CARD_NUMBER = treatment.SICK_HEIN_CARD_NUMBER;
                this.SICK_LEAVE_DAY = treatment.SICK_LEAVE_DAY;
                this.SICK_LEAVE_FROM = treatment.SICK_LEAVE_FROM;
                this.SICK_LEAVE_TO = treatment.SICK_LEAVE_TO;
                this.SICK_LOGINNAME = treatment.SICK_LOGINNAME;
                this.SICK_NUM_ORDER = treatment.SICK_NUM_ORDER;
                this.SICK_USERNAME = treatment.SICK_USERNAME;
                this.STORE_BORDEREAU_CODE = treatment.STORE_BORDEREAU_CODE;
                this.STORE_CODE = treatment.STORE_CODE;
                this.STORE_TIME = treatment.STORE_TIME;
                this.SUBCLINICAL_RESULT = treatment.SUBCLINICAL_RESULT;
                this.SURGERY = treatment.SURGERY;
                this.SURGERY_APPOINTMENT_TIME = treatment.SURGERY_APPOINTMENT_TIME;
                this.TDL_DOCUMENT_BOOK_CODE = treatment.TDL_DOCUMENT_BOOK_CODE;
                this.TDL_FIRST_EXAM_ROOM_ID = treatment.TDL_FIRST_EXAM_ROOM_ID;
                this.TDL_HEIN_CARD_FROM_TIME = treatment.TDL_HEIN_CARD_FROM_TIME;
                this.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                this.TDL_HEIN_CARD_TO_TIME = treatment.TDL_HEIN_CARD_TO_TIME;
                this.TDL_HEIN_MEDI_ORG_CODE = treatment.TDL_HEIN_MEDI_ORG_CODE;
                this.TDL_HEIN_MEDI_ORG_NAME = treatment.TDL_HEIN_MEDI_ORG_NAME;
                this.TDL_KSK_CONTRACT_ID = treatment.TDL_KSK_CONTRACT_ID;
                this.TDL_KSK_CONTRACT_IS_RESTRICTED = treatment.TDL_KSK_CONTRACT_IS_RESTRICTED;
                this.TDL_PATIENT_ACCOUNT_NUMBER = treatment.TDL_PATIENT_ACCOUNT_NUMBER;
                this.TDL_PATIENT_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                this.TDL_PATIENT_AVATAR_URL = treatment.TDL_PATIENT_AVATAR_URL;
                this.TDL_PATIENT_CAREER_NAME = treatment.TDL_PATIENT_CAREER_NAME;
                this.TDL_PATIENT_CCCD_DATE = treatment.TDL_PATIENT_CCCD_DATE;
                this.TDL_PATIENT_CCCD_NUMBER = treatment.TDL_PATIENT_CCCD_NUMBER;
                this.TDL_PATIENT_CCCD_PLACE = treatment.TDL_PATIENT_CCCD_PLACE;
                this.TDL_PATIENT_CLASSIFY_ID = treatment.TDL_PATIENT_CLASSIFY_ID;
                this.TDL_PATIENT_CMND_DATE = treatment.TDL_PATIENT_CMND_DATE;
                this.TDL_PATIENT_CMND_NUMBER = treatment.TDL_PATIENT_CMND_NUMBER;
                this.TDL_PATIENT_CMND_PLACE = treatment.TDL_PATIENT_CMND_PLACE;
                this.TDL_PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                this.TDL_PATIENT_COMMUNE_CODE = treatment.TDL_PATIENT_COMMUNE_CODE;
                this.TDL_PATIENT_COMMUNE_NAME = treatment.TDL_PATIENT_COMMUNE_NAME;
                this.TDL_PATIENT_DISTRICT_CODE = treatment.TDL_PATIENT_DISTRICT_CODE;
                this.TDL_PATIENT_DISTRICT_NAME = treatment.TDL_PATIENT_DISTRICT_NAME;
                this.TDL_PATIENT_DOB = treatment.TDL_PATIENT_DOB;
                this.TDL_PATIENT_ETHNIC_NAME = treatment.TDL_PATIENT_ETHNIC_NAME;
                this.TDL_PATIENT_FATHER_NAME = treatment.TDL_PATIENT_FATHER_NAME;
                this.TDL_PATIENT_FIRST_NAME = treatment.TDL_PATIENT_FIRST_NAME;
                this.TDL_PATIENT_GENDER_ID = treatment.TDL_PATIENT_GENDER_ID;
                this.TDL_PATIENT_GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
                this.TDL_PATIENT_IS_HAS_NOT_DAY_DOB = treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB;
                this.TDL_PATIENT_LAST_NAME = treatment.TDL_PATIENT_LAST_NAME;
                this.TDL_PATIENT_MILITARY_RANK_NAME = treatment.TDL_PATIENT_MILITARY_RANK_NAME;
                this.TDL_PATIENT_MOBILE = treatment.TDL_PATIENT_MOBILE;
                this.TDL_PATIENT_MOTHER_NAME = treatment.TDL_PATIENT_MOTHER_NAME;
                this.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                this.TDL_PATIENT_NATIONAL_CODE = treatment.TDL_PATIENT_NATIONAL_CODE;
                this.TDL_PATIENT_NATIONAL_NAME = treatment.TDL_PATIENT_NATIONAL_NAME;
                this.TDL_PATIENT_PASSPORT_DATE = treatment.TDL_PATIENT_PASSPORT_DATE;
                this.TDL_PATIENT_PASSPORT_NUMBER = treatment.TDL_PATIENT_PASSPORT_NUMBER;
                this.TDL_PATIENT_PASSPORT_PLACE = treatment.TDL_PATIENT_PASSPORT_PLACE;
                this.TDL_PATIENT_PHONE = treatment.TDL_PATIENT_PHONE;
                this.TDL_PATIENT_POSITION_ID = treatment.TDL_PATIENT_POSITION_ID;
                this.TDL_PATIENT_PROVINCE_CODE = treatment.TDL_PATIENT_PROVINCE_CODE;
                this.TDL_PATIENT_PROVINCE_NAME = treatment.TDL_PATIENT_PROVINCE_NAME;
                this.TDL_PATIENT_RELATIVE_ADDRESS = treatment.TDL_PATIENT_RELATIVE_ADDRESS;
                this.TDL_PATIENT_RELATIVE_MOBILE = treatment.TDL_PATIENT_RELATIVE_MOBILE;
                this.TDL_PATIENT_RELATIVE_NAME = treatment.TDL_PATIENT_RELATIVE_NAME;
                this.TDL_PATIENT_RELATIVE_PHONE = treatment.TDL_PATIENT_RELATIVE_PHONE;
                this.TDL_PATIENT_RELATIVE_TYPE = treatment.TDL_PATIENT_RELATIVE_TYPE;
                this.TDL_PATIENT_TAX_CODE = treatment.TDL_PATIENT_TAX_CODE;
                this.TDL_PATIENT_TYPE_ID = treatment.TDL_PATIENT_TYPE_ID;
                this.TDL_PATIENT_UNSIGNED_NAME = treatment.TDL_PATIENT_UNSIGNED_NAME;
                this.TDL_PATIENT_WORK_PLACE = treatment.TDL_PATIENT_WORK_PLACE;
                this.TDL_PATIENT_WORK_PLACE_ID = treatment.TDL_PATIENT_WORK_PLACE_ID;
                this.TDL_PATIENT_WORK_PLACE_NAME = treatment.TDL_PATIENT_WORK_PLACE_NAME;
                this.TDL_RELATIVE_CMND_NUMBER = treatment.TDL_RELATIVE_CMND_NUMBER;
                this.TDL_SOCIAL_INSURANCE_NUMBER = treatment.TDL_SOCIAL_INSURANCE_NUMBER;
                this.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID;
                this.TEST_HEPATITIS_B = treatment.TEST_HEPATITIS_B;
                this.TEST_HIV = treatment.TEST_HIV;
                this.TEST_SYPHILIS = treatment.TEST_SYPHILIS;
                this.TRADITIONAL_ICD_CODE = treatment.TRADITIONAL_ICD_CODE;
                this.TRADITIONAL_ICD_NAME = treatment.TRADITIONAL_ICD_NAME;
                this.TRADITIONAL_ICD_SUB_CODE = treatment.TRADITIONAL_ICD_SUB_CODE;
                this.TRADITIONAL_ICD_TEXT = treatment.TRADITIONAL_ICD_TEXT;
                this.TRADITIONAL_IN_ICD_CODE = treatment.TRADITIONAL_IN_ICD_CODE;
                this.TRADITIONAL_IN_ICD_NAME = treatment.TRADITIONAL_IN_ICD_NAME;
                this.TRADITIONAL_IN_ICD_SUB_CODE = treatment.TRADITIONAL_IN_ICD_SUB_CODE;
                this.TRADITIONAL_IN_ICD_TEXT = treatment.TRADITIONAL_IN_ICD_TEXT;
                this.TRADITIONAL_TRANS_IN_ICD_CODE = treatment.TRADITIONAL_TRANS_IN_ICD_CODE;
                this.TRADITIONAL_TRANS_IN_ICD_NAME = treatment.TRADITIONAL_TRANS_IN_ICD_NAME;
                this.TRAN_PATI_BOOK_NUMBER = treatment.TRAN_PATI_BOOK_NUMBER;
                this.TRAN_PATI_BOOK_TIME = treatment.TRAN_PATI_BOOK_TIME;
                this.TRAN_PATI_DEPARTMENT_LOGINNAME = treatment.TRAN_PATI_DEPARTMENT_LOGINNAME;
                this.TRAN_PATI_DEPARTMENT_USERNAME = treatment.TRAN_PATI_DEPARTMENT_USERNAME;
                this.TRAN_PATI_DOCTOR_LOGINNAME = treatment.TRAN_PATI_DOCTOR_LOGINNAME;
                this.TRAN_PATI_DOCTOR_USERNAME = treatment.TRAN_PATI_DOCTOR_USERNAME;
                this.TRAN_PATI_FORM_ID = treatment.TRAN_PATI_FORM_ID;
                this.TRAN_PATI_HOSPITAL_LOGINNAME = treatment.TRAN_PATI_HOSPITAL_LOGINNAME;
                this.TRAN_PATI_HOSPITAL_USERNAME = treatment.TRAN_PATI_HOSPITAL_USERNAME;
                this.TRAN_PATI_REASON_ID = treatment.TRAN_PATI_REASON_ID;
                this.TRAN_PATI_TECH_ID = treatment.TRAN_PATI_TECH_ID;
                this.TRANSFER_IN_CMKT = treatment.TRANSFER_IN_CMKT;
                this.TRANSFER_IN_CODE = treatment.TRANSFER_IN_CODE;
                this.TRANSFER_IN_FORM_ID = treatment.TRANSFER_IN_FORM_ID;
                this.TRANSFER_IN_ICD_CODE = treatment.TRANSFER_IN_ICD_CODE;
                this.TRANSFER_IN_ICD_ID__DELETE = treatment.TRANSFER_IN_ICD_ID__DELETE;
                this.TRANSFER_IN_ICD_NAME = treatment.TRANSFER_IN_ICD_NAME;
                this.TRANSFER_IN_MEDI_ORG_CODE = treatment.TRANSFER_IN_MEDI_ORG_CODE;
                this.TRANSFER_IN_MEDI_ORG_NAME = treatment.TRANSFER_IN_MEDI_ORG_NAME;
                this.TRANSFER_IN_REASON_ID = treatment.TRANSFER_IN_REASON_ID;
                this.TRANSFER_IN_TIME_FROM = treatment.TRANSFER_IN_TIME_FROM;
                this.TRANSFER_IN_TIME_TO = treatment.TRANSFER_IN_TIME_TO;
                this.TRANSFER_IN_URL = treatment.TRANSFER_IN_URL;
                this.TRANSPORT_VEHICLE = treatment.TRANSPORT_VEHICLE;
                this.TRANSPORTER = treatment.TRANSPORTER;
                this.TREATMENT_CODE = treatment.TREATMENT_CODE;
                this.TREATMENT_DAY_COUNT = treatment.TREATMENT_DAY_COUNT;
                this.TREATMENT_DIRECTION = treatment.TREATMENT_DIRECTION;
                this.TREATMENT_END_TYPE_EXT_ID = treatment.TREATMENT_END_TYPE_EXT_ID;
                this.TREATMENT_END_TYPE_ID = treatment.TREATMENT_END_TYPE_ID;
                this.TREATMENT_METHOD = treatment.TREATMENT_METHOD;
                this.TREATMENT_ORDER = treatment.TREATMENT_ORDER;
                this.TREATMENT_RESULT_ID = treatment.TREATMENT_RESULT_ID;
                this.TREATMENT_STT_ID = treatment.TREATMENT_STT_ID;
                this.USED_MEDICINE = treatment.USED_MEDICINE;
                this.VACCINE_ID = treatment.VACCINE_ID;
                this.VACINATION_ORDER = treatment.VACINATION_ORDER;
                this.VIR_IN_MONTH = treatment.VIR_IN_MONTH;
                this.VIR_IN_YEAR = treatment.VIR_IN_YEAR;
                this.VIR_OUT_MONTH = treatment.VIR_OUT_MONTH;
                this.VIR_OUT_YEAR = treatment.VIR_OUT_YEAR;
                this.VIR_TRAN_PATI_BOOK_YEAR = treatment.VIR_TRAN_PATI_BOOK_YEAR;

                this.YEAR_STR = this.TDL_PATIENT_DOB!=null && this.TDL_PATIENT_DOB>1000?this.TDL_PATIENT_DOB.ToString().Substring(0, 4):null;
                this.AGE_STR = Inventec.Common.DateTime.Calculation.Age(this.TDL_PATIENT_DOB) + "";
                this.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.IN_TIME);
                this.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                this.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                this.GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
                this.END_DEPARTMENT_ID = treatment.LAST_DEPARTMENT_ID;

            }
        }

        public Dictionary<string, decimal> DIC_EXPEND_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_PRICE { get; set; }

        public long NUM_ORDER { get; set; }

        public decimal TOTAL_PRICE { get; set; }

        public decimal TOTAL_PATIENT_PRICE { get; set; }

        public decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public decimal TOTAL_HEIN_PRICE { get; set; }

        public decimal TOTAL_PATIENT_PRICE_DIFF { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_1 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_2 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_3 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_4 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_5 { get; set; }

        public string END_DEPARTMENT_CODE { get; set; }

        public string END_DEPARTMENT_NAME { get; set; }

        public decimal? MEDICINE_PRICE_RATIO { get; set; }

        public decimal? TOTAL_HEIN_PRICE_NDS { get; set; }

        public string HEIN_CARD_NUMBER_1 { get; set; }

        public string HEIN_TREATMENT_TYPE_CODE { get; set; }

        public decimal TOTAL_MEDICINE_RATIO { get; set; }

        public decimal TOTAL_MATERIAL_RATIO { get; set; }

        public string PATIENT_CASE_NAME { get; set; }

        public string PATIENT_CASE_CODE { get; set; }

        public decimal VIR_TOTAL_PRICE { get; set; }

        public decimal VIR_TOTAL_PATIENT_PRICE { get; set; }

        public decimal VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public decimal VIR_TOTAL_PATIENT_PRICE_DIFF { get; set; }

        public decimal VIR_TOTAL_HEIN_PRICE { get; set; }

        public decimal DISCOUNT { get; set; }

        public decimal SESE_DEPO_AMOUNT { get; set; }

        public long TRANSACTION_TIME { get; set; }

        public decimal? EXEMPTION { get; set; }
        //thong tin hoa don dien tu
        public string INVOICE_CODE { get; set; }
        public string INVOICE_SYS { get; set; }
        public string EINVOICE_NUM_ORDER { get; set; }

        public decimal TOTAL_DISCOUNT { get; set; }

        public string END_ROOM_CODE { get; set; }

        public string END_ROOM_NAME { get; set; }
    }
}
