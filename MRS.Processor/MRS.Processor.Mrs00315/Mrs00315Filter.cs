using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00315
{
    public class Mrs00315Filter: FilterBase
    {
        public long? BRANCH_ID { get; set; }
        public long? END_DEPARTMENT_ID { get; set; }

        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public bool? IS_MERGE_TREATMENT { get; set; }
        public bool? IS_GROUP_BY_TREATMENT { get; set; }
        public string OTHER_PAY_SOURCE_CODE_ALLOWS { get; set; }
        public short? INPUT_DATA_ID__TIME_TYPE { get; set; }//1: th?i gian vào vi?n, 2: ra vi?n 3: ch? ??nh 4: duy?t giám ??nh b?o hi?m

        public string KEY_SPLIT_SR { get; set; }

        public Mrs00315Filter()
            : base()
        {

        }
    }
}
