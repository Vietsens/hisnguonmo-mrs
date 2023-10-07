using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00642
{

    public class Mrs00642RDO
    {
        public long TREATMENT_ID { get; set; }

        public string ICD_CODE { get; set; }

        public long DOB { get; set; }

        public long OUT_TIME { get; set; }

        public string CAREER_CODE { get; set; }

        public long GENDER_ID { get; set; }
    }
}
