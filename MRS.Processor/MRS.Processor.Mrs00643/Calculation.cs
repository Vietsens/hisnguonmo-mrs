using IMSys.DbConfig.HIS_RS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00643
{
    public class PatientTypeEnum
    {
        public enum TYPE
        {
            BHYT,
            THU_PHI
        }
    }

    public static class Calculation
    {
        /// <summary>
        /// -- 1. Các TH sau tính ngày điều trị = ngày ra - ngày vào + 1:
        //+ Kết quả điều trị: không thay đổi, xử trí xin về
        //+ Kết quả điều trị: không thay đổi, xử trí chuyển viện
        //+ Kết quả điều trị: không thay đổi, xử trí tử xong
        //+ Kết quả điều trị: nặng hơn, xử trí xin về
        //+ Kết quả điều trị: nặng hơn, xử trí chuyển viện
        //+ Kết quả điều trị: nặng hơn, xử trí tử xong
        //-- 2. Các trường hợp còn lại tính ngày điều trị = ngày ra - ngày vào
        //-- 3. TH bn vào viện cùng 1 ngày có thời gian điều trị> 4h vẫn tính là 1 ngày điều trị như hiện tại.
        /// </summary>
        /// <param name="timeIn"></param>
        /// <param name="timeOut"></param>
        /// <returns></returns>
        public static long? DayOfTreatment(long? timeIn, long? timeOut, long? treatmentEndTypeId, long? treatmentResultId, PatientTypeEnum.TYPE patientTypeEnum)
        {
            long? result = null;
            try
            {
                if (!timeIn.HasValue || !timeOut.HasValue || !treatmentEndTypeId.HasValue
                    || !treatmentResultId.HasValue || timeIn > timeOut)
                    return result;

                DateTime dtIn = TimeNumberToSystemDateTime(timeIn.Value) ?? DateTime.Now;
                DateTime dtOut = TimeNumberToSystemDateTime(timeOut.Value) ?? DateTime.Now;
                TimeSpan ts = new TimeSpan();
                ts = (TimeSpan)(dtOut - dtIn);

                //Cung 1 ngay
                if (timeIn.Value.ToString().Substring(0, 8) == timeOut.Value.ToString().Substring(0, 8))
                {
                    if (ts.TotalMinutes <= 1440 && ts.TotalMinutes > 240)
                    {
                        result = 1;
                    }
                    else if (ts.TotalMinutes <= 240)
                    {
                        result = 0;
                    }
                }
                else //Khac 1 ngay
                {
                    //Nếu thời gian vào trước ngày 15/07/2018. Số ngày điều trị tính theo thông tư 37
                    //Nếu thười gian vào sau ngày 15/07/2018. Số ngày điều trị tính theo thông tư 15
                    if (timeIn.Value < 20180715000000 || patientTypeEnum == PatientTypeEnum.TYPE.THU_PHI)
                    {
                        result = (int)((TimeSpan)(dtOut.Date - dtIn.Date)).TotalDays + 1;
                    }
                    else if (patientTypeEnum == PatientTypeEnum.TYPE.BHYT)
                    {
                        if (treatmentEndTypeId.Value == HIS_TREATMENT_END_TYPE.ID__CHUYEN
                            || treatmentResultId.Value == HIS_TREATMENT_RESULT.ID__KTD
                            || treatmentResultId.Value == HIS_TREATMENT_RESULT.ID__NANG
                            || treatmentResultId.Value == HIS_TREATMENT_RESULT.ID__CHET)
                        {
                            result = (int)((TimeSpan)(dtOut.Date - dtIn.Date)).TotalDays + 1;
                        }
                        else
                            result = (int)((TimeSpan)(dtOut.Date - dtIn.Date)).TotalDays;
                    }
                }
            }
            catch (Exception ex)
            {
                result = 0;
            }
            return result;
        }

        private static System.DateTime? TimeNumberToSystemDateTime(long time)
        {
            System.DateTime? result = null;
            try
            {
                if (time > 0)
                {
                    result = System.DateTime.ParseExact(time.ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
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
