using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00729
{
    class Mrs00729RDO
    {
        public class Sheet1
        {
            public long PATIENT_ID { get; set; }
            public long? TDL_PATIENT_DOB { get; set; }
            public string ROOM_CODE { get; set; }
            public string ROOM_NAME { get; set; }
            public long? COUNT_BHYT { get; set; }
            public long? COUNT_NOT_BHYT { get; set; }
            public long? COUNT_IN { get; set; }
            public long? COUNT_TRANSFER_EXAM_ROOM { get; set; }
            public long? COUNT_TRANSFER_ROUTE { get; set; }
            public long? COUNT_DEATH { get; set; }
            public long? COUNT_NGOAITRU { get; set; }
            public long? COUNT_APPOINTMENT { get; set; }
            public long? COUNT_TT { get; set; }
            public long? COUNT_PT { get; set; }
            public long? COUNT_CHILD_LESS_THAN_6 { get; set; }
            public long? COUNT_CHILD_MORE_THAN_6_LESS_THAN_18 { get; set; }
            public long? COUNT_MORE_THAN_18_LESS_THAN_60 { get; set; }
            public long? COUNT_MORE_THAN_60 { get; set; }

            public long? EXAM_TREATMENT_END_TYPE_ID { get; set; }

            public long? TDL_PATIENT_TYPE_ID { get; set; }
            public long? PREVIOUS_SERVICE_REQ_ID { get; set; }

            public long? TDL_TREATMENT_TYPE_ID { get; set; }

            public long? TDL_SERVICE_TYPE_ID { get; set; }

            public long? EXAM_END_TYPE { get; set; }

        }

        public class Sheet2
        {
            public long PATIENT_ID { get; set; }
            public long? TDL_PATIENT_DOB { get; set; }
            
            public long? COUNT_BHYT { get; set; }
            public long? COUNT_NOT_BHYT { get; set; }
            public long? COUNT_IN { get; set; }
            public long? COUNT_TRANSFER_EXAM_ROOM { get; set; }
            public long? COUNT_TRANSFER_ROUTE { get; set; }
            public long? COUNT_DEATH { get; set; }
            public long? COUNT_NGOAITRU { get; set; }
            public long? COUNT_APPOINTMENT { get; set; }
            
            public long? COUNT_CHILD_LESS_THAN_6 { get; set; }
            public long? COUNT_CHILD_MORE_THAN_6_LESS_THAN_18 { get; set; }
            public long? COUNT_MORE_THAN_18_LESS_THAN_60 { get; set; }
            public long? COUNT_MORE_THAN_60 { get; set; }

            public long? EXAM_TREATMENT_END_TYPE_ID { get; set; }

            public long? TDL_PATIENT_TYPE_ID { get; set; }
            public long? PREVIOUS_SERVICE_REQ_ID { get; set; }

            public long? TDL_TREATMENT_TYPE_ID { get; set; }

            public long? TDL_SERVICE_TYPE_ID { get; set; }

            public long? EXAM_END_TYPE { get; set; }

            public long? DEPARTMENT_ID { get; set; }
            public string DEPARTMENT_CODE { get; set; }
            public string DEPARTMENT_NAME { get; set; }

            public long IN_TIME { get; set; }

            public long TDL_TREATMENT_END_TYPE_ID { get; set; }

            public int COUNT_OLD { get; set; }

            public int COUNT_NOW { get; set; }

            public int COUNT_IN_CLINICAL { get; set; }

            public int COUNT_OUT { get; set; }

            public int COUNT_ALL { get; set; }

            public int COUNT_TRANSFER_DEPARTMENT { get; set; }

            public long? CLINICAL_IN_TIME { get; set; }

            public long? OUT_TIME { get; set; }

            public long? TREATMENT_DAY_COUNT { get; set; }
            public long? TREATMENT_DAY_COUNT_ALL { get; set; }



            public int COUNT_XINVE { get; set; }

            public int COUNT_BED_TREATMENT { get; set; }
        }

        public class Sheet3
        {
            public long? EXECUTE_DEPARTMENT_ID { get; set; }
            public long? REQUEST_DEPARTMENT_ID { get; set; }
            public long? TDL_SERVICE_TYPE_ID { get; set; }
            public long? PTTT_GROUP_ID { get; set; }

            public string EXECUTE_DEPARTMENT_CODE { get; set; }

            public string EXECUTE_DEPARTMENT_NAME { get; set; }

            public string REQUEST_DEPARTMENT_CODE { get; set; }

            public string REQUEST_DEPARTMENT_NAME { get; set; }

            public int COUNT_PT_1 { get; set; }

            public int COUNT_PT_2 { get; set; }

            public int COUNT_PT_3 { get; set; }

            public int COUNT_PT_DB { get; set; }

            public int COUNT_TT_1 { get; set; }

            public int COUNT_TT_2 { get; set; }

            public int COUNT_TT_3 { get; set; }

            public int COUNT_TT_DB { get; set; }
        }
    }
}
