using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00652
{
    class Mrs00652RDO
    {
        public V_HIS_HEIN_APPROVAL HEIN_APPROVAL { get; set; }

        public long PATIENT_ID { get; set; }
        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long? TOTAL_DATE { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }

        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string MEDIORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string BRANCH_NAME { get; set; }

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
        public string END_DEPARTMENT_CODE { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string END_ROOM_DEPARTMENT_NAME { get; set; }
        public string HEIN_CARD_NUMBER_1 { get; set; }

        public decimal TOTAL_PATIENT_PRICE_CCT { get; set; }
        public decimal TOTAL_PATIENT_PRICE_TT { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public string DOB_STR { get; set; }
        public string GENDER_NAME { get; set; }
        public string HEIN_TREATMENT_TYPE_CODE { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_1 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_2 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_3 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_4 { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE_5 { get; set; }

        public Dictionary<string,decimal> DIC_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_TOTAL_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public decimal TOTAL_BHVN_PRICE { get;  set; }

        public Mrs00652RDO() { }

        public Mrs00652RDO(V_HIS_HEIN_APPROVAL heinApproval)
        {
            this.HEIN_APPROVAL = heinApproval;
            this.HEIN_CARD_NUMBER = heinApproval.HEIN_CARD_NUMBER;
            this.MEDIORG_NAME = heinApproval.HEIN_MEDI_ORG_CODE;
            this.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
            this.PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
            this.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME.TrimEnd(' ');
            this.BRANCH_NAME = (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH()).BRANCH_NAME;
            this.TREATMENT_TYPE_ID = heinApproval.TREATMENT_TYPE_ID;
            this.GENDER_NAME = heinApproval.TDL_PATIENT_GENDER_NAME;
            this.HEIN_TREATMENT_TYPE_CODE = heinApproval.HEIN_TREATMENT_TYPE_CODE;
            if (!string.IsNullOrWhiteSpace(heinApproval.HEIN_CARD_NUMBER))
            {
                this.HEIN_CARD_NUMBER_1 = heinApproval.HEIN_CARD_NUMBER.Substring(0, 2);
            }
            this.SetExtendField();
        }

        private void SetExtendField()
        {
            if (this.HEIN_APPROVAL.TDL_PATIENT_DOB > 0)
            {
                try
                {
                    if (this.HEIN_APPROVAL.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        this.DOB_MALE = long.Parse((this.HEIN_APPROVAL.TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                    else if (this.HEIN_APPROVAL.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.DOB_FEMALE = long.Parse((this.HEIN_APPROVAL.TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                    this.DOB_STR = (this.HEIN_APPROVAL.TDL_PATIENT_DOB.ToString()).Substring(0, 4);
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }
        }

        public decimal AMOUNT { get; set; }

        public decimal PRICE { get; set; }

        public string HEIN_SERVICE_NAME { get; set; }

        public string HEIN_SERVICE_CODE { get; set; }

        public string CATEGORY_CODE { get; set; }

        public decimal VAT_RATIO { get; set; }

        public decimal? HEIN_RATIO { get; set; }

        public decimal TT_PRICE { get; set; }

        public long IN_TIME { get; set; }
    }
}
