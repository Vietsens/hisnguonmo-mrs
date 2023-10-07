using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00275
{
    public class Mrs00275RDO
    {

		 public Decimal PRICE { get;  set;  }
		 public Decimal AMOUNT { get;  set;  }
		 public Decimal TOTAL_PRICE { get;  set;  }
		 public string MAME_TYPE_NAME { get;  set;  }
		 public string PARENT_NAME { get;  set;  }
		 public string SERVICE_UNIT_NAME { get;  set;  }
		 public string NATIONAL_NAME { get;  set;  }
		 public long PARENT_ID { get;  set;  }

         public string MAME_TYPE_CODE { get; set; }

         public string PACKAGE_NUMBER { get; set; }
    }
}
