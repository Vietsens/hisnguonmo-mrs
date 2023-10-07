using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00102
{
    /// <summary>
    /// Báo cáo danh sách bệnh nhân bhyt nội trú đề nghị thanh toán (C80a) theo Khoa
    /// </summary>
    class Mrs00102Filter
    {
        public long? BRANCH_ID { get;  set;  }
        public long? DEPARTMENT_ID { get;  set;  }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public short? TIME_TYPE { get; set; }// null:duyet bhyt  | 0: vao vien | 1: ra vien |2: khoa vp

        public Mrs00102Filter()
            : base()
        {
        }
    }
}
