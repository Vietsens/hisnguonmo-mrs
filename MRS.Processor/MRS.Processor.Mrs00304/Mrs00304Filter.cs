using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00304
{
    public class Mrs00304Filter
    {
				 public long TIME_FROM { get;  set;  }
				 public long TIME_TO { get;  set;  }
                 public List<long> MEDI_STOCK_BUSINESS_IDs { get; set; }
                 public string LOGINNAME_SALE { get; set; }
                 public string LOGINNAME { get; set; }
                 public string CREATOR_LOGINNAME { get; set; }

                 public string CASHIER_LOGINNAME { get; set; }
    }
}
	