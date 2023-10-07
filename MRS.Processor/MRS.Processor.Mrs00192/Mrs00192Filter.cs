using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00192
{
    public class Mrs00192Filter
    {
        public long DATE_FROM { get; set; }
        public long DATE_TO { get; set; }

        public List<string> CREATOR_LOGINNAMEs { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<string> LOGINNAME_DOCTORs { get; set; }

        public int? INPUT_DATA_ID_REPORT_TYPE { get; set; } //1: Danh sách hồ sơ điều trị,2: Danh sách lượt khám
    }
}
