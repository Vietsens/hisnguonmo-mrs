using SAR.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace SAR.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadSarReportCalendar()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(SAR_REPORT_CALENDAR).GetProperty("REPORT_CALENDAR_CODE"));
            properties[typeof(SAR_REPORT_CALENDAR)] = pies;
        }
    }
}
