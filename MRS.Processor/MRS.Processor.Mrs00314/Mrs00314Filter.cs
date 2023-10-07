using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00314
{
    public class Mrs00314Filter
    {
        public long? TIME_FROM { get; set; }
        public long? TIME_TO { get; set; }
        public long? OUT_TIME_FROM { get; set; }
        public long? OUT_TIME_TO { get; set; }
        public long? TRANSACTION_TIME_FROM { get; set; }
        public long? TRANSACTION_TIME_TO { get; set; }
        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public string PATIENT_TYPE_CODE__BHYT { get; set; }
        public string PATIENT_TYPE_CODE__PHI { get; set; }
        public string PATIENT_TYPE_CODE__YC { get; set; }

        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public bool? IS_NOT_SPLIT_ROOM { get; set; }

        public bool? IS_NOT_SPLIT_DEPA { get; set; }

        public bool? IS_EXAM_REQ_TO_EXE_ROOM { get; set; }

        public bool? IS_GROUP_PARENT { get; set; }

        public short? REDU_OVER_HEIN_LIMIT { get; set; }

        public bool? IS_COUNT_DAY_DEPA { get; set; }

        public List<long> OTHER_PAY_SOURCE_IDs { get; set; }

        public string KEY_TOTALs { get; set; }
    }
}
