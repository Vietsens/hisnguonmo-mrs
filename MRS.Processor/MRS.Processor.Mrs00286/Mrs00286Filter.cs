using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00286
{
    public class Mrs00286Filter
    {
				 public long TIME_FROM { get;  set;  }
				 public long TIME_TO { get;  set;  }
				 public List<long> TREATMENT_TYPE_IDs { get;  set;  }
				 public long? PATIENT_TYPE_ID { get;  set;  }
				 public string LOGINNAME { get;  set;  }
				 public long? DEPARTMENT_ID { get;  set;  }
				 public long? SERVICE_REQ_TYPE_ID { get;  set;  }

				
    }
}
	