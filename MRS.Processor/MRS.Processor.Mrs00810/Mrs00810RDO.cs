using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00810
{
    public class Mrs00810RDO
    {
        public long PATIENT_ID { set; get; }
        public string PATIENT_NAME { set; get; }
        public string PATIENT_CODE { set; get; }
        public string DOB { set; get; }
        public string GENDER { set; get; }
        public string HEIN_CARD_NUMBER { set; get; }
        public long IN_DATE { set; get; } // ngày khám
        public string ICD_CODE { set; get; }
        public decimal TOTAL_PRICE { set; get; }
        public decimal XN_PRICE { set; get; }
        public decimal CDHA_PRICE { set; get; }
        public decimal MEDI_PRICE { set; get; }
        public decimal BLOOD_PRICE { set; get; }
        public decimal DVKT_TT_PRICE{ set; get; }
        public decimal DVKT_NC_PRICE { set; get; }
        public decimal VTTHYT_PRICE { set; get; }
        public decimal CPVC_PRICE { set; get; }
        public string HEIN_ORG_NAME { set; get; }
        public decimal TREATMENT_PRICE { set; get; }
        public decimal TOTAL_PRICE_ARV { set; get; }
        public decimal SERVICE_PRICE_RATIO { set; get; }
        public decimal SERVICE_PRICE_RATIO_NC { set; get; }
        public decimal TOTAL_PATIENT_PRICE { set; get; }
        public decimal HEIN_RATIO { set; get; }//% bhyt trả
        public decimal HEIN_PRICE { set; get; }// nhà nước trả
        public decimal OTHER_SOURCE_PRICE { set; get; }
        public string END_CODE { set; get; }// so ra vien
    }
}