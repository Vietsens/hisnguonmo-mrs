using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00051
{
    /// <summary>
    /// Bao cao tong hop danh sach benh nhan da xu ly tai phong
    /// </summary>
    class Mrs00051Filter
    {
        /// <summary>
        /// phong xu ly bat buoc
        /// </summary>
        public long? EXACT_EXECUTE_ROOM_ID { get; set; }
        /// </summary>
        public long? EXE_ROOM_ID { get; set; }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00051Filter()
            : base()
        {
        }

        public long? EXAM_ROOM_ID { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }
    }
}
