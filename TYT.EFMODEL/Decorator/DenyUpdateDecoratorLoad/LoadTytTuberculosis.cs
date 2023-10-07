using TYT.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace TYT.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadTytTuberculosis()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");

            properties[typeof(TYT_TUBERCULOSIS)] = pies;
        }
    }
}
