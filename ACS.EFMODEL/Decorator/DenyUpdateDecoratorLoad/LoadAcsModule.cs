using ACS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace ACS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadAcsModule()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");

            properties[typeof(ACS_MODULE)] = pies;
        }
    }
}
