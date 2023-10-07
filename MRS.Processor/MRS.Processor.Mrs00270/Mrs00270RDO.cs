using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00270
{
    class Mrs00270RDO
    {
        public Mrs00270RDO()
        {
            Amount = 0; 
        }

        public long PatientTypeId { get;  set;  }
        public string PatientTypeName { get;  set;  }
        public long Amount { get;  set;  }
    }
}
