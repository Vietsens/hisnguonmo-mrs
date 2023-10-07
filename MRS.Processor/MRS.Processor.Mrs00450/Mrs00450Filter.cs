using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00450
{
    public class Mrs00450Filter
    {
        public long DATE_TIME { get;  set;  }
        public long MEDI_STOCK_BUSINESS_ID { get;  set;  }
        public List<long> SERVICE_TYPE_IDs { get; set; }
    }
}
