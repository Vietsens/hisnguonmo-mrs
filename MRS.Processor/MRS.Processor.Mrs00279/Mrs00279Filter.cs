using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00279
{
    public class Mrs00279Filter
    {
		 public long BRANCH_ID { get;  set;  }

		 public long? TIME_FROM { get;  set;  }
		 public long? TIME_TO { get;  set;  }

		 /// <summary>
		 /// True: chi lay noi tru
		 /// False: chi lay ngoai tru
		 /// Null: lay ca hai
		 /// </summary>
		 public bool? IS_TREAT { get;  set;  }

				
    }
}
	