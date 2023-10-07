using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00478
{
    public class Mrs00478Filter
    {
        public long TIME_FROM { get;  set;  }                 // thời gian thực hiện
        public long TIME_TO { get;  set;  }

        public long? EXECUTE_DEPARTMENT_ID { get;  set;  }     // khoa thực hiện
        public List<long> MEDI_STOCK_IDs { get;  set;  }      // kho xuất
            
    }
}
