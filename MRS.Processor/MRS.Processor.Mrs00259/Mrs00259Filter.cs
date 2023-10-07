using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00259
{
    /// <summary>
    /// Báo cáo sổ khám bệnh RAE
    /// </summary>
    public class Mrs00259Filter : FilterBase
    {
        public long? MEDI_STOCK_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public string CATEGORY_CODE__KHC { get; set; }
        public string CATEGORY_CODE__CPK { get; set; }

        public string KEY_GROUP_INV { get; set; }
    }
}
