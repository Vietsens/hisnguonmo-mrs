using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00318
{
    /// <summary>
    /// De nghi thanh toan bao hiem y te benh nhan kham chua benh ngoai tru: File Mem 46 cot
    /// </summary>
    public class Mrs00318Filter : FilterBase
    {
        public long? BRANCH_ID { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }
        public bool? IS_KB { get; set; }
        public bool? IS_TNT { get; set; }
        public bool? IS_CC { get; set; }
        public string KB1 { get; set; }
        public string KB2 { get; set; }
        public string TNT { get; set; }
        public string CC { get; set; }
        public long TIME_FROM { get;  set;  }
        public long TIME_TO { get; set; }
        public string OTHER_PAY_SOURCE_CODE_ALLOWS { get; set; }
        public bool? IS_GROUP_BY_TREATMENT { get; set; }
        public short? INPUT_DATA_ID__TIME_TYPE { get; set; }//1: th?i gian vào vi?n, 2: ra vi?n 3: ch? ??nh 4: duy?t giám ??nh b?o hi?m

        public string KEY_SPLIT_SR { get; set; }

        public Mrs00318Filter()
            : base()
        {

        }
    }
}
