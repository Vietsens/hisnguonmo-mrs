using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00607
{
    class Mrs00607RDO
    {
        public string TREATMENT_CODE { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string JOB { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string TDL_PATIENT_PHONE { get; set; }
        public string GIOITHIEU { get; set; }//Nơi khám bệnh ban đầu
        public string DATE_IN_STR { get; set; }
        public long IN_TIME { get; set; }
        public string DATE_TRIP_STR { get; set; }//Ngày ra
        public string DATE_OUT_STR { get; set; }
        public string DATE_DEAD_STR { get; set; }
        public string DIAGNOSE_TUYENDUOI { get; set; }
        public string DIAGNOSE_KKB { get; set; }
        public string DIAGNOSE_KDT { get; set; }
        public string DIAGNOSE_KGBP { get; set; }

        public string ICD_CODE_TUYENDUOI { get; set; }
        public string ICD_CODE_KKB { get; set; }
        public string ICD_CODE_KDT { get; set; }
        public string ICD_CODE_KGBP { get; set; }

        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public decimal? MALE_AGE { get; set; }
        public decimal? FEMALE_AGE { get; set; }
        public decimal TOTAL_DATE_TREATMENT { get; set; }
        public string DOB_STR { get; set; }

        public string IS_OFFICER { get; set; }
        public string IS_BHYT { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string IS_CITY { get; set; }
        public string IS_COUNTRYSIDE { get; set; }
        public string IS_DUOI_12THANG { get; set; }
        public string IS_1DEN15TUOI { get; set; }
        public string IS_DEAD_IN_24H { get; set; }
        public string IS_CURED { get; set; }
        public string IS_ABATEMENT { get; set; }
        public string IS_AGGRAVATION { get; set; }
        public string IS_UNCHANGED { get; set; }

        public decimal TOTAL_PRICE { set; get; }
        public decimal TOTAL_PATIENT_PRICE { set; get; }

        public string MEDI_MATE_TYPE_CODE { get; set; }

        public string MEDI_MATE_TYPE_NAME { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public decimal AMOUNT { get; set; }

        public decimal PRICE { get; set; }

        public decimal VAT { get; set; }

        public decimal TOTAL_MEDI_MATE_PRICE { get; set; }

        public string TYPE { get; set; }

        public string CONCENTRA { get; set; }
    }
}
