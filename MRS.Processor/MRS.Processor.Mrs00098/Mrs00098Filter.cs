using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00098
{
    /// <summary>
    /// Sổ siêu âm theo đối tượng điều trị
    /// </summary>
    class Mrs00098Filter
    {
        public long? TIME_FROM { get;  set;  }
        public long? TIME_TO { get; set; }
        public long? FINISH_TIME_FROM { get; set; }
        public long? FINISH_TIME_TO { get; set; }

        public long? TREATMENT_TYPE_ID { get;  set;  }
        public List<long> TREATMENT_TYPE_IDs { get;  set;  }

        public Mrs00098Filter()
            : base()
        {
        }
        public bool? IS_SUIM_1 { get; set; }
        public bool? IS_SUIM_2 { get; set; }
        public bool? IS_SUIM_3 { get; set; }
        public bool? IS_SUIM_4 { get; set; }
        public bool? IS_SUIM_5 { get; set; }
        public bool? IS_SUIM_6 { get; set; }
        public bool? IS_SUIM_7 { get; set; }

        public string SUIM_ROOM_CODE_1 { get; set; }
        public string SUIM_ROOM_CODE_2 { get; set; }
        public string SUIM_ROOM_CODE_3 { get; set; }
        public string SUIM_ROOM_CODE_4 { get; set; }
        public string SUIM_ROOM_CODE_5 { get; set; }
        public string SUIM_ROOM_CODE_6 { get; set; }
        public string SUIM_ROOM_CODE_7 { get; set; }

        public long? EXECUTE_ROOM_ID { get; set; }
    }
}
