using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00817
{
    public class Mrs00817RDO
    {
        public MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL HEIN_APPROVAL { get; set; }
        public long  TREATMENT_ID { get; set; }
        public string  PATIENT_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public string GENDER_CODE { get; set; }
        public string GENDER_NAME { set; get; }
        public long DOB { get; set; }
        public string VIR_ADDRESS { set; get; }
        public string  PATIENT_TYPE_NAME { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string HENI_CARD_NUMBER { set; get; }
        public string TREATMENT_CODE { set; get; }
        public string TREATMENT_TYPE_CODE { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public string HEIN_MEDI_ORG_CODE { set; get; }
        public string HEIN_MEDI_ORG_NAME { get; set; }
        public string HEIN_CARD_NUMBER{set;get;}
        public long IN_TIME { set; get; }
        public long? OUT_TIME { set; get; }
        public long CLINICAL_IN_TIME { set; get; }
        public long? TOTAL_DAY { set; get; }
        public string ICD_CODE_MAIN { set; get; }
        public string  ICD_NAME_MAIN { get; set; }
        public string ICD_CODE_EXTRA { get; set; }
        public string ICD_NAME_EXTRA { get; set; }
        public long  TREATMENT_DAY_COUNT { get; set; }
        public decimal TOTAL_PRICE { set; get; }
        public decimal TOTAL_PATIENT_PRICE { set; get; }
        public decimal TOTAL_HEIN_PRICE { set; get; }
        public decimal TOTAL_HEIN_PRICE_NDS { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string  DEPARTMENT_NAME { get; set; }
        public long BRANCH_ID { set; get; }
        public Mrs00817RDO()
        {

        }
        public Mrs00817RDO(MOS.EFMODEL.DataModels.V_HIS_HEIN_APPROVAL heinApproval)
        {
            this.HEIN_APPROVAL = heinApproval;
            this.TREATMENT_CODE = heinApproval.TREATMENT_CODE;
        }

        public void SetExtendField(Mrs00817RDO Data)
        {
        }
    }
}
