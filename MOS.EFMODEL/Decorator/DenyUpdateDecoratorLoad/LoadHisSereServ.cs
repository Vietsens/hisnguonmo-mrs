using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisSereServ()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("SERVICE_ID");//ko cho phep update service_id
            pies.Add("SERVICE_REQ_ID");//ko cho phep update service_req_id
            pies.Add("TDL_TREATMENT_CODE");//ko cho phep update treatment_code
            pies.Add("TDL_SERVICE_REQ_CODE");//ko cho phep update service_req_code

            properties[typeof(HIS_SERE_SERV)] = pies;
        }
    }
}
