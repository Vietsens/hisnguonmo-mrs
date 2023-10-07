using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisSereServTein()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("SERE_SERV_ID");//ko cho phep update SERE_SERV_ID

            properties[typeof(HIS_SERE_SERV_TEIN)] = pies;
        }
    }
}
