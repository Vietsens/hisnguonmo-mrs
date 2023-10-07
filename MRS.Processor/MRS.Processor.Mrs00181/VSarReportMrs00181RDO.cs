using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00181
{
    public class VSarReportMrs00181RDO
    {
        public string SERVICE_NAMEs { get; set; }
        public int COUNT { get; set; }
        public int SUM_TIME { get; set; }
        public string A { get; set; }
        public Dictionary<string, int> DIC_MONTH_COUNT { get; set; }
        public Dictionary<string, int> DIC_MONTH_SUM_TIME { get; set; }
        public Dictionary<string, int> DIC_DATE_COUNT { get; set; }
        public Dictionary<string, int> DIC_DATE_SUM_TIME { get; set; }
        public Dictionary<string, int> DIC_HOUR_COUNT { get; set; }
        public Dictionary<string, int> DIC_HOUR_SUM_TIME { get; set; }
    }
    public class ReqTypeUsed
    {
        public long TREATMENT_ID { get; set; }
        public long IN_TIME { get; set; }
        public long? OUT_TIME { get; set; }
        public long? FINISH_TIME { get; set; }
        public long? START_EXAM_TIME { get; set; }
        public long? FINISH_EXAM_TIME { get; set; }
        public string LIST_TYPE { get; set; }
    }
}
