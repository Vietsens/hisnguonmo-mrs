using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00073
{
    public class Mrs00073Filter
    {
        public long? BRANCH_ID { get; set; }
        public List<long> BRANCH_IDs { get; set; }

        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get;  set;  }
        public bool? SPLIT_RIGHT_ROUTE { get; set; }
        public List<long> END_DEPARTMENT_IDs { get; set; }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public List<string> CASHIER_LOGINNAMEs { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> EXECUTE_DEPARTMENT_IDs { get; set; }
        public List<long> EXECUTE_ROOM_IDs { get; set; }
        public List<long> REQUEST_DEPARTMENT_IDs { get; set; }
        public List<long> SERVICE_TYPE_IDs { get; set; }
        public List<long> SERVICE_IDs { get; set; }
        public List<long> SS_PATIENT_TYPE_IDs { get; set; }
        public bool? IS_POLICE_OFFICER { get; set; }
        public string HEIN_RATIO { get; set; }

        public short? INPUT_DATA_ID_PROVINCE_TYPE { get; set; } //1:Noi tinh, 2:Ngoai tinh

        public short? INPUT_DATA_ID_ROUTE_TYPE { get; set; } //1:Dung tuyen, 2:Trai tuyen

        public string ACCEPT_HEIN_MEDI_ORG_CODE { get; set; }
    }
}
