using SDA.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace SDA.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadSdaConfig()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");

            properties[typeof(SDA_CONFIG)] = pies;
        }
    }
}
