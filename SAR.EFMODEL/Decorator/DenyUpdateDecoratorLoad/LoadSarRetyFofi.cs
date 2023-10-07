using SAR.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace SAR.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadSarRetyFofi()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");

            properties[typeof(SAR_RETY_FOFI)] = pies;
        }
    }
}
