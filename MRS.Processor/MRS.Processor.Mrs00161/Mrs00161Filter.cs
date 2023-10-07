using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00161
{
    /// <summary>
    /// Báo cáo sổ thủ thuật theo khoa, đối tượng bệnh nhân
    /// </summary>
    public class Mrs00161Filter : FilterBase
    {
        /// <summary>
        /// Khoa thuc hien
        /// </summary>
        public long? DEPARTMENT_ID { get;  set;  }

        /// <summary>
        /// Doi tuong benh nhan cuoi cung cua ho so dieu tri
        /// </summary>
        public long? PATIENT_TYPE_ID { get;  set;  }

        /// <summary>
        /// Đối tượng điều trị
        /// </summary>
        public long? TREATMENT_TYPE_ID { get;  set;  }

        public List<long> TREATMENT_TYPE_IDs { get;  set;  }

        /// <summary>
        /// Thoi gian yeu cau dinh vu tu den (Instruction_Time)
        /// </summary>
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public List<long> EXECUTE_ROOM_IDs { get; set; } //phòng thực hiện

        public short? INPUT_DATA_ID_SVT_TYPE { get; set; } //1:Phẫu thuật,2:thủ thuật

        public Mrs00161Filter()
            : base()
        {

        }
    }
}
