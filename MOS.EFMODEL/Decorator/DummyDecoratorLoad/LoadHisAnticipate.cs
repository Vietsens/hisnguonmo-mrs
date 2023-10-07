using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisAnticipate()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_ANTICIPATE).GetProperty("ANTICIPATE_CODE"));

            properties[typeof(HIS_ANTICIPATE)] = pies;
        }
    }
}
