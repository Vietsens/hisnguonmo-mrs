using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisExpMest()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("EXP_MEST_CODE");
            pies.Add("EXP_MEST_TYPE_ID"); //ko cho phep cap nhat exp_mest_type_id
            pies.Add("SERVICE_REQ_ID"); //ko cho phep cap nhat service_req_id
            pies.Add("TDL_SERVICE_REQ_CODE"); //ko cho phep cap nhat TDL_SERVICE_REQ_CODE
            pies.Add("TDL_TREATMENT_ID");//ko cho sua
            pies.Add("MEDI_STOCK_ID");// ko cho sua
            pies.Add("BILL_ID"); //ko cho sua thong tin hoa don
            pies.Add("CREATE_DATE");
            pies.Add("PRESCRIPTION_ID");

            properties[typeof(HIS_EXP_MEST)] = pies;
        }
    }
}
