using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisServiceReq()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_SERVICE_REQ).GetProperty("SERVICE_REQ_CODE"));

            properties[typeof(HIS_SERVICE_REQ)] = pies;
        }
    }
}
