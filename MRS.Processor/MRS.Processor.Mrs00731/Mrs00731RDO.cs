using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00731
{
    class Mrs00731RDO
    {
        public long ID { get; set; }
        public long TDL_TREATMENT_TYPE_ID { get; set; }
        public string SERVICE_CODE { get; set; }
        public long TDL_INTRUCTION_TIME { get; set; }
        public string SERVICE_NAME { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public string YEAR { get; set; }

        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
    }
}
