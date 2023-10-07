using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00707
{
    
    public class Mrs00707Filter
    {
        public bool? IS_PRINT { get; set; }
        public bool? IS_PRINT_NEW { get; set; }
        public bool? IS_CANCEL { get; set; }
        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }
        public List<long> MEDI_STOCK_BUSINESS_IDs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

    }
}
