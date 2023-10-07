using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00229
{
    public class Mrs00229RDO
    {
        public long EXP_MEST_ID { get; set; }
        public long MEDICINE_ID { get; set; }
        public long? TDL_PATIENT_ID { get; set; }
        public long? TDL_TREATMENT_ID { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get;  set;  } 
        public string MEDICINE_TYPE_NAME { get;  set;  } //Ten thuoc
        public string MEDICINE_TYPE_CODE { get;  set;  } //don vi
        public decimal? PRICE { get; set; }  // don gia
        public decimal? AMOUNT { get; set; } // so luong     
        public decimal? TH_AMOUNT { get; set; } // so luong thu hoi     
        public decimal? TOTAL_PRICE { get; set; }  // tong tien   

        public string EXP_MEST_CODE { get; set; }  // ma phieu xuat
        public string TDL_PATIENT_CODE { get; set; }  // ma benh nhan
        public long? TDL_INTRUCTION_DATE { get; set; }  // ngay y lenh
        public string ACTIVE_INGR_BHYT_CODE { get; set; }  // ma hoat chat
        public string ACTIVE_INGR_BHYT_NAME { get; set; }  // ten hoat chat
        public string CONCENTRA { get; set; }  // ham luong
        public string SERVICE_UNIT_NAME { get; set; }  // don vi
        public string MEDICINE_USE_FORM_NAME { get; set; }  // duong dung
        public long? TDL_PATIENT_DOB { get; set; }  // ngay sinh
        public string TUTORIAL { get; set; }  // huong dan su dung
        public string REQUEST_USERNAME { get; set; }  // bac si
        public string ICD_CODE { get; set; }  // ma benh chinh
        public string ICD_NAME { get; set; }  // ten benh chinh
        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public string TDL_HEIN_CARD_NUMBER { get; set; }
    }
}
