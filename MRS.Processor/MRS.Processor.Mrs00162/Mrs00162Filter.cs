using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00162
{

    /// <summary>
    /// Báo cáo sổ nội soi theo khoa , đối tượng bệnh nhân
    /// </summary>
    public class Mrs00162Filter : FilterBase
    {
        /// <summary>
        /// Khoa thực hiện
        /// </summary>
        public long? DEPARTMENT_ID { get;  set;  }

        /// <summary>
        /// Đối tượng bệnh nhân
        /// </summary>
        public long? PATIENT_TYPE_ID { get;  set;  }

        /// <summary>
        /// Đối tượng điều trị
        /// </summary>
        public long? TREATMENT_TYPE_ID { get;  set;  }

        public List<long> TREATMENT_TYPE_IDs { get;  set;  }

        /// <summary>
        /// Thời gian yêu cầu dịch vụ từ đến (Instruction_Time)
        /// </summary>
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }

        public Mrs00162Filter()
            : base()
        {

        }
    }
}
