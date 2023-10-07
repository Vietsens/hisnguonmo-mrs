using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00101
{
    class Mrs00101RDO
    {
        public MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL HEIN_APPROVAL { get; set; }

        public long PATIENT_ID { get; set; }
        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long TOTAL_DATE { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }

        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string MEDIORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string FIRST_CHAR_CARD { get; set; }
        public string HEIN_TREATMENT_TYPE_CODE { get; set; }

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
        public decimal TT_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00101RDO() { }

        public Mrs00101RDO(MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval)
        {
            this.HEIN_APPROVAL = heinApproval;
            this.HEIN_CARD_NUMBER = heinApproval.HEIN_CARD_NUMBER;
            this.FIRST_CHAR_CARD = (heinApproval.HEIN_CARD_NUMBER ?? " ").Substring(0, 1);
            this.HEIN_CARD_NUMBER_1 = (heinApproval.HEIN_CARD_NUMBER ?? " ").Substring(0, 2);
            this.MEDIORG_NAME = heinApproval.HEIN_MEDI_ORG_CODE;
            this.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
            this.PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
            this.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME;
            this.SetExtendField();
        }

        public void SetExtendField()
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
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }
        }

        public string IN_DEPARTMENT_NAME { get; set; }

        public string REQUEST_LOGINNAME { get; set; }

        public string REQUEST_USERNAME { get; set; }

        public string EXECUTE_ROOM_CODE { get; set; }

        public string EXECUTE_ROOM_NAME { get; set; }

        public string HEIN_CARD_NUMBER_1 { get; set; }
    }
}
