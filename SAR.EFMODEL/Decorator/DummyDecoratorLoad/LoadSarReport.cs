using SAR.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace SAR.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadSarReport()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(SAR_REPORT).GetProperty("REPORT_CODE"));
            properties[typeof(SAR_REPORT)] = pies;
        }
    }
}
