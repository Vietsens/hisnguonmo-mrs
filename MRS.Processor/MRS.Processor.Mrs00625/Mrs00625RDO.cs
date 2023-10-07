using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00625
{
    public class Mrs00625RDO
    {

        public long ID { get; set; }
        public long? SSB_ID { get; set; }
        public long? SSD_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long AGE { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string ICD_NAME { get; set; }	
        public string TDL_SERVICE_NAME { get; set; }	
        public long TDL_FINISH_DATE_STR { get; set; }	
        public string PTTT_GROUP_NAME { get; set; }
        public decimal? PACKAGE_PRICE { get; set; }
        public decimal? TOTAL_PRICE_IN_FEE { get; set; }
        public decimal? TOTAL_MEDICINE_PRICE_IN_FEE { get; set; }
        public decimal? TOTAL_MATERIAL_PRICE_IN_FEE { get; set; }
        public decimal? TOTAL_OTHER_PRICE_IN_FEE { get; set; }
        public decimal? TOTAL_PRICE_OUT_FEE { get; set; } 
    }
}
