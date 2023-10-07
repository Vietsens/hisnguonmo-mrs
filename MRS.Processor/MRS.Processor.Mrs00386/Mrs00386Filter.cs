using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00386
{
    public class Mrs00386Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? BRANCH_ID { get; set; }
        public bool? IS_EXTRA { get; set; }
        public string PATIENT_TYPE_CODE__KSK { get; set; }

        public bool? IS_PARENT_FINISH_TIME { get; set; }

        public bool? IS_COUNT_EXAM { get; set; }
    }
}
	