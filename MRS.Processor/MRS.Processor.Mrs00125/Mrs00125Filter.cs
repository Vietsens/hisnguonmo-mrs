using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00125
{
    /// <summary>
    /// Thống kê chi tiết bệnh nhân sử dụng dịch vụ kỹ thuật
    /// </summary>
    class Mrs00125Filter
    {
        public long PATIENT_TYPE_ID { get; set; }
        public long SERVICE_ID { get; set; }

        public long DATE_FROM { get; set; }
        public long DATE_TO { get; set; }

        public Boolean? TRUE_FALSE { get; set; }
    }
}
