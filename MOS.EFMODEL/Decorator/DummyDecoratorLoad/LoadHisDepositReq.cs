using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisDepositReq()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_DEPOSIT_REQ).GetProperty("DEPOSIT_REQ_CODE"));

            properties[typeof(HIS_DEPOSIT_REQ)] = pies;
        }
    }
}
