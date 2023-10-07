using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00470
{
    public class Mrs00470RDO : V_HIS_HEIN_APPROVAL
    {
        public string PATIENT_NAME { get; set; }
        public long HEIN_CARD_FROM_TIME_STR { get; set; }
        public long HEIN_CARD_TO_TIME_STR { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string ICD_NAME_MAIN { get; set; }
        public string ICD_NAME_EXTRA { get; set; }
        public string ICD_CODE_EXTRA { get; set; }
        public string REASON_INPUT_CODE { get; set; }
        public string MEDI_ORG_FROM_CODE { get; set; }
        public string OPEN_TIME_SEPARATE_STR { get; set; }
        public string CLOSE_TIME_SEPARATE_STR { get; set; }
        public long? TOTAL_DAY { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public long INSURANCE_YEAR { get; set; }
        public long INSURANCE_MONTH { get; set; }
        public string HEIN_LIVE_AREA_CODE { get; set; }
        public string TREATMENT_TYPE_CODE { get; set; }
        public string CURRENT_MEDI_ORG_CODE { get; set; }
        public long PLACE_PAYMENT_CODE { get; set; }
        public long INSURANCE_STT { get; set; }
        public decimal REASON_OUT_PRICE { get; set; }
        public string REASON_OUT { get; set; }
        public decimal POLYLINE_PRICE { get; set; }
        public decimal EXCESS_PRICE { get; set; }
        public string ROUTE_CODE { get; set; }
        public string SPECIALITY_CODE { get; set; }
        public string IN_CODE { get; set; }
        public string GENDER_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public long DOB { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string END_LOGINNAME { get; set; }
        public string DOCTOR_LOGINNAME { get; set; }
        public string END_USERNAME { get; set; }
        public string DOCTOR_USERNAME { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public string FEE_LOCK_LOGINNAME { get; set; }
        public string FEE_LOCK_USERNAME { get; set; }
        public Mrs00470RDO(V_HIS_HEIN_APPROVAL heinApproval)
        {
            if (heinApproval != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00470RDO>(this, heinApproval);
                this.PATIENT_CODE = this.TDL_PATIENT_CODE;
                this.DOB = Convert.ToInt64(heinApproval.TDL_PATIENT_DOB.ToString().Substring(0, 8));
                this.VIR_ADDRESS = this.TDL_PATIENT_ADDRESS;
            }
        }


        public string END_DEPARTMENT_CODE { get; set; }

        public string END_DEPARTMENT_NAME { get; set; }

        public string DOCTOR { get; set; }

        public decimal TT_PRICE { get; set; }
    }
}
