using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00587
{
    
    public class Mrs00587Filter
    {
        public long? FEE_LOCK_TIME_FROM { get; set; }
        public long? FEE_LOCK_TIME_TO { get; set; }
        public long? TREATMENT_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
    }
}
