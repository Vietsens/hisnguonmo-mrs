using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00480
{
    public class Mrs00480RDO
    {
        public V_HIS_TRANSACTION TRANSACTION { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
    }
}
