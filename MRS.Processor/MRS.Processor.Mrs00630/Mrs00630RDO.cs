using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00630
{
    public class Mrs00630RDO
    {
        
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public Dictionary<string,int> DIC_WAIT_TYPE_TIME { get; set; }
        public Dictionary<string, int> DIC_TYPE_TIME { get; set; }
        public Dictionary<string, int> DIC_GROUP_TIME { get; set; }
        public Dictionary<string, int> DIC_WAIT_GROUP_TIME { get; set; }
        public Dictionary<string, int> DIC_TYPE_TIME1 { get; set; }
        public Dictionary<string, int> DIC_TYPE_TIME2 { get; set; }
        public Dictionary<string, int> DIC_TYPE_TIME3 { get; set; }
        public Dictionary<string, int> DIC_TYPE_TIME4 { get; set; }
        public Dictionary<string, int> DIC_TYPE_TIME5 { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public long? START_TIME { get; set; }
        public long INTRUCTION_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? IN_TIME { get; set; }
        public long? FEE_LOCK_TIME { get; set; }
        public string TDL_FIRST_EXAM_ROOM_NAME { get; set; } 
        public long HEIN_LOCK_TIME { get; set; }
        public int WAIT_FEE_LOCK_TIME { get; set; }
        public int ON_TIME { get; set; }
        public bool HAS_CLS { get; set; }
        public long? FINISH_TIME { get; set; }
        public string LIST_SERVICE_TYPE { get; set; }
        //public int COMBO_1_KH { get; set; }
        //public int COMBO_2_KH { get; set; }
        //public int COMBO_2_XN { get; set; }
        //public int COMBO_3_KH { get; set; }
        //public int COMBO_3_XN { get; set; }
        //public int COMBO_3_HA { get; set; }
        //public int COMBO_4_KH { get; set; }
        //public int COMBO_4_XN { get; set; }
        //public int COMBO_4_HA { get; set; }
        //public int COMBO_4_CN { get; set; }
        //public int COMBO_5_KH { get; set; }
        //public int COMBO_5_CL { get; set; }
        
        public Mrs00630RDO()
        {
            DIC_WAIT_TYPE_TIME = new Dictionary<string, int>();
            DIC_TYPE_TIME = new Dictionary<string, int>();
            DIC_GROUP_TIME = new Dictionary<string, int>();
            DIC_WAIT_GROUP_TIME = new Dictionary<string, int>();
            DIC_TYPE_TIME1 = new Dictionary<string, int>();
            DIC_TYPE_TIME2 = new Dictionary<string, int>();
            DIC_TYPE_TIME3 = new Dictionary<string, int>();
            DIC_TYPE_TIME4 = new Dictionary<string, int>();
            DIC_TYPE_TIME5 = new Dictionary<string, int>();
            
        }

        public class LIST_TYPE
        {
            public long? START_TIME { get; set; }
            public long? FINISH_TIME { get; set; }
            public long? OUT_TIME { get; set; }
            public long TREATMENT_ID { get; set; }
            public string TYPE { get; set; }
        }
    }

    public class SAMPLE
    {
        public long SERVICE_REQ_ID { get; set; }
        public long CREATE_TIME { get; set; }
        public long RESULT_TIME { get; set; }
    }
}
