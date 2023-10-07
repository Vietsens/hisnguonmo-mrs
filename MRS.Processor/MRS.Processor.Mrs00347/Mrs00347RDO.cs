using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00347
{
    public class Mrs00347RDO
    {
        public long NUMBER { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string DOB { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string GENDER { get; set; }
        public string END_DEPARTMENT_CODE { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string IN_TIME { get; set; }
        public string OUT_TIME { get; set; }
        public string WORK_PLACE_NAME { get; set; }
        public string ADDRESS { get; set; }
        public decimal TOTAL_PRICE { get; set; }           // tổng chi phí
        public decimal TOTAL_HEIN_PRICE { get; set; }      // BHYT chi trả
        public decimal TOTAL_PATIENT_PRICE { get; set; }   // tổng số tiền BN phải trả
        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }  // tổng số tiên BN tạm ứng
        public decimal PATIENT_PRICE { get; set; }         // số tiền bệnh nhân trả
        public decimal GIVE_BACK { get; set; }             // số tiền trả lại BN
        public decimal EXPEND_PRICE { get; set; }          // số tiền hao phí

        public decimal TOTAL_BILL_AMOUNT { get; set; }//tổng số tiền thanh toán
        public decimal TOTAL_BILL_FUND { get; set; }//tổng số tiền quỹ thanh toán
        public decimal TOTAL_BILL_OTHER_AMOUNT { get; set; }//tổng số tiền nguồn khác
        public decimal TOTAL_BILL_TRANSFER_AMOUNT { get; set; }//tổng số tiền kết chuyển
        public decimal TOTAL_REPAY_AMOUNT { get; set; }//tổng số tiền hoàn ứng
        public decimal TOTAL_DISCOUNT { get; set; }//tổng số tiền miễn giảm chi tiết dv
        public decimal TOTAL_BILL_EXEMPTION { get; set; }//tổng số tiền miễn giảm trên hóa đơn

        public string BILL_NUM_ORDER { get; set; }//so hoa don

        public short? IS_LOCK_HEIN { get; set; }
        public string NOTE { get; set; }
        public short? IS_ACTIVE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public short? IS_PAUSE { get; set; }
        public long TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }

        public long? END_ROOM_ID { get; set; }

        public string END_ROOM_NAME { get; set; }

        public string END_ROOM_CODE { get; set; }

        public string TDL_PATIENT_PHONE { get; set; }
        public long TDL_PATIENT_TYPE_ID { get; set; }
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }
        public string TREATMENT_STT_NAME { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }

        public string ICD_NAME { get; set; }

        public string ICD_CODE { get; set; }

        public string FEE_LOCK_TIME { get; set; }
    }

    public class Mrs00347RDODepatmentDetail
    {
        public long TREATMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }

        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }  // tổng số tiền BN tạm ứng do khoa yêu cầu
        public decimal TOTAL_PATIENT_PRICE { get; set; }//tổng số tiền BN phải trả
        public decimal GIVE_BACK { get; set; }             // số tiền trả lại BN
        public decimal PATIENT_PRICE { get; set; }          // số tiền bệnh nhân trả
    }
}

