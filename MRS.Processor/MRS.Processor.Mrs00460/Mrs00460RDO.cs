using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00460
{
    public class Mrs00460RDO
    {
        public long TREATMENT_ID { get;  set;  }

        public long DEPARTMENT_ID { get;  set;  }         // khoa thực hiện
        public string DEPARTMENT_NAME { get;  set;  }     

        public long ROOM_GROUP_ID { get;  set;  }         // nhóm phòng
        public string ROOM_GROUP_NAME { get;  set;  }

        public long ROOM_ID { get;  set;  }               // phòng thực hiện
        public string ROOM_NAME { get;  set;  }

        public long EXAM_FIRST { get;  set;  }            // bệnh nhân khám lân đầu
        public long EXAM_REEX { get;  set;  }             // lượt khám thứ 2+

        public long PROVINCE { get;  set;  }              // từ tỉnh khác đén,  khác PP
        public long HOSPITALI_ZED { get;  set;  }          // nhập viện
        public long TRAN_TO_ED { get;  set;  }            // cấp cứu
        public long TRANS_HOS { get;  set;  }             // chuyển viện

        public Mrs00460RDO() { }
    }
}
