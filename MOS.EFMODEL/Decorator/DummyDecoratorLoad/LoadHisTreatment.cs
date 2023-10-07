using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisTreatment()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_TREATMENT).GetProperty("TREATMENT_CODE"));
            
            properties[typeof(HIS_TREATMENT)] = pies;
        }
    }
}
