using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00002
{
    public class convertSolar2Lunar
    {
        public int[] convertSolar2Lunars(int dd, int mm, int yy, int timeZone)
        {
            try
            {
                long dayNumber = jdFromDate(dd, mm, yy);
                long k = INT((dayNumber - 2415021.076998695) / 29.530588853);
                long monthStart = getNewMoonDay(k + 1, timeZone);
                if (monthStart > dayNumber)
                {
                    monthStart = getNewMoonDay(k, timeZone);
                }
                long a11 = getLunarMonth11(yy, timeZone);
                long b11 = a11;
                int lunarYear;
                if (a11 >= monthStart)
                {
                    lunarYear = yy;
                    a11 = getLunarMonth11(yy - 1, timeZone);
                }
                else
                {
                    lunarYear = yy + 1;
                    b11 = getLunarMonth11(yy + 1, timeZone);
                }
                long lunarDay = dayNumber - monthStart + 1;
                long diff = INT((monthStart - a11) / 29);
                int lunarLeap = 0;
                long lunarMonth = diff + 11;
                if (b11 - a11 > 365)
                {
                    int leapMonthDiff = getLeapMonthOffset(a11, timeZone);
                    if (diff >= leapMonthDiff)
                    {
                        lunarMonth = diff + 10;
                        if (diff == leapMonthDiff)
                        {
                            lunarLeap = 1;
                        }
                    }
                }
                if (lunarMonth > 12)
                {
                    lunarMonth = lunarMonth - 12;
                }
                if (lunarMonth >= 11 && diff < 4)
                {
                    lunarYear -= 1;
                }

                return new int[] { (int)lunarDay, (int)lunarMonth, (int)lunarYear, lunarLeap };
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
                return new int[] { (int)0, (int)0, (int)0, 0 };
            }
           
        }

        private long INT(double d)
        {
            return (long)Math.Floor(d);
        }

        private long jdFromDate(int dd, int mm, int yy)
        {
            long a = INT((14 - mm) / 12);
            long y = yy + 4800 - a;
            long m = mm + 12 * a - 3;
            long jd = dd + INT((153 * m + 2) / 5) + 365 * y + INT(y / 4) - INT(y / 100) + INT(y / 400) - 32045;
            if (jd < 2299161)
            {
                jd = dd + INT((153 * m + 2) / 5) + 365 * y + INT(y / 4) - 32083;
            }
            return jd;
        }

        private long getNewMoonDay(long k, long timeZone)
        {
            double T = k / 1236.85; // Time in Julian centuries from 1900 January 0.5
            double T2 = T * T;
            double T3 = T2 * T;
            double dr = Math.PI / 180;
            double Jd1 = 2415020.75933 + 29.53058868 * k + 0.0001178 * T2 - 0.000000155 * T3;
            Jd1 = Jd1 + 0.00033 * Math.Sin((166.56 + 132.87 * T - 0.009173 * T2) * dr); // Mean new moon
            double M = 359.2242 + 29.10535608 * k - 0.0000333 * T2 - 0.00000347 * T3; // Sun's mean anomaly
            double Mpr = 306.0253 + 385.81691806 * k + 0.0107306 * T2 + 0.00001236 * T3; // Moon's mean anomaly
            double F = 21.2964 + 390.67050646 * k - 0.0016528 * T2 - 0.00000239 * T3; // Moon's argument of latitude
            double C1 = (0.1734 - 0.000393 * T) * Math.Sin(M * dr) + 0.0021 * Math.Sin(2 * dr * M);
            C1 = C1 - 0.4068 * Math.Sin(Mpr * dr) + 0.0161 * Math.Sin(dr * 2 * Mpr);
            C1 = C1 - 0.0004 * Math.Sin(dr * 3 * Mpr);
            C1 = C1 + 0.0104 * Math.Sin(dr * 2 * F) - 0.0051 * Math.Sin(dr * (M + Mpr));
            C1 = C1 - 0.0074 * Math.Sin(dr * (M - Mpr)) + 0.0004 * Math.Sin(dr * (2 * F + M));
            C1 = C1 - 0.0004 * Math.Sin(dr * (2 * F - M)) - 0.0006 * Math.Sin(dr * (2 * F + Mpr));
            C1 = C1 + 0.0010 * Math.Sin(dr * (2 * F - Mpr)) + 0.0005 * Math.Sin(dr * (2 * Mpr + M));
            double deltat = 0;
            if (T < -11)
            {
                deltat = 0.001 + 0.000839 * T + 0.0002261 * T2 - 0.00000845 * T3 - 0.000000081 * T * T3;
            }
            else
            {
                deltat = -0.000278 + 0.000265 * T + 0.000262 * T2;
            };
            double JdNew = Jd1 + C1 - deltat;
            return INT(JdNew + 0.5 + (double)((double)timeZone / 24));
        }

        private long getSunLongitude(long jdn, int timeZone)
        {
            double T = (jdn - 2451545.5 - timeZone / 24) / 36525; // Time in Julian centuries from 2000-01-01 12:00:00 GMT
            double T2 = T * T;
            double dr = Math.PI / 180; // degree to radian
            double M = 357.52910 + 35999.05030 * T - 0.0001559 * T2 - 0.00000048 * T * T2; // mean anomaly, degree
            double L0 = 280.46645 + 36000.76983 * T + 0.0003032 * T2; // mean longitude, degree
            double DL = (1.914600 - 0.004817 * T - 0.000014 * T2) * Math.Sin(dr * M);
            DL = DL + (0.019993 - 0.000101 * T) * Math.Sin(dr * 2 * M) + 0.000290 * Math.Sin(dr * 3 * M);
            double L = L0 + DL; // true longitude, degree
            // obtain apparent longitude by correcting for nutation and aberration
            double omega = 125.04 - 1934.136 * T;
            L = L - 0.00569 - 0.00478 * Math.Sin(omega * dr);
            L = L * dr;
            L = L - Math.PI * 2 * (INT(L / (Math.PI * 2))); // Normalize to (0, 2*PI)
            return INT(L / Math.PI * 6);
        }

        private long getLunarMonth11(int yy, int timeZone)
        {
            long off = jdFromDate(31, 12, yy) - 2415021;
            long k = INT(off / 29.530588853);
            long nm = getNewMoonDay(k, timeZone);
            long sunLong = getSunLongitude(nm, timeZone); // sun longitude at local midnight
            if (sunLong >= 9)
            {
                nm = getNewMoonDay(k - 1, timeZone);
            }
            return nm;
        }

        private int getLeapMonthOffset(long a11, int timeZone)
        {
            long k = INT((a11 - 2415021.076998695) / 29.530588853 + 0.5);
            long last = 0;
            int i = 1; // We start with the month following lunar month 11
            long arc = getSunLongitude(getNewMoonDay(k + i, timeZone), timeZone);
            do
            {
                last = arc;
                i = i + 1;
                arc = getSunLongitude(getNewMoonDay(k + i, timeZone), timeZone);
            } while (arc != last && i < 14);
            return i - 1;
        }
    }
}
