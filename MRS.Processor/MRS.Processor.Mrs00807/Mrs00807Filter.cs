using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00807
{
    public class Mrs00807Filter
    {
        public long TIME_FROM { set; get; }
        public long TIME_TO { set; get; }
        public List<long> PATIENT_TYPE_IDs { set; get; }
        public List<long> AREA_IDs { set; get; }
        public List<string> REQUEST_LOGINNAME_DOCTORs { set; get; }

        public long? EXECUTE_ROOM_ID { get; set; }

        public List<long> EXECUTE_ROOM_IDs { get; set; }

        public long? EXECUTE_DEPARTMENT_ID { get; set; }

        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }

        public long? REQUEST_ROOM_ID { get; set; }

        public List<long> REQUEST_ROOM_IDs { get; set; }

        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }

        public long? REQUEST_DEPARTMENT_ID { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }

        public List<long> TDL_PATIENT_TYPE_ID { get; set; }

        public short? INPUT_DATA_ID_TIME_TYPE { get; set; }

        public long? SERVICE_TYPE_ID { get; set; }

        public List<long> SERVICE_TYPE_IDs { get; set; }

        public long? SERVICE_ID { get; set; }

        public List<long> SERVICE_IDs { get; set; }

        public string SERVICE_NAME { get; set; }

        public long? EXACT_PARENT_SERVICE_ID { get; set; }

        public List<long> EXACT_PARENT_SERVICE_IDs { get; set; }

        public long? TREATMENT_TYPE_ID { get; set; }

        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public long? EXACT_CASHIER_ROOM_ID { get; set; }

        public List<long> EXACT_CASHIER_ROOM_IDs { get; set; }

        public string CASHIER_LOGINNAME { get; set; }

        public List<string> CASHIER_LOGINNAMEs { get; set; }

        public long? SERVICE_REQ_STT_ID { get; set; }

        public List<long> SERVICE_REQ_STT_IDs { get; set; }

        public List<long> TDL_PATIENT_TYPE_IDs { get; set; }

        public long? PATIENT_CLASSIFY_ID { get; set; }

        public List<long> PATIENT_CLASSIFY_IDs { get; set; }
    }
}
