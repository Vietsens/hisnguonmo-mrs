using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00019
{
    public class Mrs00019Filter
    {
        public long? BRANCH_ID { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }

        public bool? IS_ROUTE { get; set; }

        public List<long> BRANCH_IDs { get; set; }

        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public string KEY_GROUP_SS { get; set; }
        public long? INPUT_DATA_ID_RPT { get; set; } // loại báo cáo: 1-Báo cáo chung, 2-chi tiết khoa phòng
        public long? SERVICE_TYPE_ID { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public bool? IS_RIGHT_ROUTE { get; set; }// đúng tyến , trái tuyến
        public bool? IS_IN_PROVICE { set; get; }// trong tỉnh , ngoại tỉnh
        public bool? IS_VALID_PROVINCE_FOR_A { get; set; }//check mã tỉnh cho nhóm ban đầu
    }
}
