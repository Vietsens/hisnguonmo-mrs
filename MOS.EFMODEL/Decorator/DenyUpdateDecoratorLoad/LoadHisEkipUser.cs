using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisEkipUser()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("EKIP_ID");//ko cho phep update ekip_id cua ekip_user

            properties[typeof(HIS_EKIP_USER)] = pies;
        }
    }
}
