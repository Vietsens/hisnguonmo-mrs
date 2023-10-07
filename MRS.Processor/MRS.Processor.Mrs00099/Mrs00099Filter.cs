using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00099
{
    /// <summary>
    /// Sổ chẩn đoán hình ảnh theo đối tượng điều trị
    /// </summary>
    class Mrs00099Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get;  set;  }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }
        public long? EXE_ROOM_ID { get; set; }

        public long? TREATMENT_TYPE_ID { get;  set;  }
        public List<long> TREATMENT_TYPE_IDs { get; set; }

        public long? REQUEST_DEPARTMENT_ID { get; set; }
        public long? EXECUTE_DEPARTMENT_ID { get; set; }
        public long? REQUEST_ROOM_ID { get; set; }

        public List<long> SERVICE_REQ_TYPE_IDs { get; set; }

        public List<long> SERVICE_REQ_STT_IDs { get; set; }


        public bool? IS_END_TIME { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }

        public List<long> KSK_CONTRACT_IDs { get; set; }

        public Mrs00099Filter()
            : base()
        {
        }
    }
}
