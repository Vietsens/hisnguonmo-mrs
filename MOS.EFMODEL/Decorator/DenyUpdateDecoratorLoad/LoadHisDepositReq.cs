using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisDepositReq()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("TREATMENT_ID"); //ko cho sua
            pies.Add("DEPOSIT_REQ_CODE"); //ko cho sua

            properties[typeof(HIS_DEPOSIT_REQ)] = pies;
        }
    }
}
