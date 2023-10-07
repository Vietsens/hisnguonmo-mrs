using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00327
{
    /// <summary>
    /// báo cáo xuất nhập chung toàn viện tim hn
    /// </summary>
    public class Mrs00327Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public bool IS_MEDICINE { get;  set;  }
        public bool IS_MATERIAL { get;  set; }
        public bool IS_CHEMICAL_SUBSTANCE { get; set; }
        public string MEDICINE_TYPE_CODEs { get; set; }
        public string MATERIAL_TYPE_CODEs { get; set; }

        public List<long> MEDICINE_TYPE_IDs { get; set; }

        public List<long> MATERIAL_TYPE_IDs { get; set; }

        public Mrs00327Filter()
            : base()
        {

        }
    }
}
