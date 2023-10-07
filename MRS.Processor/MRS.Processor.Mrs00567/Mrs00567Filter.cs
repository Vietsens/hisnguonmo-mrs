using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TYT.MANAGER.Core.TytUninfect.Get;

namespace MRS.Processor.Mrs00567
{
    public class Mrs00567Filter : TytUninfectFilterQuery
    {
        public string ICD_GROUP_CODE { get; set; }
        public string ICD_CODE { get; set; }
        public string ICD_CODE_INFUSIONs { get; set; }
    }
}
