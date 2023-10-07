using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00720
{
    class Mrs00720RDO
    {
        public long PATIENT_ID { get; set; }
        public string PATIENT_CODE { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string BILL_SYMBOL { get; set; }
        public string BILL_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string PATIENT_DOB_YEAR { get; set; }
        public string PATIENT_GENDER { get; set; }
        public string PATIENT_ADDRESS { get; set; }
        //public string NATIONAL { get; set; }
        public string MEDI_ORG_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string ICD_CODE { get; set; }
        public string IN_TIME { get; set; }
        public string OUT_TIME { get; set; }
        public decimal TOTAL_DAY_IN_TREATMENT { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPOSIT_NUMBER_SYMBOL { get; set; }
        public string NUM_ORDER { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public decimal DEPOSIT_AMOUNT { get; set; }
        public decimal KC_AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal PR_SERVICE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal TTPT_PRICE { get; set; }
        public decimal VTYT_PRICE { get; set; }
        public decimal CDHA_PRICE { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal TRANSFER_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal OTHER_PRICE { get; set; }

        //public decimal VIR_TOTAL_PRICE { get; set; }
        //public decimal TOTAL_REAL_PRICE { get; set; }
        public string CASHIER_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public long TRANSACTION_TYPE { get; set; }
        public decimal TT_AMOUNT { get; set; }
        public long in_date { get; set; }
        public long out_date { get; set; }

        public decimal CHI_RA { get; set; }
        public decimal THU_THEM { get; set; }

        public decimal DA_THU_TRUOC { get; set; }

        public string PR_SERVICE_CODE { get; set; }
        public string PR_SERVICE_NAME { get; set; }
        public Dictionary<string, decimal> DIC_PARENT_PRICE { get;  set; }
        public Dictionary<string, decimal> DIC_PARENT_COUNT { get; set; }
        public Dictionary<string, decimal> DIC_SERVICE_TYPE_PRICE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }

        public string SERVICE_REQ_CODE { get; set; }

        public decimal PRICE { get; set; }

        public string IN_CODE { get; set; }

        public string FEMALE_DOB { get; set; }

        public string MALE_DOB { get; set; }

        public string PATIENT_NATIONALITY { get; set; }

        public string MEDI_ORG_CODE { get; set; }

        public string ICD_NAME { get; set; }

        public string ICD_SUB_CODE { get; set; }

        public string ICD_TEXT { get; set; }

        public string SERVICE_REQUEST_CODEs { get; set; }

        public string TREATMENT_DAY_COUNT { get; set; }

        public string SERVICE_REQUEST_CODE { get; set; }

        public Dictionary<string, decimal> DIC_SERVICE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_PRICE { get; set; }

        public decimal REPAY_AMOUNT { get; set; }
        public long TRANSACTION_TIME { get; set; }

        public string TRANSACTION_TIME_STR { get; set; }

        public decimal GPBL_PRICE { get; set; }

        public decimal FOOD_PRICE { get; set; }

        public decimal SIEUAM_PRICE { get; set; }

        public decimal NOISOI_PRICE { get; set; }

        public decimal TAKECARE_PRICE { get; set; }

        public decimal TOTAL_TREATMENT_DEPO_AMOUNT { get; set; }

        public decimal ADD_PRICE { get; set; }

        public decimal TDCN_PRICE { get; set; }

        public decimal XHH_PRICE { get; set; }

        public string REQUEST_ROOM_NAME { get; set; }

        public string REQUEST_ROOM_CODE { get; set; }

        public decimal MEDICINE_PRICE_NDM { get; set; }

        public decimal VTYT_PRICE_NDM { get; set; }

        //public short? IS_TRANSACTION_CANCEL { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Mrs00720RDO() {

            DIC_PARENT_PRICE = new Dictionary<string, decimal>();
            DIC_BILL_GOODS_PRICE = new Dictionary<string, decimal>();
            DIC_CATE_PRICE = new Dictionary<string, decimal>();
            DIC_PARENT_COUNT = new Dictionary<string, decimal>();
            DIC_SERVICE_TYPE_PRICE = new Dictionary<string, decimal>();
            DIC_SERVICE_PRICE = new Dictionary<string, decimal>();
        }

        public decimal SV_PRICE_NDM { get; set; }

        public decimal PACKAGE_PRICE { get; set; }

        public decimal SALE_SERVICE_PRICE { get; set; }

        public string PATIENT_CLASSIFY_CODE { get; set; }

        public string PATIENT_CLASSIFY_NAME { get; set; }

        public string CASHIER_ROOM_CODE { get; set; }

        public string CASHIER_ROOM_NAME { get; set; }

        public string TRANSACTION_TIME_STR1 { get; set; }

        public decimal TEST_TOTAL_PRICE { get; set; }

        public decimal CDHA_TOTAL_PRICE { get; set; }

        public decimal MEDICINE_TOTAL_PRICE { get; set; }

        public decimal BLOOD_TOTAL_PRICE { get; set; }

        public decimal TTPT_TOTAL_PRICE { get; set; }

        public decimal VTYT_TOTAL_PRICE { get; set; }

        public decimal EXAM_TOTAL_PRICE { get; set; }

        public decimal BED_TOTAL_PRICE { get; set; }

        public decimal GPBL_TOTAL_PRICE { get; set; }

        public decimal TRANSFER_TOTAL_PRICE { get; set; }

        public decimal MEDICINE_TOTAL_PRICE_NDM { get; set; }

        public decimal VTYT_TOTAL_PRICE_NDM { get; set; }

        public decimal SV_TOTAL_PRICE_NDM { get; set; }

        public decimal FOOD_TOTAL_PRICE { get; set; }

        public decimal SIEUAM_TOTAL_PRICE { get; set; }

        public decimal NOISOI_TOTAL_PRICE { get; set; }

        public decimal TAKECARE_TOTAL_PRICE { get; set; }

        public decimal XHH_TOTAL_PRICE { get; set; }

        public decimal PACKAGE_TOTAL_PRICE { get; set; }

        public decimal OTHER_TOTAL_PRICE { get; set; }

        public string REPAY_NUM_ORDER { get; set; }

        public Dictionary<string, decimal> DIC_BILL_GOODS_PRICE { get; set; }
    }

    //public class TREATMENT_FEE
    //{
    //    public long ID { get; set; }
    //    public decimal? TOTAL_HEIN_PRICE { get; set; }
    //    public decimal? TOTAL_PATIENT_PRICE_BHYT { get; set; }
    //    public decimal? TOTAL_PATIENT_PRICE { get; set; }
    //    public decimal? TOTAL_DISCOUNT { get; set; }
    //    public decimal? TOTAL_OTHER_SOURCE_PRICE { get; set; }
    //    public decimal? TOTAL_DEPOSIT_AMOUNT { get; set; }
    //    public decimal? TOTAL_REPAY_AMOUNT { get; set; }

    //}

    public class PATIENT_CLASSIFY
    {
        public long ID { get; set; }
        public string PATIENT_CLASSIFY_CODE { get; set; }
        public string PATIENT_CLASSIFY_NAME { get; set; }

    }

    public class CASHIER_USER
    {
        
        public string LOGINNAME { get; set; }
        public string USERNAME { get; set; }

    }
}
