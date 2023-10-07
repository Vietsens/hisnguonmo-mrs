using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00369
{
    class Mrs00369RDO
    {
        // báo cáo nhập
        public long NUMBER { get;  set;  }

        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }

        public long SERVICE_TYPE_ID { get;  set;  }
        public string SERVICE_TYPE_CODE { get;  set;  }
        public string SERVICE_TYPE_NAME { get;  set;  }

        public long SERVICE_ID { get;  set;  }
        public string SERVICE_CODE { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }           // đơn vị
        public string PACKING_TYPE_NAME { get;  set;  }           // quy cách đóng gói

        public string MANUFACTURER_NAME { get;  set;  }           // nhà sản xuất
        public string NATIONAL_NAME { get;  set;  }               // nước sản xuất
            
        public decimal AMOUNT { get;  set;  }                     // số lượng
        public decimal IMP_PRICE { get;  set;  }                  // giá nhập

        public string BID_NUMBER { get;  set;  }                  // số thầu
        public long? IMP_TIME { get;  set;  }                     // thời gian nhập

        public string DOCUMENT_NUMBER { get;  set;  }             // số chứng từ
        public long? DOCUMENT_DATE { get;  set;  }                // ngày chứng từ
    }
}
