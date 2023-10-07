using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00547
{
    public class Mrs00547RDO:HIS_SERE_SERV
    {
        //public HIS_SERVICE_REQ HIS_SERVICE_REQ { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_NAME { get; set; }
        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }

        //public HIS_SERE_SERV_EXT HIS_SERE_SERV_EXT { get; set; }
        public string NOTE { get; set; }
        public string CONCLUDE { get; set; }

        public long PATIENT_ID { get; set; }//
        public string PATIENT_CODE { get;  set;  }//
        public string PATIENT_NAME { get;  set;  }//

        public long TREATMENT_ID { get;  set;  }//
        public string TREATMENT_CODE { get;  set;  }//

        public string GENDER { get;  set;  }//
        public long DOB { get; set; }//
        public string DOB_YEAR_MALE { get; set; }//
        public string DOB_YEAR_FEMALE { get; set; }//

        public string ADDRESS { get; set; }//

        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_ROOM_CODE { get; set; }//
        public string REQUEST_DEPARTMENT_CODE { get; set; }//
        public string SERVICE_CODE { get; set; }//
        public string PATIENT_TYPE_CODE { get; set; }//

        public string REQUEST_USERNAME { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }//
        public string REQUEST_DEPARTMENT_NAME { get; set; }//
        public string SERVICE_NAME { get; set; }//
        public string PATIENT_TYPE_NAME { get; set; }//

        public string GROUP_HEIN_CARD { get; set; }//

        public string TREATMENT_TYPE_NAME_IN { get; set; }//

        public string TREATMENT_TYPE_NAME_OUT { get; set; }//

        public string CATEGORY_NAME { get; set; }

        public string CATEGORY_CODE { get; set; }

        public long INTRUCTION_DATE { get; set; }

        public long INTRUCTION_TIME { get; set; }

        public string BED_NAME { get; set; }

        public string BED_CODE { get; set; }

        public string BED_ROOM_NAME { get; set; }

        public string BED_ROOM_CODE { get; set; }

        public long TDL_PATIENT_GENDER_ID { get; set; }

        public string TDL_PATIENT_ADDRESS { get; set; }

        public string TDL_PATIENT_CODE { get; set; }

        public string TDL_PATIENT_NAME { get; set; }

        public string TDL_PATIENT_GENDER_NAME { get; set; }

        public long TDL_PATIENT_DOB { get; set; }

        public long TDL_TREATMENT_TYPE_ID { get; set; }
    }
}
