using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00356
{
    class Mrs00356RDO
    {
        // biên bản kiểm nhập tùy chọn (viện tim)
        public long NUMBER { get;  set;  }

        public long GROUP_ID { get;  set;  }
        public string GROUP_NAME { get;  set;  }

        public long PARENT_ID { get;  set;  }
        public string PARENT_CODE { get;  set;  }
        public string PARENT_NAME { get;  set;  }

        public long SERVICE_ID { get;  set;  }
        public string SERVICE_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }

        public string SERVICE_UNIT_NAME { get;  set;  }
        public string NATIONAL_NAME { get;  set;  }
        public string MANUFACTURER_NAME { get;  set;  }       // hãng sản xuất

        public string DOCUMENT_NUMBER { get;  set;  }         // số chứng từ
        public string BID_NUMBER { get;  set;  }              // số quyết định(số thầu)

        public decimal AMOUNT { get;  set;  }
        public decimal PRICE { get;  set;  }

        public string PACKAGE_NUMBER { get;  set;  }          // số lô
        public long EXPIRED_DATE { get;  set;  }              // hạn sử dụng
    }
}
