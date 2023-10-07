using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;

namespace MRS.Processor.Mrs00314
{
    public class Mrs00314RDO
    {
        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }// ma benh nhan
        public string TRANSACTION_CODE { get; set; }//ma vien phi
        public long REQUEST_DEPARTMENT_ID { get; set; }// khoa yêu cầu
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string END_DEPARTMENT_CODE { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }

        public string PATIENT_NAME { get; set; }
        public string PATIENT_BHYT { get; set; }
        public string ICD_CODE { get; set; }
        public string PATIENT_GENDER { get; set; }
        public long DOB { get; set; }
        public string DOB_YEAR { get; set; }
        public string IS_PATIENT_TYPE_BHYT { get; set; }// có phải là đối tượng bhyt không
        public long? OPEN_TIME { get; set; } // thời gian vào viện
        public long? CLOSE_TIME { get; set; } // thời gian ra viện
        public string OPEN_TIME_STR { get; set; }// thời gian vào viện
        public string CLOSE_TIME_STR { get; set; }// thời gian ra viện
        public decimal TEST_FUEX_PRICE { get; set; } // tiền xét nghiệm, thăm dò chức năng
        public decimal DIIM_PRICE { get; set; } // tiền chẩn đoán hình ảnh
        public decimal ENDO_PRICE { get; set; } // nội soi
        public decimal SUIM_PRICE { get; set; } // siêu âm
        public decimal MEDICINE_PRICE { get; set; } // tiền thuốc
        public decimal BLOOD_PRICE { get; set; } // tiền máu
        public decimal MATERIAL_PRICE { get; set; } // tiền vật tư
        public decimal KTC_PRICE { get; set; }// tiền dịch vụ kĩ thuật cao
        public decimal SURGMISU_PRICE { get; set; }// tiền dịch vụ kĩ thuật thông thường
        public decimal MATERIAL_PRICE_RATIO { get; set; }
        public decimal MEDICINE_PRICE_RATIO { get; set; }
        public decimal SERVICE_PRICE_RATIO { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal DISCOUNT { get; set; }
        public decimal TOTAL_PRICE_100 { get; set; }
        public decimal TOTAL_PRICE_5 { get; set; }
        public decimal TOTAL_PRICE_20 { get; set; }
        public decimal TOTAL_PRICE_0 { get; set; }
        public decimal EXAM_PRICE { get; set; }// tiền công khám
        public decimal OTHER_PRICE { get; set; }// tiền khác
        public decimal TRAN_PRICE { get; set; } // chi phí vận chuyển 
        public decimal AN_PRICE { get; set; } // chi phí vận chuyển 
        public decimal BED_PRICE { get; set; } // tiền giường
        public string DEPARTMENT_NAME { get; set; }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY
        public decimal PATIENT_PRICE_PAY { get; set; }
        public decimal PATIENT_TU { get; set; } //Tạm ứng
        public decimal PATIENT_HU { get; set; } //Hoàn ứng
        public decimal PATIENT_TT { get; set; } //Thanh toan
        public decimal PATIENT_TTK { get; set; } //Thanh toan khac
        public decimal ADD_PRICE { get; set; } //Phụ thu

        public long REQUEST_ROOM_ID { get; set; }// khoa yêu cầu
        public string REQUEST_ROOM_NAME { get; set; }
        

        public string DEPARTMENT_ROOM_NAME { get; set; }

        public Dictionary<string, decimal> DIC_HEIN_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PATIENT_PRICE { get; set; }

        public decimal TOTAL_PATIENT_PRICE_PHI { get; set; }
        public decimal TOTAL_PATIENT_PRICE_YC { get; set; }

        public Dictionary<string, decimal> DIC_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_AMOUNT { get; set; }

        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_PARENT_PATIENT_PRICE { get; set; }
        public decimal? TREATMENT_DAY_COUNT { get; set; }


        public string END_ROOM_NAME { get; set; }

        public string END_EXAM_ROOM_NAME { get; set; }

        public decimal TEST_PRICE { get; set; }

        public decimal FUEX_PRICE { get; set; }
        public string TREATMENT_TYPE_CODE { get;  set; }
        public decimal TOTAL_EXPEND_PRICE { get;  set; }

        public Dictionary<string, decimal> DIC_CATEGORY { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_BHYT { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_DCT { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_BN { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_TT { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_AMOUNT { get; set; }

        public long EXECUTE_DEPARTMENT_ID { get; set; }

        public string EXECUTE_DEPARTMENT_NAME { get; set; }

        public decimal DIFF_PRIMARY_PRICE { get; set; }

        public decimal EXEMPTION { get; set; }

        public decimal MATERIAL_REUSEABLE_PRICE { get; set; }

        public decimal SALE_MEDICINE_PRICE { get; set; }

        public long? TDL_HEIN_CARD_FROM_TIME { get; set; }

        public long? TDL_HEIN_CARD_TO_TIME { get; set; }

        public string PATIENT_ADDRESS { get; set; }

        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }

        public string STORE_CODE { get; set; }

        public string PATIENT_TT_CODE { get; set; }

        public string TDL_PATIENT_TYPE_CODE { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }

        public Dictionary<string, decimal> DIC_TYPE_PRICE { get; set; }
    }
}
