using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00542
{
    public class Mrs00542RDO
    {
        public V_HIS_TREATMENT V_HIS_TREATMENT { get; set; }
        public V_HIS_HEIN_APPROVAL V_HIS_HEIN_APPROVAL { get; set; }
        public V_HIS_SERE_SERV_3 V_HIS_SERE_SERV_3 { get; set; }
        public string MATERIAL_STT_DMBYT { get;  set;  }
        public string MATERIAL_CODE_DMBYT { get;  set;  }
        public string MATERIAL_CODE_DMBYT_1 { get;  set;  }
        public string MATERIAL_TYPE_NAME_BYT { get;  set;  }
        public string MATERIAL_TYPE_NAME { get;  set;  }
        public string MATERIAL_QUYCACH_NAME { get;  set;  }
        public string MATERIAL_UNIT_NAME { get;  set;  }
        public decimal MATERIAL_PRICE { get;  set;  } // gia mua vao
        public decimal AMOUNT_NGOAITRU { get;  set;  }
        public decimal AMOUNT_NOITRU { get;  set;  }
        public decimal PRICE { get;  set;  }
        public decimal TOTAL_PRICE { get;  set;  }

        public long? SERVICE_ID { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OUT_TIME_STR { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public short IS_STENT { get; set; }

        public Mrs00542RDO()
        {

        }

        public void SetExtendField(Mrs00542RDO Data)
        {

        }
    }
}
