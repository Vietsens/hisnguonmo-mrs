using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00242
{
    public static class DayOfTreatmentWith3360
    {
        public static long DayOfTreatmentWithCV3360(long? timeNumberIn, long? timeNumberOut)
        {
            long result = 0;
            try
            {
                if (!timeNumberIn.HasValue)
                    return 0;
                if (!timeNumberOut.HasValue)
                    return 0;
                if (timeNumberIn > timeNumberOut)
                    return result;

                DateTime dtIn = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(timeNumberIn.Value) ?? DateTime.Now;
                DateTime dtOut = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(timeNumberOut.Value) ?? DateTime.Now;

                TimeSpan ts = new TimeSpan();
                ts = (TimeSpan)(dtOut - dtIn);
                if (ts.TotalMinutes <= 1440 && ts.TotalMinutes >= 240)
                {
                    result = 1;
                }
                else if (ts.TotalMinutes < 240)
                {
                    result = 0;
                }
                else
                {
                    result = (int)((TimeSpan)(dtOut.Date - dtIn.Date)).TotalDays + 1;
                }
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }
    }
}
