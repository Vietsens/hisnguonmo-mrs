using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00156
{
    public class VSarReportMrs00156RDO
    {
        public long PARENT_ID { get;  set;  }

        public string MEDICINE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }

        public string NATIONAL_NAME { get;  set;  }

        public decimal? PRICE { get;  set;  }

        public decimal AMOUNT { get;  set;  }

        public decimal TOTAL_PRICE { get;  set;  }

        public long DEPARTMENT_ID { get; set; }
    }

    public class V_HIS_MEDICINE_TYPE_NEW
    {
        public long PARENT_ID { get;  set;  }

        public V_HIS_MEDICINE_TYPE V_HIS_MEDICINE_TYPE { get;  set;  }
    }

    public class V_HIS_MATERIAL_TYPE_NEW
    {
        public long PARENT_ID { get;  set;  }

        public V_HIS_MATERIAL_TYPE V_HIS_MATERIAL_TYPE { get; set; }
    }

    public class V_HIS_EXP_MEST_MEDICINE_NEW
    {
        public long PARENT_ID { get;  set;  }

        public V_HIS_EXP_MEST_MEDICINE V_HIS_EXP_MEST_MEDICINE { get;  set;  }
    }

    public class V_HIS_EXP_MEST_MATERIAL_NEW
    {
        public long PARENT_ID { get;  set;  }

        public V_HIS_EXP_MEST_MATERIAL V_HIS_EXP_MEST_MATERIAL { get; set; }
    }
}
