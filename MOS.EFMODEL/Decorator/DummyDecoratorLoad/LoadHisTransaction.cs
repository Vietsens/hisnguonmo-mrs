using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisTransaction()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_TRANSACTION).GetProperty("TRANSACTION_CODE"));
            
            properties[typeof(HIS_TRANSACTION)] = pies;
        }
    }
}
