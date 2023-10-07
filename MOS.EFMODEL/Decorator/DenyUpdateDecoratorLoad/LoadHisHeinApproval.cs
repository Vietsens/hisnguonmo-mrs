using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisHeinApproval()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("TREATMENT_ID"); //Ko cho phep cap nhat treatment_id
            pies.Add("HEIN_APPROVAL_CODE"); //Ko cho phep cap nhat HEIN_APPROVAL_CODE

            properties[typeof(HIS_HEIN_APPROVAL)] = pies;
        }
    }
}
