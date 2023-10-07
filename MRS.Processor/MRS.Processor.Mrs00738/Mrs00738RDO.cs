using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00738
{
    class Mrs00738RDO
    {
        public string TDL_PATIENT_CODE { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public string DEPARTMENT_TIME_STR { get; set; }

        public string LOGIN_NAME { get; set; }

        public string EVENT_TIME_STR { get; set; }

        public string IP { get; set; }

        public string DESCRIPTION { get; set; }

        public string START_TIME_STR { get; set; }

        public long START_TIME { get; set; }

        public string FINISH_TIME_STR { get; set; }

        public string TREATMENT_CODE { get; set; }

        public string BED_NAME { get; set; }

        public string BED_ROOM_NAME { get; set; }

        public string CREATOR { get; set; }
    }

    class TREATMENT_BED_LOG
    {
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string BED_CODE { get; set; }
        public string BED_NAME { get; set; }
        public string BED_ROOM_NAME { get; set; }

        public long IN_TIME { get; set; }

        public long START_TIME { get; set; }

        public long? FINISH_TIME { get; set; }
        public long? DEPARTMENT_IN_TIME { get; set; }
        public string CREATOR { get; set; }
        public string SERVICE_REQ_CODE { get; set; }


        public long? DEPARTMENT_IN_DATE { get; set; }
    }
}
