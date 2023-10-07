using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00629
{
    public class PatientTypeEnum
    {

        public enum TYPE
        {
            BHYT = 0,
            THU_PHI = 1,
        }
    }
    public static class Calculation
    {
        public static long? DayOfTreatment(long? timeIn, long? timeOut, long? treatmentEndTypeId, long? treatmentResultId, PatientTypeEnum.TYPE patientTypeEnum)
        {
            long? num = null;
            long? result;
            try
            {
                if (!timeIn.HasValue || !timeOut.HasValue || !treatmentEndTypeId.HasValue || !treatmentResultId.HasValue || timeIn > timeOut)
                {
                    result = num;
                    return result;
                }
                DateTime d = Calculation.TimeNumberToSystemDateTime(timeIn.Value) ?? DateTime.Now;
                DateTime d2 = Calculation.TimeNumberToSystemDateTime(timeOut.Value) ?? DateTime.Now;
                TimeSpan timeSpan = default(TimeSpan);
                timeSpan = d2 - d;
                if (timeIn.Value.ToString().Substring(0, 8) == timeOut.Value.ToString().Substring(0, 8))
                {
                    if (timeSpan.TotalMinutes <= 1440.0 && timeSpan.TotalMinutes >= 240.0)
                    {
                        num = new long?(1L);
                    }
                    else if (timeSpan.TotalMinutes < 240.0)
                    {
                        num = new long?(0L);
                    }
                }
                else if (timeIn.Value < 20180715000000L || patientTypeEnum == PatientTypeEnum.TYPE.THU_PHI)
                {
                    num = new long?((long)((int)(d2.Date - d.Date).TotalDays + 1));
                }
                else if (patientTypeEnum == PatientTypeEnum.TYPE.BHYT)
                {
                    if ((treatmentResultId.Value == 3L && treatmentEndTypeId.Value == 8L) || (treatmentResultId.Value == 3L && treatmentEndTypeId.Value == 2L) || (treatmentResultId.Value == 3L && treatmentEndTypeId.Value == 1L) || (treatmentResultId.Value == 4L && treatmentEndTypeId.Value == 8L) || (treatmentResultId.Value == 4L && treatmentEndTypeId.Value == 2L) || (treatmentResultId.Value == 4L && treatmentEndTypeId.Value == 1L))
                    {
                        num = new long?((long)((int)(d2.Date - d.Date).TotalDays + 1));
                    }
                    else
                    {
                        num = new long?((long)((int)(d2.Date - d.Date).TotalDays));
                    }
                }
            }
            catch (Exception ex)
            {
                num = new long?(0L);
            }
            result = num;
            return result;
        }

        private static DateTime? TimeNumberToSystemDateTime(long time)
        {
            DateTime? result = null;
            try
            {
                if (time > 0L)
                {
                    result = new DateTime?(DateTime.ParseExact(time.ToString(), "yyyyMMddHHmmss", CultureInfo.InvariantCulture));
                }
            }
            catch (Exception ex)
            {
                result = null;
            }
            return result;
        }
    }
}
