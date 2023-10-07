using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00544
{
    public class Mrs00544Filter
    {
        public long? BRANCH_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }


        public long? SERVICE_TYPE_ID { get; set; }
        public long? SERVICE_ID { get; set; }
        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get;  set;  }

        public List<long> BRANCH_IDs { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public long? REQUEST_DEPARTMENT_ID { get; set; }

        public bool? IS_ROUTE { get; set; }
    }
}
