using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00421
{
    public class Mrs00421Filter
    {
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public bool? IS_AGGR_OR_CHMS_EXP_MEST { get; set; }
        public long? REQ_DEPARTMENT_ID { get; set; }
        public List<long> REQ_DEPARTMENT_IDs { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public List<long> EXP_MEST_TYPE_IDs { get; set; }
    }
}
