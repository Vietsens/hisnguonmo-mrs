using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00464
{
    public class Mrs00464RDO : HIS_TREATMENT
    {
        //public long GROUP_ID { get;  set;  }
        public string TREATMENT_RESULT_NAME { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string PATIENT_NAME { get;  set;  }
        public string GENDER { get; set; }
        public string ADDRESS { get; set; }
        public string PATIENT_TYPE_NAME { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string BIRTHDAY { get;  set;  }
        public string IN_ICD { get;  set;  }
        public string TIME { get;  set;  }
        //public string OUT_TIME { get;  set;  }
        public string CREAT_USER_CODE { get; set; }
        public string IN_ROOM_NAME { get; set; }

        public long CLINICAL_DEPARTMENT_ID { get; set; }
        public string CLINICAL_DEPARTMENT_CODE { get; set; }
        public string CLINICAL_DEPARTMENT_NAME { get; set; }

        public string ICD_GROUP_CODE { get; set; }
        public Mrs00464RDO() { }

        public Mrs00464RDO(HIS_TREATMENT r)
        {
            PropertyInfo[] p = typeof(HIS_TREATMENT).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
                
            }
        }

        public long ON_DEPARTMENT_ID { get; set; }

        public string ON_DEPARTMENT_CODE { get; set; }

        public string ON_DEPARTMENT_NAME { get; set; }

        public string PHONE { get; set; }

        public string ETHNIC_NAME { get; set; }

        public string RELATIVE { get; set; }

        public long PREVIOUS_DEPARTMENT_ID { get; set; }

        public long LOG_TIME { get; set; }

        public long REQUEST_TIME { get; set; }

        public long SAVE_TIME { get; set; }

        public string RELATIVE_TYPE { get; set; }

        public string RELATIVE_NAME { get; set; }

        public string PTTT_METHOD_IDS { get; set; }

        public string PTTT_METHOD_NAMES { get; set; }

        public string PART_EXAM_EYESIGHT_GLASS_RIGHT { get; set; }

        public string PART_EXAM_EYESIGHT_GLASS_LEFT { get; set; }

        public string PART_EXAM_EYESIGHT_RIGHT { get; set; }

        public string PART_EXAM_EYESIGHT_LEFT { get; set; }

        public string PART_EXAM_EYE_TENSION_RIGHT { get; set; }

        public string PART_EXAM_EYE_TENSION_LEFT { get; set; }

        public string APPOINT_EYESIGHT_GLASS_RIGHT { get; set; }

        public string APPOINT_EYESIGHT_GLASS_LEFT { get; set; }

        public string APPOINT_EYESIGHT_RIGHT { get; set; }

        public string APPOINT_EYESIGHT_LEFT { get; set; }

        public string APPOINT_EYE_TENSION_RIGHT { get; set; }

        public string APPOINT_EYE_TENSION_LEFT { get; set; }

        public string APPOINT_EYESIGHT_GLASS_RIGHT_2 { get; set; }

        public string APPOINT_EYESIGHT_GLASS_LEFT_2 { get; set; }

        public string APPOINT_EYESIGHT_RIGHT_2 { get; set; }

        public string APPOINT_EYESIGHT_LEFT_2 { get; set; }

        public string APPOINT_EYE_TENSION_RIGHT_2 { get; set; }

        public string APPOINT_EYE_TENSION_LEFT_2 { get; set; }

        public string APPOINT_EYESIGHT_GLASS_RIGHT_3 { get; set; }

        public string APPOINT_EYESIGHT_GLASS_LEFT_3 { get; set; }

        public string APPOINT_EYESIGHT_RIGHT_3 { get; set; }

        public string APPOINT_EYESIGHT_LEFT_3 { get; set; }

        public string APPOINT_EYE_TENSION_RIGHT_3 { get; set; }

        public string APPOINT_EYE_TENSION_LEFT_3 { get; set; }

        public string MANNER { get; set; }
    }

    public class PATIENT_TYPE_ALTER
    {
        public long TREATMENT_ID { get; set; }
        public long LOG_TIME { get; set; }
        public long DEPARTMENT_TRAN_ID { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }

        public string RIGHT_ROUTE_CODE { get; set; }

        public long CREATE_TIME { get; set; }
    }

    public class DEPARTMENT_TRAN
    {
        public long TREATMENT_ID { get; set; }
        public long? DEPARTMENT_IN_TIME { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public long ID { get; set; }
        public long? PREVIOUS_ID { get; set; }

        public long? REQUEST_TIME { get; set; }

        public string CREATOR { get; set; }

        public string ICD_NAME { get; set; }
    }

    public class PATIENT
    {
        public long ID { get; set; }
        public string PHONE  { get; set; }
        public string RELATIVE_TYPE { get; set; }
        public string RELATIVE_NAME { get; set; }
        public string ETHNIC_NAME { get; set; }
    }

    public class GROUP_ICD
    {
        public string ICD_GROUP_CODE { get; set; }
        public int COUNT_ALL { get; set; }
        public int COUNT_BHYT { get; set; }
        public int COUNT_TP { get; set; }
    }
    public class INFO_EXAM_EYE
    {
        public long TREATMENT_ID { get; set; }
        public long SERVICE_REQ_ID { get; set; }
        public string PART_EXAM_EYESIGHT_GLASS_RIGHT { get; set; }

        public string PART_EXAM_EYESIGHT_GLASS_LEFT { get; set; }

        public string PART_EXAM_EYESIGHT_RIGHT { get; set; }

        public string PART_EXAM_EYESIGHT_LEFT { get; set; }

        public string PART_EXAM_EYE_TENSION_RIGHT { get; set; }

        public string PART_EXAM_EYE_TENSION_LEFT { get; set; }
    }
}
