using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;

namespace MRS.Processor.Mrs00644
{
    public class Mrs00644RDO
    {
        public string TREATMENT_CODE { get; set; }
        public long END_DEPARTMENT_ID { get; set; }
        public string END_DEPARTMENT_CODE { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }

        public long DOB { get; set; }
        public string DOB_YEAR { get; set; }
        public long IN_TIME { get; set; } // thời gian vào viện
        public long OUT_TIME { get; set; } // thời gian ra viện
        public string IN_TIME_STR { get; set; }// thời gian vào viện
        public string OUT_TIME_STR { get; set; }// thời gian ra viện
        public long CREATE_TIME { get; set; } //tạo
        public string CREATE_TIME_STR { get; set; }//tạo
        public string DEPARTMENT_NAME { get; set; }//Lấy từ bảng V_HIS_TREATMENT_SUMMARY

        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string TEMPLATE_CODE { get; set; }
        public string SYMBOL_CODE { get; set; }
        public string VIR_NUM_ORDER { get; set; }

        public decimal VIR_TOTAL_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal OXY_PRICE { get; set; }
        public decimal KSK_EXAM_PRICE { get; set; }
        public decimal GDTT_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal SUIM_PRICE { get; set; }
        public decimal ECG_PRICE { get; set; }
        public decimal EEG_PRICE { get; set; }
        public decimal ENDO_PRICE { get; set; }
        public decimal CT_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal KTC_PRICE { get; set; }
        public decimal KTG_PRICE { get; set; }
        public decimal DD_PRICE { get; set; }
        public decimal OTHER_PRICE { get; set; }
        public decimal TOTAL_PRICE_30 { get; set; }
        public decimal TOTAL_PRICE_20 { get; set; }
        public decimal TOTAL_PRICE_5 { get; set; }
        public decimal TOTAL_PRICE_OTHER { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal VCH_PRICE { get; set; }
        public decimal GTGT { get; set; } 	
        //public string NOTE { get;  set;  } 

    }
    public class SAR_PRINT_LOG_D
    {
        public string UNIQUE_CODE { get; set; }
        public long NUM_ORDER { get; set; }
    }
    public class HIS_INVOICE_CANCEL
    {
        public string VIR_UNIQUE { get; set; }
        public string VIR_NUM_ORDER { get; set; }
        public long? CANCEL_TIME { get; set; }
        public string CANCEL_REASON { get; set; }
        public string TEMPLATE_CODE { get; set; }
        public string TREATMENT_CODE { get; set; }
        public short? IS_CANCEL { get; set; }
        public long? INVOICE_TIME { get; set; }
        public long NUM_ORDER { get; set; }
        public string CREATOR { get; set; }
        public long INVOICE_BOOK_ID { get; set; }
        public string SYMBOL_CODE { get; set; }

        public long PRINT_COUNT { get; set; }
        public string CANCEL_LOGINNAME { get; set; }
        public string CANCEL_USERNAME { get; set; }
        public long? REUSE_ID { get; set; }

        public long ID { get; set; }
    }
}
