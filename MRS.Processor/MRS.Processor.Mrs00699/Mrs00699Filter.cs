using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00699
{
    public class Mrs00699Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public bool? IS_VIENPHI { set; get; }
        public bool? IS_DICHVU { get; set; }
        public bool? IS_BHXH { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        /// <summary>
        /// 1: vào viện, 2: ra viện, 3: khóa viện phí, 4: chuyển viện, 5: nhập viện, 6: chỉ định khám, 7: bắt đầu khám, 8: kết thúc khám
        /// </summary>
        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }
    }
}
