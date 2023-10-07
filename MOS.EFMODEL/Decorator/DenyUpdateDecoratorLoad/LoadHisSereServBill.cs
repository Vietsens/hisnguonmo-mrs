using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisSereServBill()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("TDL_TREATMENT_ID"); //ko cho phep update TDL_TREATMENT_ID
            pies.Add("TDL_SERVICE_REQ_ID"); //ko cho phep update TDL_SERVICE_REQ_ID
            pies.Add("SERE_SERV_ID");//ko cho phep update SERE_SERV_ID
            pies.Add("BILL_ID");//ko cho phep update BILL_ID

            properties[typeof(HIS_SERE_SERV_BILL)] = pies;
        }
    }
}
