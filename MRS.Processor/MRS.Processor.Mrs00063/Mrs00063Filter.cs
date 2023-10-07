using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00063
{
    /// <summary>
    /// Sổ ra viên theo khoa hồ sơ điều trị đã duyệt khóa kế toán
    /// </summary>
    class Mrs00063Filter
    {
        public long? DEPARTMENT_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        //Bắt buộc truyền vào từ đến - nếu không truyền vào => time=0
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }

        public Mrs00063Filter()
            : base()
        {
        }
    }
}
