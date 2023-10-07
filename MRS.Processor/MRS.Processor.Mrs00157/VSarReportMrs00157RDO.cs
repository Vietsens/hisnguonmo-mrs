using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00157
{
    public class VSarReportMrs00157RDO
    {
        public long PARENT_ID { get; set ; }
        public long IMP_STT { get;  set;  }
        public string IMP_NAME_MEDICINE { get;  set;  }
        public string IMP_UNIT { get;  set;  }
        public decimal IMP_NUMBER_MEDICINE { get;  set;  }
        public decimal? IMP_CK { get;  set;  }
        public decimal IMP_PRICE_MEDICINE { get;  set;  }
        public decimal? IMP_TOTAL_PRICE { get;  set;  }

    }
    public class V_HIS_IMP_MEST_MEDICINE_NEW
    {
        public long PARENT_ID { get;  set;  }
        public V_HIS_IMP_MEST_MEDICINE V_HIS_IMP_MEST_MEDICINE { get;  set;  }
    }
}
