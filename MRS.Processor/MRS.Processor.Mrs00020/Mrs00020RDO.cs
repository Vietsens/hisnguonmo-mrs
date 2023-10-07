using MOS.MANAGER.HisBranch;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00020
{
    class Mrs00020RDO
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
        public string MEDIORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }
        public string FIRST_CHAR_CARD { get; set; }
        public string HEIN_TREATMENT_TYPE_CODE { get; set; }
        public string BRANCH_NAME { get; set; }

        public string HEIN_CARD_NUMBER { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal MEDICINE_PRICE_NDM { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal CPM_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal FUEX_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal TT_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal? PATIENT_PAY_PRICE { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public long END_DEPARTMENT_ID { get; set; }
        public int COUNT_TREATMENT { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string END_DEPARTMENT_CODE { get; set; }
        public long END_ROOM_DEPARTMENT_ID { get; set; }
        public string END_ROOM_DEPARTMENT_CODE { get; set; }
        public string END_ROOM_DEPARTMENT_NAME { get; set; }
        public string HEIN_CARD_NUMBER_1 { get; set; }
        public string PATIENT_CASE_NAME { get; set; }
        public string END_USERNAME { set; get; }
        public string END_LOGINNAME { set; get; }
        public Dictionary<string, decimal> DIC_PARENT_PRICE { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }
        public string CASHIER_ROOM_CODE { get; set; }
        public long? FEE_LOCK_TIME { get; set; }

        public long ACCUM_TREATMENT { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_PATIENT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_PATIENT_PRICE_BHYT { get; set; }

        public Mrs00020RDO()
        {

        }

        public Mrs00020RDO(MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval)
        {
            this.HEIN_APPROVAL = heinApproval;
            this.FIRST_CHAR_CARD = (heinApproval.HEIN_CARD_NUMBER??" ").Substring(0,1);
            this.HEIN_CARD_NUMBER = heinApproval.HEIN_CARD_NUMBER;
            this.MEDIORG_NAME = heinApproval.HEIN_MEDI_ORG_CODE;
            this.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
            this.PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
            this.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME.TrimEnd(' ');
            this.BRANCH_NAME = (HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH()).BRANCH_NAME;
            this.TREATMENT_TYPE_ID = heinApproval.TREATMENT_TYPE_ID;
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
                }
                catch (Exception ex)
                {
                    Inventec.Common.Logging.LogSystem.Error(ex);
                }
            }

        }

        public long END_ROOM_ID { get; set; }
        public string END_ROOM_CODE { get; set; }
        public string END_ROOM_NAME { get; set; }

        public long EXECUTE_DATE { get; set; }
        public string EINVOICE_NUMBER { get; set; }

        public string TRANS_REQ_CODE { get; set; }

        public string BANK_TRANSACTION_CODE { get; set; }

        public long? BANK_TRANSACTION_TIME { get; set; }

        public string RIGHT_ROUTE_CODE { get; set; }
        public string RIGHT_ROUTE_NAME { get; set; }

        //danh sách bổ sung để tính tổng
        public string HEIN_CARD_NUMBER_EXSUM { get; set; }
        public decimal TOTAL_PRICE_EXSUM { get; set; }
        public decimal TEST_PRICE_EXSUM { get; set; }
        public decimal DIIM_PRICE_EXSUM { get; set; }
        public decimal MEDICINE_PRICE_EXSUM { get; set; }
        public decimal MEDICINE_PRICE_NDM_EXSUM { get; set; }
        public decimal BLOOD_PRICE_EXSUM { get; set; }
        public decimal CPM_PRICE_EXSUM { get; set; }
        public decimal SURGMISU_PRICE_EXSUM { get; set; }
        public decimal MATERIAL_PRICE_EXSUM { get; set; }
        public decimal SERVICE_PRICE_RATIO_EXSUM { get; set; }
        public decimal MEDICINE_PRICE_RATIO_EXSUM { get; set; }
        public decimal MATERIAL_PRICE_RATIO_EXSUM { get; set; }
        public decimal BED_PRICE_EXSUM { get; set; }
        public decimal EXAM_PRICE_EXSUM { get; set; }
        public decimal FUEX_PRICE_EXSUM { get; set; }
        public decimal TRAN_PRICE_EXSUM { get; set; }
        public decimal TT_PRICE_EXSUM { get; set; }
        public decimal TOTAL_PATIENT_PRICE_EXSUM { get; set; }
        public decimal TOTAL_HEIN_PRICE_EXSUM { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS_EXSUM { get; set; }
        public decimal TOTAL_PATIENT_PRICE_BHYT_EXSUM { get; set; }
        public decimal? PATIENT_PAY_PRICE_EXSUM { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE_EXSUM { get; set; }

        //danh sách bổ sung để tính tổng
        public string HEIN_CARD_NUMBER_ALL { get; set; }
        public decimal TOTAL_PRICE_ALL { get; set; }
        public decimal TEST_PRICE_ALL { get; set; }
        public decimal DIIM_PRICE_ALL { get; set; }
        public decimal MEDICINE_PRICE_ALL { get; set; }
        public decimal MEDICINE_PRICE_NDM_ALL { get; set; }
        public decimal BLOOD_PRICE_ALL { get; set; }
        public decimal CPM_PRICE_ALL { get; set; }
        public decimal SURGMISU_PRICE_ALL { get; set; }
        public decimal MATERIAL_PRICE_ALL { get; set; }
        public decimal SERVICE_PRICE_RATIO_ALL { get; set; }
        public decimal MEDICINE_PRICE_RATIO_ALL { get; set; }
        public decimal MATERIAL_PRICE_RATIO_ALL { get; set; }
        public decimal BED_PRICE_ALL { get; set; }
        public decimal EXAM_PRICE_ALL { get; set; }
        public decimal FUEX_PRICE_ALL { get; set; }
        public decimal TRAN_PRICE_ALL { get; set; }
        public decimal TT_PRICE_ALL { get; set; }
        public decimal TOTAL_PATIENT_PRICE_ALL { get; set; }
        public decimal TOTAL_HEIN_PRICE_ALL { get; set; }
        public decimal TOTAL_HEIN_PRICE_NDS_ALL { get; set; }
        public decimal TOTAL_PATIENT_PRICE_BHYT_ALL { get; set; }
        public decimal? PATIENT_PAY_PRICE_ALL { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE_ALL { get; set; }
    }
}
