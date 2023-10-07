using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00371
{
    public class Mrs00371RDO
    {
        public string PATIENT_NAME { get; set; }
        public string FINISH_TIME { get; set; }
        public long? FINISH_TIME_NUM { get; set; }
        public string PATIENT_CODE { get; set; }
        public string INTRUCTION_TIME { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string ICD_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public long REPORT_TYPE_CAT_ID { get; set; }
        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CATEGORY_CODE { get; set; }
        public char IS_BHYRT { get; set; }
        public string MALE_AGE { get; set; }
        public string FEMALE_AGE { get; set; }
        public decimal AMOUNT { get; set; }
        public long TDL_REQUEST_ROOM_ID { get; set; }
        public string REQUEST_ROOM_CODE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }


        public string START_TIME { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
        public string PATIENT_TYPE_CODE { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }

        public string MACHINE_CODE { get; set; }
        public string MACHINE_NAME { get; set; }

        public string EXECUTE_MACHINE_CODE { get; set; }
        public string EXECUTE_MACHINE_NAME { get; set; }

        public string TDL_SERVICE_REQ_STT { get; set; }

        public string TDL_SERVICE_REQ_CODE { get; set; }

        public long COUNT_REQ { get; set; }

        public long COUNT_REQ_COMPLETE { get; set; }

        public string PARENT_SERVICE_CODE { get; set; }

        public string PARENT_SERVICE_NAME { get; set; }

        public long SERVICE_TYPE_ID { get; set; }

        public Dictionary<string, decimal> DIC_PARENT_SERVICE_AMOUNT { get; set; }

        public string SERVICE_CODE { get; set; }

        public string SERVICE_NAME { get; set; }

        public string ROOM_TYPE_ID_STR { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public long? SERVICE_REQ_STT_ID { get; set; }
        public long SERVICE_ID { get; set; }

        public long INTRUCTION_TIME_NUM { get; set; }

        public long? START_TIME_NUM { get; set; }

        public long? TDL_PATIENT_GENDER_ID { get; set; }

        public string REQUEST_LOGINNAME { get; set; }

        public string REQUEST_USERNAME { get; set; }

        public long? MACHINE_ID { get; set; }
    }
}
