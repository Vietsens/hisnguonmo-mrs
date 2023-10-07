using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00018
{
    public class Mrs00018Filter
    {
        public long? BRANCH_ID { get; set; }

        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }

        public bool? IS_ROUTE { get; set; }
        /// <summary>
        /// True: chi lay noi tru
        /// False: chi lay ngoai tru
        /// Null: lay ca hai
        /// </summary>
        public bool? IS_TREAT { get; set; }

        public List<long> BRANCH_IDs { get; set; }

        public string CATEGORY_CODE__OXY { get; set; }

        public string CATEGORY_CODE__VCM { get; set; }

        public bool? IS_BID_NUM_ORDER_MERGE { get; set; }//tách theo số thứ tự thầu

        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> MEDICINE_TYPE_IDs { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public bool? IS_RIGHT_ROUTE { get; set; }// đúng tyến , trái tuyến
        public bool? IS_IN_PROVICE { set; get; }// trong tỉnh , ngoại tỉnh

        public string OTHER_PAY_SOURCE_CODE_ALLOWS { get; set; }
    }
}
