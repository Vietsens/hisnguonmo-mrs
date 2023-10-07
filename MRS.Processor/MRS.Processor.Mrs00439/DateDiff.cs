using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00439
{
    public  class DateDiff
    {
        public static int diffDate(long? from, long? to)
        {
            if (from == null || to == null) return 0;
            DateTime dtIn = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(from.Value) ?? DateTime.Now;
            DateTime dtOut = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(to.Value) ?? DateTime.Now;
            return (int)((TimeSpan)(dtOut.Date - dtIn.Date)).TotalDays + 1;

        }
    }
}
