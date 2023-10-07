using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00679
{
    public class Mrs00679Filter
    {
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SERVICE_REQ_TYPE_IDs { get; set; }
        public List<long> SERVICE_REQ_STT_IDs { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> END_ROOM_IDs { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public long? BRANCH_ID { get; set; }

        public bool? IS_MERGE_TREA_SERV { get; set; }
        public short? INPUT_DATA_ID_TIME_TYPE { get; set; } //1:vao vien, 2:chi dinh, 3:bat dau, 4:ket thuc, 5: ra vien, 6: thanh toan, 7: khoa vien phi, 8: Duyet giam dinh


    }
}
