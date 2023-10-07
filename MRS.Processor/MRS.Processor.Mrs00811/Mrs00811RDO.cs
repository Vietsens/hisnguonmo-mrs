using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00811
{
    public class Mrs00811RDO
    {
        public string MEDICINE_USE_FROM_CODE { get; set; }

        public long  PATIENT_ID { get; set; }
        public string PATIENT_NAME { set; get; }
        public string PATIENT_CODE { set; get; }
        public string DOB { set; get; }
        public string GENDER { set; get; }
        public string IN_DATE { set; get; }
        public string OUT_DATE { set; get; }
        public decimal TREATMENT_DAY_COUNT{set;get;}
        public string ICD_CODE { set; get; }
        public string ICD_NAME { set; get; }
        public string ICD_SUB_CODE { set; get; }
        public string ICD_SUB_TEXT { set; get; }
        public string CONCENTRA { set; get; }
        public string DOCTOR_USERNAME { set; get; }
        public string MEDICINE_TYPE_NAME { set; get; }
        public string MEDICINE_TYPE_CODE { set; get; }
        public string MEDICINE_USE_FROM { set; get; }
        public decimal AMOUNT { set; get; }
        public decimal PRICE { set; get; }
        public decimal TOTAL_PRICE { set; get; }
        public string DEPARTMENT_NAME { set; get; }
        public string DEPARTMENT_CODE { set; get; }
        public string APPROVAL_DATE { set; get; }// ngày kê đơn
        public string BYT_NUM_ORDER { set; get; }
        public string ROOM_NAME { set; get; }
        public string ACTIVE_INGR_BHYT_CODE { set; get; } // tên hoạt chất BHYT
        public string ACTIVE_INGR_BHYT_NAME { set; get; } // tên hoạt chất BHYT
        public string TUTORIAL { set; get; }
        public string MEDICINE_REGISTER_NUMBER { set; get; }
        public string ATC_CODE { set; get; }
        public string SERVICE_UNIT_NAME{set;get;}
        public decimal EXP_TOTAL_AMOUNT { set; get; }
        public decimal IMP_HT_AMOUNT{set;get;}
        public decimal TOTAL_AMOUNT { set; get; }
        public string NATIONAL_NAME { set; get; }
        public decimal TH_AMOUNT { set; get; }
        public string ACTIVE_INGREDIENT_IDs { get; set; }
        public string ACTIVE_INGREDIENT_CODEs { get; set; }
        public string ACTIVE_INGREDIENT_NAMEs { get; set; }
    }
}
