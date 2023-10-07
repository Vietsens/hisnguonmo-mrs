using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00045
{
    /// <summary>
    /// So luu tru ho so benh an tu vong
    /// </summary>
    class Mrs00045Filter
    {
        public long? CREATE_TIME_FROM { get;  set;  }
        public long? CREATE_TIME_TO { get;  set;  }

        public long? DEPARTMENT_ID { get;  set;  }

        public Mrs00045Filter()
            : base()
        {
        }

        public short? INPUT_DATA_ID_TIME_TYPE { get; set; }// loại thời gian: mặc định lấy theo thời gian ra viện, chọn 1: thời gian lưu trữ, chọn 2: thời gian ra viện
    }
}
