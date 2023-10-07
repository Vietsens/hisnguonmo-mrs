using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00589
{
    public class Mrs00589RDO
    {
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string TDL_HEIN_CARD_NUMBER { get; set; }
        public string TDL_PATIENT_CAREER_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_RESULT_NAME { get; set; }
        public long AGE { get; set; }
        public string ACCIDENT_HURT_TYPE_CODE { get; set; }//Nguyên nhân
        public string ACCIDENT_BODY_PART_CODE { get; set; }//Bộ phận thương tích
        public string ACCIDENT_BODY_PART_NAME { get; set; }//Bộ phận thương tích
        public string ACCIDENT_HELMET_CODE { get; set; }//mũ bảo hiểm
        public string ACCIDENT_RESULT_CODE { get; set; }//Diễn biến sau tai nạn
        public string ACCIDENT_CARE_CODE { get; set; }//Xử lí sau tai nạn
        public string IS_USE_ALCOHOL { get; set; }//Sử dụng rượu bia
        public string TRANSPORT_VEHICLE { get; set; } // phương tiện chuyển đến
        public string AGE_STR { get; set; }
        public string IS_MALE { get; set; }
        public string IS_FEMALE { get; set; }
        public long ACCIDENT_TIME { get; set; }
        public string ACCIDENT_RESULT_NAME { get; set; }
        public string ACCIDENT_TIME_STR { get; set; }
        public string TRANSFER_IN_MEDI_ORG_CODE { get; set; }
        public string IS_TRANSFER_IN_MEDI_ORG_CODE { get; set; }
        public string LEVEL_CODE { get; set; }

        public string ACCIDENT_HURT_TYPE_NAME { get; set; }//Loại tai nạn

        public long IN_TIME { get; set; }//nhập viện
        public long? CLINICAL_IN_TIME { get; set; }
        public decimal? TREATMENT_DAY_COUNT { get; set; }

        public string ICD_CODE { get; set; }//Mã chẩn đoán chính

        public string ICD_NAME { get; set; }//tên chẩn đoán chính

        public string ICD_CAUSE_CODE { get; set; }//Nguyên nhân ngoài

        public string ICD_CAUSE_NAME { get; set; }//Nguyên nhân ngoài

        public short? USE_ALCOHOL { get; set; }

        public string ACCIDENT_LOCATION_CODE { get; set; }

        public string ACCIDENT_POISON_CODE { get; set; }

        public short? ALCOHOL_TEST_RESULT { get; set; }

        public short? NARCOTICS_TEST_RESULT { get; set; }
    }
    public class HIS_TREATMENT_DEATH
    {
        public long TREATMENT_ID { get; set; }
        public string DEATH_CAUSE_CODE { get; set; }
    }
}
