using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00627
{
    class Mrs00627RDO
    {
        public long DATE { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY { get; set; }
        public Dictionary<string, int> DIC_CATEGORY_COUNT { get; set; }
        public Dictionary<string, decimal> DIC_SERVICE_TYPE_WAIT { get; set; }
        public Dictionary<string, decimal> DIC_EXAM_WAIT { get; set; }
        public Dictionary<string, decimal> DIC_EXAM_DOING { get; set; }
        public Dictionary<string, decimal> DIC_CATEGORY_CLS { get; set; }
        public Dictionary<string, decimal> DIC_SERVICE { get; set; }
        public Dictionary<string, decimal> DIC_SERVICE_ALL { get; set; }
        public Dictionary<string, int> DIC_ICD { get; set; }
        public Dictionary<string, int> DIC_BEGIN { get; set; }
        public Dictionary<string, int> DIC_IMP { get; set; }
        public Dictionary<string, int> DIC_CHMS { get; set; }
        public Dictionary<string, int> DIC_CLINICAL_IN { get; set; }
        public Dictionary<string, int> DIC_EXP { get; set; }
        public Dictionary<string, int> DIC_EXP_END_TYPE { get; set; }
        public Dictionary<string, int> DIC_EXP_OUT { get; set; }
        public Dictionary<string, int> DIC_EXP_TRAN { get; set; }
        public Dictionary<string, int> DIC_EXP_EXAM_TRAN { get; set; }
        public Dictionary<string, int> DIC_EXP_DIE { get; set; }
        public long NEW_PATIENT { get; set; }
        public long OLD_PATIENT { get; set; }
        public Dictionary<string, int> DIC_PATIENT_TYPE { get; set; }
        public Dictionary<string, int> DIC_TREATMENT_PATIENT_TYPE { get; set; }
        public long SUBCLINICAL_EXTRA { get; set; }
        public Dictionary<string, int> DIC_CLIENT_ICD { get; set; }
        public Dictionary<string, int> DIC_TREATMENT_EXAM_ICD { get; set; }
        public Dictionary<string, decimal> DIC_EMOTIONLESS_METHOD { get; set; }
        public Dictionary<string, decimal> DIC_EMEGENCY { get; set; }
        public Dictionary<string, decimal> DIC_UNEMEGENCY { get; set; }

        public Dictionary<string, decimal> DIC_SURG_GROUP { get; set; }

        public Dictionary<string, int> DIC_EXP_RESULT_END_TYPE { get; set; }

        public Dictionary<string, int> DIC_KSK { get; set; }
        public long COUNT_APPOINTMENT_TRUE { get; set; }
        public long COUNT_EXAM_CLINICAL_IN { get; set; }

        public Mrs00627RDO()
        {
            this.DIC_CATEGORY = new Dictionary<string, decimal>();
            this.DIC_CATEGORY_COUNT = new Dictionary<string, int>();
            this.DIC_SERVICE_TYPE_WAIT = new Dictionary<string, decimal>();
            this.DIC_EXAM_WAIT = new Dictionary<string, decimal>();
            this.DIC_EXAM_DOING = new Dictionary<string, decimal>();
            this.DIC_CATEGORY_CLS = new Dictionary<string, decimal>();
            this.DIC_SERVICE = new Dictionary<string, decimal>();
            this.DIC_SERVICE_ALL = new Dictionary<string, decimal>();
            this.DIC_ICD = new Dictionary<string, int>();
            this.DIC_BEGIN = new Dictionary<string, int>();
            this.DIC_IMP = new Dictionary<string, int>();
            this.DIC_CHMS = new Dictionary<string, int>();
            this.DIC_CLINICAL_IN = new Dictionary<string, int>();
            this.DIC_EXP = new Dictionary<string, int>();
            this.DIC_EXP_END_TYPE = new Dictionary<string, int>();
            this.DIC_EXP_OUT = new Dictionary<string, int>();
            this.DIC_EXP_TRAN = new Dictionary<string, int>();
            this.DIC_EXP_EXAM_TRAN = new Dictionary<string, int>();
            this.DIC_EXP_DIE = new Dictionary<string, int>();
            this.DIC_PATIENT_TYPE = new Dictionary<string, int>();
            this.DIC_TREATMENT_PATIENT_TYPE = new Dictionary<string, int>();
            this.DIC_CLIENT_ICD = new Dictionary<string, int>();
            this.DIC_EMOTIONLESS_METHOD = new Dictionary<string, decimal>();
            this.DIC_EMEGENCY = new Dictionary<string, decimal>();
            this.DIC_UNEMEGENCY = new Dictionary<string, decimal>();
            this.DIC_SURG_GROUP = new Dictionary<string, decimal>();
            this.DIC_BEGIN_LESS15 = new Dictionary<string, int>();
            this.DIC_BEGIN_MORE15 = new Dictionary<string, int>();
            this.DIC_IMP_LESS15 = new Dictionary<string, int>();
            this.DIC_IMP_MORE15 = new Dictionary<string, int>();
            this.DIC_EXP_RESULT_END_TYPE = new Dictionary<string, int>();
            this.DIC_TREATMENT_ICD = new Dictionary<string, int>();
            this.DIC_TREATMENT_EXAM_ICD = new Dictionary<string, int>();
            this.DIC_KSK = new Dictionary<string, int>();
            this.DIC_CATEGORY_TREATMENT_TYPE_CLS = new Dictionary<string, decimal>();
            this.DIC_SERVICE_TREATMENT_TYPE_ALL = new Dictionary<string, decimal>();
        }

        public Dictionary<string, int> DIC_BEGIN_LESS15 { get; set; }

        public Dictionary<string, int> DIC_BEGIN_MORE15 { get; set; }

        public Dictionary<string, int> DIC_IMP_LESS15 { get; set; }

        public Dictionary<string, int> DIC_IMP_MORE15 { get; set; }

        public Dictionary<string, int> DIC_TREATMENT_ICD { get; set; }

        public Dictionary<string, decimal> DIC_CATEGORY_TREATMENT_TYPE_CLS { get; set; }

        public Dictionary<string, decimal> DIC_SERVICE_TREATMENT_TYPE_ALL { get; set; }
    }

    public class DepartmentInOut
    {
        public long ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public long DEPARTMENT_ID { get; set; }
        public long? DEPARTMENT_IN_TIME { get; set; }
        public long? NEXT_ID { get; set; }
        public long? NEXT_DEPARTMENT_ID { get; set; }
        public long? NEXT_DEPARTMENT_IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? OUT_DATE { get; set; }
        public string TREATMENT_ICD_CODE { get; set; }
        public string TREATMENT_ICD_NAME { get; set; }
        public long? TREATMENT_RESULT_ID { get; set; }
        public long? TREATMENT_END_TYPE_ID { get; set; }
        public short? IS_PAUSE { get; set; }
        public long? TDL_TREATMENT_TYPE_ID { get; set; }
        public long? CLINICAL_IN_TIME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
    }

    public class PatientTypeAlter
    {
        public long ID { get; set; }
        public long DEPARTMENT_TRAN_ID { get; set; }
        public long TREATMENT_ID { get; set; }
        public long LOG_TIME { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }
        public long PATIENT_TYPE_ID { get; set; }
    }

    public class Client: HIS_TREATMENT
    {
        public short? IS_USED_TO_EXAM { get; set; }
        public short? IS_SUBCLINICAL_EXTRA { get; set; }
    }

    public class D_HIS_SERE_SERV
    {
        public long SERVICE_ID { get; set; }
        public long TDL_SERVICE_TYPE_ID { get; set; }
        public long? EMOTIONLESS_METHOD_ID { get; set; }
        public long TDL_INTRUCTION_DATE { get; set; }
        public long TDL_REQUEST_DEPARTMENT_ID { get; set; }
        public long TDL_EXECUTE_ROOM_ID { get; set; }
        public long TDL_TREATMENT_TYPE_ID { get; set; }

        public decimal AMOUNT { get; set; }

        public short? IS_EMERGENCY { get; set; }

        public long SERVICE_REQ_STT_ID { get; set; }

        public string TDL_SERVICE_CODE { get; set; }
    }

    public class KskAmount
    {
        public string CODE { get; set; }

        public long IN_DATE { get; set; }

        public int COUNT { get; set; }
    }

    public class AppointmentAmount
    {
        public long IN_DATE { get; set; }

        public int COUNT { get; set; }
    }
}
