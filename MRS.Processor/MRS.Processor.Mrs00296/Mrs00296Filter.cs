using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00296
{
    public class Mrs00296Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public long? BRANCH_ID { get; set; }
        public long? DEPARTMENT_ID { get; set; }
        public long? EXAM_ROOM_ID { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
        public long? SERVICE_TYPE_ID { get; set; }
        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> PATIENT_CLASSIFY_IDs { get; set; }
        public List<long> WORK_PLACE_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }
        public List<long> AREA_IDs { get; set; }
        public bool? IS_CLINICAL_BILL { get; set; }
        public bool? IS_MOV_CLS_TO_PARENT { get; set; }
        public bool? IS_ADD_TREA_INFO { get; set; }

        public bool? IS_ADD_INFO_AREA { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> TRANSACTION_TYPE_IDs { get; set; }

        public bool? IS_ONLY_SPLIT_CASH { get; set; }

        public bool? IS_POLICE_OFFICER { get; set; }
        public string HEIN_RATIO { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SS_PATIENT_TYPE_IDs { get; set; }

        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }

        public bool IS_HOLIDAYS { get; set; }

        public bool IS_NOT_HOLIDAYS { get; set; }

        public long? INPUT_DATA_ID_TIME_TYPE { get; set; }
        public List<long> SERVICE_REQ_STT_IDs { get; set; }
        public List<string> REQUEST_LOGINNAMEs { get; set; }
        public List<string> EXECUTE_LOGINNAMEs { get; set; }

        public short? IS_EXECUTED { get; set; }
        public string KEY_GROUP_SS { get; set; }

        public bool? TAKE_ZERO_PRICE { get; set; }

        public long? INPUT_DATA_ID_IS_BILL { get; set; }

        public bool? EXAM_IS_CONSULT { get; set; }

        public long? HOUR_TIME_FROM { get; set; } 

        public long? HOUR_TIME_TO { get; set; }
        public bool? IS_PATIENT_TYPE { get; set; }


        public string STR_HOLIDAYS_NEWs { get; set; }
        public string STR_LUNAR_HOLIDAYS_NEWs { get; set; }

        public bool? CLEAR_AMOUNT_WHEN_NO_PRICE { get; set; }

        public string KEY_PRICEs { get; set; }


    }
}
