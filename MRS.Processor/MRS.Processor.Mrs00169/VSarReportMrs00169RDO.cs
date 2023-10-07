using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00169
{
    public class VSarReportMrs00169RDO
    {
        public HIS_TREATMENT HIS_TREATMENT { get; set; }
        public string TRANSFER_IN_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string DOB_YEAR { get; set; }
        public string ADDRESS { get; set; }

        public string IN_TIME { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }

        public string MEDI_ORG_NAME { get; set; }
        public string MEDI_ORG_CODE { get; set; }
        public long AMOUNT_COUNT { get; set; }
        public string ICD_NAME { get; set; }
        public string OUT_ICD_NAME { get; set; }

        public string EMERGENCY { get; set; }
        public string TREATMENT_METHOD { get; set; }
        public string OUT_TIME { get; set; }

        public string END_DEPARTMENT_NAME { get; set; }

        public int? AGE_MALE { get; set; }
        public int? AGE_FEMALE { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public string FORM_1a { get; set; }//Chuyển người bệnh từ tuyến dưới lên tuyến trên liền kề (theo trình tự)
        public string FORM_1b { get; set; }//Chuyển người bệnh từ tuyến dưới lên tuyến trên không qua tuyến liền kề (không theo trình tự)
        public string FORM_2 { get; set; }//Chuyển người bệnh từ tuyến trên về tuyến dưới
        public string FORM_3 { get; set; }//Chuyển người bệnh giữa các cơ sở khám bệnh, chữa bệnh trong cùng tuyến
        public string REASON_4 { get; set; }//Chuyển người bệnh đi các tuyến khi đủ điều kiện
        public string REASON_5 { get; set; }//Chuyển theo yêu cầu của người bệnh hoặc đại diện hợp pháp của người bệnh
        public string RESULT_6 { get; set; }//Tình trạng bệnh thuyên giảm, tiến triển tốt, ra viện
        public string RESULT_7 { get; set; }//Tình trạng bệnh không thuyên giảm, nặng lên
        public string RESULT_8 { get; set; }//Tử vong
        public string RESULT_9 { get; set; }//Tuyến trên chuyển về cơ sở KBCB nơi gửi NB để tiếp tục điều trị

        public string PREFIX_TF_ICD_CODE { get; set; }
        public int ICD_COUNT { get; set; }

        public string TRANSFER_IN_ICD_CODE { get; set; }

        public string TRANSFER_IN_ICD_NAME { get; set; }

        public string ICD_CODE { get; set; }

        public string OUT_ICD_CODE { get; set; }
        public string ROOM_NAME { set; get; }
        public string  PATIENT_DOB_STR { get; set; }
        public string TRAN_PATI_FORM_NAME { get; set; }// hình thức chuyển
        public string TRAN_PATI_TECH_NAME { get; set; }// lý do chuyển
        public string TRAN_PATI_REASON_NAME { get; set; }// lý do chuyển
        public string EXIT_DEPARTMENT_NAME { get; set; }
        public string RIGHT_ROUTE_NAME { get; set; }// loại chuyển tuyến
        public string TRANSFER_IN_NAME { get; set; }
        public string RIGHT_ROUTE_TYPE_NAME { get; set; }

        public string TREATMENT_RESULT_NAME { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
    }
}
