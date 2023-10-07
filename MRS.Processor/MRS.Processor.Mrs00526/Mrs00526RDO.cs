using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00526
{
    class Mrs00526RDO
    {
        public bool HAS_PAY { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public long REQUEST_DEPARTMENT_ID { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public long TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { get; set; }

        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string DATE_IN_STR { get; set; }
        public string DATE_OUT_STR { get; set; }

        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }

        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_HEIN_LIMIT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PATIENT_PRICE { get; set; }//Bệnh nhân cùng trả
        public decimal TOTAL_FEE_PATIENT_PRICE { get; set; }//Tiền Ngoài Danh Mục
        public decimal TOTAL_DIFFERENCE_PRICE { get; set; }//Tiền chênh lệch
        public decimal TOTAL_HEIN_PAY_YOURSELF_PRICE { get; set; }//Tiền Bệnh nhân tự trả
        public decimal VIR_TOTAL_PATIENT_PRICE { get; set; }//Tổng thu bệnh nhân
        public decimal TOTAL_BILL_EXAM_AMOUNT { get; set; }//Đã t.toán ngoại trú: = tạm ứng dịch vụ - hoàn ứng nội trú (lý do hoàn ngoại trú) - hoàn ứng dịch vụ
        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }//Tạm thu(Tạm ứng) :  lấy giao dịch tạm ứng nội trú - số tiền đã hoàn ứng nội trú (lý do hoàn lại tiền tạm ứng)
        public decimal TOTAL_DEPOSIT_SERE_AMOUNT { get; set; }
        public decimal TOTAL_PATIENT_AMOUNT { get; set; }//Tiền BN phải nộp (số tiền nộp thêm)
        public decimal TOTAL_WITHDRAW_AMOUNT { get; set; }//Tiền BN trả lại: số tiền của giao dịch hoàn ứng nội trú, lý do hoàn ứng ra viện
        public decimal TOTAL_BILL_FUND { get; set; }//Quỹ chi trả
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public long? LOG_TIME_EXAM { get; set; }

        public Mrs00526RDO() { }

        public Mrs00526RDO(V_HIS_TREATMENT_FEE treatment)
        {
            if (treatment != null)
            {
                this.TREATMENT_ID = treatment.ID;
                this.TREATMENT_CODE = treatment.TREATMENT_CODE;
                this.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                this.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                this.IN_TIME = treatment.IN_TIME;
                this.OUT_TIME = treatment.OUT_TIME;
                this.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                this.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                this.DEPARTMENT_ID = treatment.END_DEPARTMENT_ID ?? 0;
                this.DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                this.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            }
        }
    }

    public class REQUEST_DEPARTMENT_ID
    {
        public long TRAN_ID { get; set; }
        public long? REQ_ID { get; set; }
    }
}
