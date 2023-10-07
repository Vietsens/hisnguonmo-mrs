using SDA.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace SDA.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadSdaGroup()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(SDA_GROUP).GetProperty("GROUP_CODE"));
            properties[typeof(SDA_GROUP)] = pies;
        }
    }
}
