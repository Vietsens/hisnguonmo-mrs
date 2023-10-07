using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00261
{
    public class Mrs00261RDO:V_HIS_INVOICE
    {
       public string INVOICE_TIME_STR { get;  set;  }	
        public Decimal? VIR_TOTAL_PRICE { get;  set;  }	

    }
}
