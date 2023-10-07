using SDA.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace SDA.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadSdaTrouble()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(SDA_TROUBLE).GetProperty("TROUBLE_CODE"));
            properties[typeof(SDA_TROUBLE)] = pies;
        }
    }
}
