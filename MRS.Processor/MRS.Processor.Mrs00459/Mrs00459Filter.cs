using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00459
{
    public class Mrs00459Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public List<long> DEPARTMENT_IDs { get;  set; }
        public short? INPUT_DATA_ID_TIME_TYPE { get; set; }//loại thời gian: 1. Khóa viện phí; 2: kết thúc điều trị
        public bool? IS_TREAT72H_AND_HAS_BIO { get; set; }//chỉ lấy bệnh nhân điều trị trên 72 giờ và có sử dụng kháng sinh
    }
}
