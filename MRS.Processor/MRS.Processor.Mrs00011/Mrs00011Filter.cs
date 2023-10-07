using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00011
{
    /// <summary>
    /// Filter báo cáo bệnh nhân đang điều trị tại khoa
    /// </summary>
    class Mrs00011Filter
    {
        /// <summary>
        /// Khoa bat buoc
        /// </summary>
        public long? DEPARTMENT_ID { get;  set;  }
        /// <summary>
        /// Thoi gian tong hop du lieu
        /// </summary>
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }

        public Mrs00011Filter()
            : base()
        {
        }
    }
}
