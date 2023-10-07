using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00356
{
    /// <summary>
    /// biên bản kiểm nhập tùy chọn (viện tim)
    /// </summary>
    public class Mrs00356Filter : FilterBase
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public long? IMP_SOURCE_ID { get;  set;  }

        public List<long> MEDI_STOCK_IDs { get;  set;  }

        public bool IS_MEDICINE { get;  set;  }
        public bool IS_MATERIAL { get;  set;  }
        public bool IS_BLOOD { get;  set;  }

        public Mrs00356Filter()
            : base()
        {

        }
    }
}
