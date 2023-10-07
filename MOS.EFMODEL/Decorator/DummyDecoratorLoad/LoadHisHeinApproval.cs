using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisHeinApproval()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_HEIN_APPROVAL).GetProperty("HEIN_APPROVAL_CODE"));

            properties[typeof(HIS_HEIN_APPROVAL)] = pies;
        }
    }
}
