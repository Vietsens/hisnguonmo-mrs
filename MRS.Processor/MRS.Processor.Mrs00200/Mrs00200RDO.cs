using MRS.Processor.Mrs00200;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Proccessor.Mrs00200
{
    public class Mrs00200RDO : IMP_EXP_MEST_TYPE
    {
        public Mrs00200RDO()
        {
           DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
        }
        public string CATEGORY_NAME1 { get;  set;  }
        public string CATEGORY_NAME2 { get;  set;  }
        public long MEDI_STOCK_ID { get;  set;  }
        public long TYPE { get;  set;  }
        public long MEDI_MATE_TYPE_ID { get;  set;  }
        public long SERVICE_TYPE_ID { get;  set;  }

        public long MEDI_MATE_ID { get;  set;  }
        public long SERVICE_ID { get;  set;  }
        public string PACKAGE_NUMBER { get;  set;  }
        public string EXPIRED_DATE_STR { get;  set;  }
        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public string SERVICE_TYPE_NAME { get; set; }


        public decimal BEGIN_AMOUNT { get;  set;  }
        public decimal? VAT_RATIO { get;  set;  }
        public string VAT_RATIO_STR { get;  set;  }
        public decimal END_AMOUNT { get;  set;  }
        public decimal IMP_PRICE { get;  set;  }
        public string CONCENTRA { get;  set;  }
        public string MEDICINE_TYPE_PROPRIETARY_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }

        public long? SUPPLIER_ID { get;  set;  }
        public string SUPPLIER_CODE { get;  set;  }
        public string SUPPLIER_NAME { get;  set;  }

        public long? NUM_ORDER { get;  set;  }
        public decimal EXP_PRES_NT_BH_AMOUNT { get; set; }
        public decimal EXP_PRES_NT_ND_AMOUNT { get; set; }
        public decimal EXP_PRES_NGT_BH_AMOUNT { get; set; }
        public decimal EXP_PRES_NGT_ND_AMOUNT { get; set; }


        public decimal EXP_PRES_LESS6_BH_AMOUNT { get; set; }
        public decimal EXP_PRES_LESS6_ND_AMOUNT { get; set; }
        public decimal EXP_PRES_MORE6_BH_AMOUNT { get; set; }
        public decimal EXP_PRES_MORE6_ND_AMOUNT { get; set; }

        public long? MEDICINE_LINE_ID { get; set; }
        public string MEDICINE_LINE_CODE { get; set; }
        public string MEDICINE_LINE_NAME { get; set; }

        public long? MEDICINE_GROUP_ID { get; set; }
        public string MEDICINE_GROUP_CODE { get; set; }
        public string MEDICINE_GROUP_NAME { get; set; }

        //public long? MEMA_GROUP_ID { get; set; }
        //public string MEMA_GROUP_CODE { get; set; }
        //public string MEMA_GROUP_NAME { get; set; }
        public long? PARENT_MEDICINE_TYPE_ID { get; set; }
        public string PARENT_MEDICINE_TYPE_CODE { get; set; }
        public string PARENT_MEDICINE_TYPE_NAME { get; set; }
        public decimal IMP_TOTAL_AMOUNT { get; set; }
        public decimal EXP_TOTAL_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_EXP_MEST_REASON { get; set; }
    }
}
