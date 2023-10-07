using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00319
{
    public class Age
    {
        public static int? CalculateAge(long ageNumber)
        {
            int tuoi; 
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageNumber) ?? DateTime.MinValue; 
                TimeSpan diff = DateTime.Now - dtNgSinh; 
                long tongsogiay = diff.Ticks; 
                if (tongsogiay < 0)
                {
                    tuoi = 0; 
                    return 0; 
                }
                DateTime newDate = new DateTime(tongsogiay); 

                int nam = newDate.Year - 1; 
                int thang = newDate.Month - 1; 
                int ngay = newDate.Day - 1; 
                int gio = newDate.Hour; 
                int phut = newDate.Minute; 
                int giay = newDate.Second; 

                if (nam > 0)
                {
                    tuoi = nam; 
                }
                else
                {
                    tuoi = 0; 
                }
                return tuoi; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex); 
                return null; 
            }
        }
    }

}
