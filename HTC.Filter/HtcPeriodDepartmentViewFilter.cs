using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTC.Filter
{
    public class HtcPeriodDepartmentViewFilter : FilterBase
    {
        public long? PERIOD_ID { get; set; }

        public List<long> PERIOD_IDs { get; set; }

        public HtcPeriodDepartmentViewFilter()
            : base()
        {

        }
    }
}
