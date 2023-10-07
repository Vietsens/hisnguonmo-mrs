using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00135
{
    class Mrs00135RDO
    {
        public long SERVICE_ID { get;  set;  }
        public long HEIN_APPROVAL_ID { get;  set;  }

        public string HEIN_APPROVAL_CODE { get;  set;  }
        public string SERVICE_CODE { get;  set;  }
        public string MATERIAL_CODE { get;  set;  }
        public string SERVICE_REPORT_CODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string SERVICE_UNIT_NAME { get;  set;  }
        public decimal AMOUNT { get;  set;  }
        public decimal VIR_HEIN_PRICE { get;  set;  }
        public decimal RATIO { get;  set;  }//Tỷ lệ thanh toán
        public decimal VIR_TOTAL_HEIN_PRICE { get;  set;  }
        public string DEPARTMENT_BHYT_CODE { get;  set;  }
        public string DOCTOR_CODE { get;  set;  }
        public string ICD_CODEs { get;  set;  }//Bệnh chính và phụ cách nhau dấu ; 
        public string INSTRUCTION_DATE { get;  set;  }//Ngày y lệnh
        public string RESULT_DATE { get;  set;  }//Ngày trả kết quả
        public short PTTT_CODE { get;  set;  }//Mã phương thức thanh toán

        public long INSTRUCTION_TIME { get;  set;  }
        public long RESULT_TIME { get;  set;  }

        public bool IS_ATTACH { get;  set;  }
    }
}
