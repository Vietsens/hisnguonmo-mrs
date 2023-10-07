using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisServiceReq()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("NUM_ORDER");
            pies.Add("SERVICE_REQ_TYPE_ID"); //ko cho phep update service_req_type_id
            pies.Add("TDL_TREATMENT_CODE"); //ko cho phep update TREATMENT_CODE
            pies.Add("TREATMENT_ID"); //ko cho phep update TREATMENT_ID
            pies.Add("INTRUCTION_DATE"); //ko cho phep update INTRUCTION_DATE

            properties[typeof(HIS_SERVICE_REQ)] = pies;
        }
    }
}
