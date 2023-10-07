using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisExpMest()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_EXP_MEST).GetProperty("EXP_MEST_CODE"));

            properties[typeof(HIS_EXP_MEST)] = pies;
        }
    }
}
