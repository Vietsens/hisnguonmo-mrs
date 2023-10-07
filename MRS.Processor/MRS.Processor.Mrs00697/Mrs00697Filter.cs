using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00697
{
    public class Mrs00697Filter
    {
        public short? TIME_TYPE { get; set; }// null:thanh toan  | 0: ra vien |1: khoa vp | 2: khoa bhyt
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? PATIENT_TYPE_ID { get; set; } //đối tượng thanh toán
    }
}
