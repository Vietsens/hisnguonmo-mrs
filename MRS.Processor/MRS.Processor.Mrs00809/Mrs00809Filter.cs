using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00809
{
    public class Mrs00809Filter
    {
        public long TIME_FROM { set; get; }
        public long TIME_TO {set;get;}
        public List<long> DEPARTMENT_IDs { set; get; }
        public List<long> EXECUTE_ROOM_IDs { set; get; }
        public bool CHECK_TYPE_TIME { set; get; }
        public List<long> BRANCH_IDs { set; get; }
        public long BRANCH_ID { set; get; }
    }
}
