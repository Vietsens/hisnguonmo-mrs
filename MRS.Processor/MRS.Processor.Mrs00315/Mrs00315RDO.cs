using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00315
{
    class Mrs00315RDO
    {
        public MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL HEIN_APPROVAL_BHYT { get; set; }

        public string IN_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public long DOB { get; set; }
        public string GENDER_CODE { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_MEDI_ORG_CODE { get; set; }
        public long HEIN_CARD_FROM_TIME_STR { get; set; }
        public long HEIN_CARD_TO_TIME_STR { get; set; }
        public string MEDIORG_CODE { get; set; }
        public string MEDIORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string ICD_CODE_EXTRA { get; set; }
        public string REASON_INPUT_CODE { get; set; }

        public string MEDI_ORG_FROM_CODE { get; set; }

        public string TRANSFER_IN_MEDI_ORG_CODE { get; set; }

        public string OPEN_TIME_SERPARATE_STR0 { get; set; }
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
        public string END_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }

        //TheoThoi gian vao
        public string IN_TIME_STR { get; set; }
        public long? MAIN_DAY { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public long GR_1_HEIN_FROM_TIME_STR { get; set; }
        public long GR_1_HEIN_TO_TIME_STR { get; set; }
        public string GR_2_HEIN_FROM_TIME_STR { get; set; }
        public string GR_2_HEIN_TO_TIME_STR { get; set; }

        public Mrs00315RDO(MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval)
        {
            this.HEIN_APPROVAL_BHYT = heinApproval;
            this.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
            this.MEDIORG_NAME = heinApproval.HEIN_MEDI_ORG_NAME;
            this.MEDIORG_CODE = heinApproval.HEIN_MEDI_ORG_CODE;
        }

        public Mrs00315RDO()
        {
            // TODO: Complete member initialization
        }

        public string MEDI_ORG_FROM_NAME { get; set; }

        public string TRANSFER_IN_MEDI_ORG_NAME { get; set; }

        public decimal TT_PRICE { get; set; }

        public decimal TDCN_PRICE { get; set; }

        public string REQUEST_DEPARTMENT_CODE { get; set; }

        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public long IN_TIME { get; set; }

        public long? OUT_TIME { get; set; }

        public long? CLINICAL_IN_TIME { get; set; }
    }
}
