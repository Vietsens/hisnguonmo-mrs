using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00710
{
    
    public class Mrs00710Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public string NORMAL_BORN_ICD_CODE { get; set; }//cac ma icd de thuong ngan cach nhau dau ,
        public string DIFFI_BORN_ICD_CODE { get; set; }//cac ma icd de kho ngan cach nhau dau ,
        public string SURG_BORN_ICD_CODE { get; set; }//cac ma icd mo de ngan cach nhau dau ,
        public string EEG_SERVICE_CODE { get; set; }//cac dich vu dien tim cach nhau dau ,
        public string ECG_SERVICE_CODE { get; set; }//cac dich vu dien nao cach nhau dau ,
        public string SCAN_SERVICE_CODE { get; set; }//cac dich vu ct scan cach nhau dau ,
        public string TRADI_SERVICE_CODE { get; set; }//cac dich vu kham yhct cach nhau dau ,
        public short? STATUS_TREATMENT { get; set; }
    }
}
