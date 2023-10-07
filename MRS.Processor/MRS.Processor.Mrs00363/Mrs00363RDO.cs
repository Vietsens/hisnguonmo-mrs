using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00363
{
    class Mrs00363RDO
    {
        // biên bản kiểm nhập tùy chọn (viện tim)
        public long NUMBER { get;  set;  }

        public long DEPARTMENT_ID { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }

        public long ROOM_ID { get;  set;  }
        public string ROOM_NAME { get;  set;  }

        public string LOGINNAME { get;  set;  }           // tên đăng nhập
        public string EXECUTE_NAME { get;  set;  }        // tên bác sĩ khám

        public long END_OF_EXAM_AMOUNT { get;  set;  }    // số bn kết thúc khám
        public long TRAN_PATI_AMOUNT { get;  set;  }      // số bệnh nhân chuyển viện
        public long HOSPITALIZE_AMOUNT { get;  set;  }    // số bệnh nhân nhập viện 

        public long PRESCRIPTION_AMOUNT { get;  set;  }   // số đơn thuốc đã chỉ định
    }

    public class Mrs00363Execute
    {
        public string LOGINNAME { get;  set;  }
        public string EXECUTE_NAME { get;  set;  }

        public long ROOM_ID { get;  set;  }
        public string ROOM_NAME { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
        public string DEPARTMENT_NAME { get;  set;  }

        public long IS_HOSPITALIZE { get;  set;  }        // có nhập viện
        public long IS_TRAN_PATI { get;  set;  }          // có chuyển viện

        public long IS_PRESCRIPTION { get;  set;  }       // có đơn thuốc
    }
}
