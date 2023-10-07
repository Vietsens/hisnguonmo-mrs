using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00187
{
    public class Mrs00187Filter
    {
        public long EXP_TIME_FROM { get;  set;  }
        public long EXP_TIME_TO { get;  set;  }
        public long DEPARTMENT_ID { get;  set;  }
        public List<long> IMP_MEDI_STOCK_IDs { get; set; }
        public List<long> MEDI_STOCK_IDs { get;  set;  } //kho xuất
        public List<string> REQ_LOGINNAMEs { get; set; }
        public string PATIENT_NAME { get; set; }
        public List<long> PATIENT_TYPE_IDs { get; set; }
        public List<long> SS_PATIENT_TYPE_IDs { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
        public bool? IS_AGGR { get; set; }
        public List<long> PATIENT_CLASSIFY_IDs { set; get; }
    }
}
