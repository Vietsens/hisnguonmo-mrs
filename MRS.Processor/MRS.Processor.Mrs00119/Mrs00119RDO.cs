using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00119
{
    class Mrs00119RDO
    {
        public Mrs00119RDO()
        {
            Amount = 0; 
        }

        public long PatientTypeId { get;  set;  }
        public string PatientTypeName { get; set; }
        public long Amount { get; set; }
        public string treatmentcodes { get; set; }
    }
}
