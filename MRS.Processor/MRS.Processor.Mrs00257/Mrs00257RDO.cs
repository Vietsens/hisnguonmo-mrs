using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels; 

namespace MRS.Processor.Mrs00257
{
    public class Mrs00257RDO : V_HIS_BID_MATERIAL_TYPE
    {
        public Decimal END_MEDI_AMOUNT{get; set; }
        public Decimal END_AMOUNT { get;  set;  }
        public Decimal BID_END_AMOUNT { get;  set;  }

        public decimal BID_AMOUNT { get; set; }

        public decimal AMOUNT_COMPLETE { get; set; }

        public decimal AMOUNT_WAITING { get; set; }
    }
}
