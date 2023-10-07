using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00181
{
    public class Mrs00181Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public List<long> ROOM_IDs { get; set; }
        public long? ROOM_ID { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public string SERVICE_CODE__XQ { get; set; }
        public string SERVICE_CODE__DT { get; set; }
        public bool? FILTER_01 { get; set; }//cdha chi lay xq nhom cha SERVICE_CODE__XQ, khong lay noi soi, tdcn chi lay dien tim thuong SERVICE_CODE__DT

        public bool? IS_NGOAI_TRU { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }

        public bool? IS_SPLIT_HASMEDI { get; set; }

        public string PARENT_SV_CODE__VSs { get; set; } // mã các dịch vụ cha xét nghiệm vi sinh ngăn cách nhau bởi dấu phẩy
    }
}
