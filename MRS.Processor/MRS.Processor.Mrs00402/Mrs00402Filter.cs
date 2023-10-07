using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00402
{
    /// <summary>
    /// báo cáo bệnh nhân ngoại trú làm dịch vụ theo nhóm
    /// </summary>
    public class Mrs00402Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }

        public long? MEDI_STOCK_ID { get; set; }

        public List<long> MEDI_STOCK_IDs { get; set; }

        public Mrs00402Filter()
            : base()
        {

        }
    }
}
