using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 
using System.Reflection; 

namespace MRS.Processor.Mrs00609
{
    public class Mrs00609RDO
    {
        public bool? IS_DELETE { get; set; }//Xóa
        public string TREATMENT_CODE { get; set; }//Mã điều trị
        public string PATIENT_CODE { get; set; }// ma benh nhan
        public string ADDRESS { get; set; }//địa chỉ
        public string HEIN_CARD_NUMBER { get; set; }// Số thẻ
        public string PATIENT_NAME { get; set; }//Tên

        public long? TDL_PATIENT_DOB { get; set; }//ngày sinh

        public string DOB_STR { get; set; }//ngày sinh
        public int? AGE { get;  set;  }//tuổi
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public decimal? TREATMENT_DAY_COUNT { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }//	Đối tượng bệnh nhân
        public long? TDL_TREATMENT_TYPE_ID { get; set; }//Diện điều trị
        public string PATIENT_TYPE_NAME { get; set; }//	Đối tượng bệnh nhân
        public string TREATMENT_TYPE_NAME { get; set; }//	Diện điều trị
        public string DOB_YEAR_STR { get; set; }//	Năm sinh
        public string IN_TIME_STR { get; set; }//Thời gian vào
        public string OUT_TIME_STR { get; set; }//	Thời gian ra
        public string ICD_NAME { get; set; }//	tên bệnh chính
        public string ICD_TEXT { get; set; }//	Tên bệnh phụ
        public decimal? VIR_TOTAL_PRICE { get; set; }//Tổng tiền
	
        public string ICD_CODE { get; set; }//	Mã bệnh chính
        public string ICD_SUB_CODE { get; set; }//	Mã bệnh phụ
        public string REQUEST_LOGINNAME { get; set; }//	Tài khoản yêu cầu
        public string REQUEST_USERNAME { get; set; }//	người yêu cầu
        public string SERVICE_NAME { get; set; }//	Tên dịch vụ
        public string SERVICE_ID { get; set; }//	Tên dịch vụ
        public string SERVICE_CODE { get; set; }//Mã dịch vụ
        public string OTHER_PAY_SOURCE_NAMEs { get; set; }//Tên các nguồn chi trả khác
        public Dictionary<string,decimal> DIC_SVT_TOTAL_PRICE { get; set; }//Mã dịch vụ	
        public Mrs00609RDO()
        {
            DIC_SVT_TOTAL_PRICE = new Dictionary<string, decimal>();
            DIC_CATE_TOTAL_PRICE = new Dictionary<string, decimal>();
        }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_PRICE { get; set; }

        public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }

        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }

        public decimal? VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public decimal? TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_PATIENT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_PATIENT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PATIENT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_PAR_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PATIENT_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PATIENT_PRICE_SELF { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public long IN_TIME { get; set; }

        public long? OUT_TIME { get; set; }

        public long? FEE_LOCK_TIME { get; set; }


        public long TREATMENT_ID { get; set; }

        public string BLOOD_PRESSURE { get; set; }

        public string BMI { get; set; }
    }
}
