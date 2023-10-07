using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00369
{
    /// <summary>
    /// báo cáo nhập
    /// </summary>
    public class Mrs00369Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long MEDI_BIG_STOCK_ID { get;  set;  }

        public List<long> IMP_MEST_TYPE_IDs { get;  set;  }

        public bool IS_MEDICINE { get;  set;  }
        public bool IS_MATERIAL { get;  set;  }
        public bool IS_BLOOD { get;  set;  }

        public bool IS_CHEMICAL_SUBSTANCE { get; set; }

        public Mrs00369Filter()
            : base()
        {

        }
    }
}
