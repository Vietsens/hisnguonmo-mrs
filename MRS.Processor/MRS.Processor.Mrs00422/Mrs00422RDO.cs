using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00422
{
    public class Mrs00422RDO
    {
        public long SERVICE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }
        public Decimal PRICE { get;  set;  }
        public Decimal AMOUNT { get;  set;  }
        public Decimal FEE { get;  set;  }
        public Decimal BHYT { get;  set;  }
        public Decimal CHILD { get;  set;  }
        public Decimal FREE { get;  set;  }
        public Decimal KSK { get;  set;  }
        public Decimal KSK_HD { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public Decimal IS_MALE { get; set; }
        public Decimal IS_FEMALE { get; set; }
        public long IN_TIME { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public long TDL_PATIENT_GENDER_ID { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public string TREATMENT_CODE { get; set; }
        public long SERVICE_REQ_STT_ID { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public string SERVICE_REQ_STT_NAME { get; set; }
        public string EXECUTE_ROOM_NAME { get; set; }
        public long? EXECUTE_ROOM_ID { get; set; }
        public long? REQUEST_ROOM_TYPE_ID { get; set; }
        public int IS_EXPEND { get; set; }
        public int IS_NO_EXECUTE { get; set; }
    }

    public class Mrs00422ServiceDoctorReq
    {
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public long? PARENT_NUM_ORDER { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public decimal PRICE { get; set; }
        public decimal AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_REQUEST_LOGINNAME_AMOUNT { get; set; }
        public decimal AMOUNT_FROM_REA { get; set; }
    }

}
