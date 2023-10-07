using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00372
{
    public class Mrs00372RDO
    {
        // báo cáo bệnh nhân ngoại trú thực hiện dịch vụ
        public string TREATMENT_CODE { get;  set;  }      // mã bệnh nhân 
        public string PATIENT_NAME { get;  set;  }        // họ tên bệnh nhân
        public long DOB { get;  set;  }                   // ngày sinh => tuổi
        public string GENDER { get;  set;  }              // giới tính
        public string IS_HEIN { get;  set;  }             // có bhyt?
        public long INTRUCTION_TIME { get;  set;  }          // thời gian chỉ định
        public string SERVICE_NAME { get;  set;  }        // tên dịch vụ
        public decimal PRICE { get;  set;  }              // đơn giá

        public Mrs00372RDO(){}
    }

    public class PATIENT_TYPE_ALTER
    {
        public long TREATMENT_ID { get;  set;  }

        public long TREATMENT_TYPE_ID { get;  set;  }
        public string TREATMENT_TYPE_CODE { get;  set;  }
        public string TREATMENT_TYPE_NAME { get;  set;  }

        public long PATIENT_TYPE_ID { get;  set;  }
        public string PATIENT_TYPE_CODE { get;  set;  }
        public string PATIENT_TYPE_NAME { get;  set;  }

        public long LOG_TIME { get;  set;  }
        public long OUT_TIME { get;  set;  }
    }
}
