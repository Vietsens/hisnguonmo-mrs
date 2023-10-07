using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00249
{
    public class Mrs00249RDO : V_HIS_TRANSACTION
    {
        public long TRANSACTION_ID { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public Decimal DEP_BIL { get; set; }
        public Decimal REPAYs { get; set; }
        public Decimal KC_AMOUNTs { get; set; }
        public string NUM_ORDER_STR { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string VIR_PATIENT_CODE { get; set; }
        public Decimal REDIASUAL_AMOUNT { get; set; }
        public Decimal TOTAL_DIFFERENCE_PRICE { get; set; }
        public Decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public Decimal TOTAL_PATIENT_PRICE_VP { get; set; }
        public Decimal SS_DISCOUNT { get; set; }
        public Decimal AMOUNT_NO { get; set; }
        public string LAST_DEPARTMENT_NAME { get; set; }
        public long? LAST_DEPARTMENT_ID { get; set; }
        public bool IS_DEPOSIT_DETAIL { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public short IS_ON_TIME { get; set; }
        public Dictionary<string, decimal> DIC_SVT_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_HSVT_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_SVT_HEIN_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_HSVT_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE_IN_BILL { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public decimal TOTAL_PRICE_GD12H { set; get; }
        public long? IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public long? TREATMENT_DAY_COUNT { get; set; }
        public Mrs00249RDO()
        {

        }



        public decimal SSD_DISCOUNT { get; set; }

        public string LAST_MEDI_STOCK_NAME { get; set; }

        public string KSK_CONTRACT_CODE { get; set; }

        public string WORK_PLACE_NAME { get; set; }

        public string REQUEST_DEPARTMENT_CODE { get; set; }

        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public string CARD_NUMBER { get; set; }

        public string CARD_CODE { get; set; }

        public string CARD_MAC { get; set; }

        public string BANK_CARD_CODE { get; set; }

        public string HEIN_CARD_NUMBER { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public decimal TOTAL_HEIN_PRICE { get; set; }

        public decimal TOTAL_TREATMENT_DEPO_AMOUNT { get; set; }

        public decimal TOTAL_TREATMENT_REP_AMOUNT { get; set; }
        public decimal CHI_RA { get; set; }
        public decimal THU_THEM { get; set; }
        public decimal HIEN_DU { get; set; }
        public decimal CAN_TRU { get;  set; }

        public decimal CANCEL_AMOUNT { get; set; }

        public decimal? CANCEL_KC_AMOUNT { get; set; }

        public decimal CANCEL_KC_AMOUNTs { get; set; }

        public decimal? CANCEL_EXEMPTION { get; set; }

        public decimal? CANCEL_TDL_BILL_FUND_AMOUNT { get; set; }

        public decimal? EXTRA_AMOUNT { get; set; }
        public long CANCEL { get; set; }

        public int TRANSACTION_COUNT { get; set; }
        public decimal? TOTAL_PRICE_AN { set; get; }

        public string CLINICAL_DEPARTMENT_NAME { get; set; }

        public long CLINICAL_DEPARTMENT_ID { get; set; }

        public string CLINICAL_DEPARTMENT_CODE { get; set; }

        public long ADD_MIN_TIME { get; set; }

        public long SUB_MIN_TIME { get; set; }
        
    }
    public class TREATMENT_FEE
    {
        public long ID { get; set; }
        public decimal? TOTAL_HEIN_PRICE { get; set; }
        public decimal? TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal? TOTAL_PATIENT_PRICE { get; set; }
        public decimal? TOTAL_DISCOUNT { get; set; }
        public decimal? TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public decimal? TOTAL_DEPOSIT_AMOUNT { get; set; }
        public decimal? TOTAL_REPAY_AMOUNT { get; set; }


        public long? HOSPITALIZE_DEPARTMENT_ID { get; set; }
    }

    public class HISTORY_TIME
    {
        public long TRANSACTION_ID { get; set; }
        public long TRANSACTION_TIME { get; set; }
    }

    public class TYPE_PRICE
    {
        public long BILL_ID { get; set; }
        public Decimal? TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public Decimal? TOTAL_DIFFERENCE_PRICE { get; set; }
        public Decimal? TOTAL_PATIENT_PRICE_VP { get; set; }
        public Decimal? SS_DISCOUNT { get; set; }
        public Decimal? TOTAL_OTHER_SOURCE_PRICE { get; set; }

    }

    public class REQUEST_DEPARTMENT_ID
    {
        public long TRAN_ID { get; set; }
        public long? REQ_ID { get; set; }
        
    }

    public class LAST_ALTER_INFO
    {
        public long TRANSACTION_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long? KSK_CONTRACT_ID { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        
    }

    public class CARD_INFO
    {
        public long TRANSACTION_ID { get; set; }
        public string BANK_CARD_CODE { get; set; }
        public string CARD_CODE { get; set; }
        public string CARD_MAC { get; set; }
        public string CARD_NUMBER { get; set; }
        
    }

    public class SSB
    {
        public long TRANSACTION_ID { get; set; }
        public long BILL_ID { get; set; }
        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public long TDL_EXECUTE_ROOM_ID { get; set; }
        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string REQUEST_ROOM_CODE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string HEIN_SERVICE_TYPE_CODE { get; set; }
        public long? PARENT_ID { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public long? PARENT_NUM_ORDER { get; set; }
        public string CATEGORY_CODE { get; set; }
        public decimal PRICE { get; set; }
        public long TDL_PATIENT_TYPE_ID { get; set; }
        public decimal? TDL_TOTAL_PRICE { get; set; }
        public decimal? TDL_TOTAL_PATIENT_PRICE { get; set; }
        public decimal? TDL_TOTAL_PATIENT_PRICE_BHYT { get; set; }
        public decimal? TDL_TOTAL_HEIN_PRICE { get; set; }
        public decimal? TOTAL_PRICE_SV { get; set; }
        public string SERVICE_NAME { set; get; }
    }
    public class ParamThread
    {
        public List<Mrs00249RDO> transactionSub { get; set; }
        public List<SSB> ssbSub { get; set; }
        public Dictionary<string, DepartmentRoomBill> dicDepaRoomBill { get; set; }
        public List<long> listRepaySameDate { get; set; }
    }

    public class DepartmentRoomBill
    {
        public Mrs00249RDO mainRdo { get; set; }
        public long TRANSACTION_ID { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public long TDL_REQUEST_ROOM_ID { get; set; }
        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_ROOM_CODE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public Dictionary<string, decimal> DIC_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_PRICE_DIFF { get; set; }
        public Dictionary<string, decimal> DIC_TOTAL_HEIN_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_SERVICE_TYPE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATEGORY { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_BHYT { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_DCT { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_BN { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_TT { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_DIFF { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public decimal TOTAL_PRICE_BHYT { get; set; }
        public decimal TOTAL_PRICE_DCT { get; set; }
        public decimal TOTAL_PRICE_BN { get; set; }
        public decimal TOTAL_PRICE_TT { get; set; }
        public decimal TOTAL_PRICE_DIFF { get; set; }
        public decimal? TOTAL_PRICE_GD12H { set; get; }//Gói giảm đau theo yêu cầu 12h đầu sau phẫu thuật
        public decimal? TOTAL_PRICE_AN { set; get; }
        public string NOTE_PRICE { get; set; }

        public string NOTE_PRICE_DIFF { get; set; }

        public Dictionary<string, decimal> DIC_PRICE_DIFF_NR { get; set; }

        public decimal TOTAL_PRICE_DIFF_R { get; set; }
    }
}
