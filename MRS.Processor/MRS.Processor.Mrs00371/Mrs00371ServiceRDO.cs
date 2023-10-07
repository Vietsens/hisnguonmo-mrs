using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00371
{
    class Mrs00371ServiceRDO
    {
        public long ID { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public long? PR_ID { get; set; }
        public string PR_SERVICE_CODE { get; set; }
        public string PR_SERVICE_NAME { get; set; }
    }
}
