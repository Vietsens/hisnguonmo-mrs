using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826
{
    public class Mrs00826Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        /// <summary>
        /// 1: vào viện, 2: ra viện, 3: chỉ định, 4: kết thúc, 5: Thanh toán, 6: Khoá viện phí, 7-null: duyệt giám định BHYT
        /// </summary>
        public short? INPUT_DATA_ID__TIME_TYPE { get; set; }

        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public List<long> REQUEST_ROOM_IDs { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }

        //Thêm trường lọc lọc theo nhóm cha.
        public long? SERVICE_TYPE_ID { get; set; }
        public long? EXACT_PARENT_SERVICE_ID { get; set; }
        public List<long> EXACT_CHILD_SERVICE_IDs { get; set; }
    }
}
