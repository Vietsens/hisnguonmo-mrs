using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00711
{
    
    public class Mrs00711Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public List<long> DISTRICT_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public string PREFIX_ICD_CODE__DTTTs { get; set; }//cac ma icd duc thuy tinh the,
        public string PREFIX_ICD_CODE__MOs { get; set; }//cac ma icd mong mat ,
        public string PREFIX_ICD_CODE__QUs { get; set; }//cac ma icd quam ,
        public string PREFIX_ICD_CODE__GLs { get; set; }//cac ma icd glocom ,
    }
}
