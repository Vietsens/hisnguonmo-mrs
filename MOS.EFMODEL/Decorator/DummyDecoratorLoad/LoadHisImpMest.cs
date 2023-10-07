using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisImpMest()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_IMP_MEST).GetProperty("IMP_MEST_CODE"));

            properties[typeof(HIS_IMP_MEST)] = pies;
        }
    }
}
