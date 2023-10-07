using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00302
{
    public class Mrs00302RDO
    {
        public string TREATMENT_CODE1 { get;  set;  }
        public string TREATMENT_CODE2 { get;  set;  }
        public string TREATMENT_CODE3 { get;  set;  }
        public string TREATMENT_CODE4 { get;  set;  }
        public string TREATMENT_CODE5 { get;  set;  }
        public string TREATMENT_CODE6 { get;  set;  }

        public long TREATMENT_CODE_NUMBER1 { get;  set;  }
        public long TREATMENT_CODE_NUMBER2 { get;  set;  }
        public long TREATMENT_CODE_NUMBER3 { get;  set;  }
        public long TREATMENT_CODE_NUMBER4 { get;  set;  }
        public long TREATMENT_CODE_NUMBER5 { get;  set;  }
        public long TREATMENT_CODE_NUMBER6 { get;  set;  }
    }
}
