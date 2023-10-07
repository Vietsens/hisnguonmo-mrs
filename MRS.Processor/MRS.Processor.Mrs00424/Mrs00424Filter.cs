using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00424
{
    public class Mrs00424Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? MEDI_STOCK_ID { get;  set;  }
        public List<long> EXP_MEST_TYPE_IDs { get;  set;  }
        public long? DEPARTMENT_ID { get;  set; }
        public short? INPUT_DATA_ID_GROUP_TYPE { get; set; } //ki?u g?p: 1:G?p theo giá bán; 2: G?p theo giá nh?p
    }
}
