using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisSeseDepoRepay()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("SERE_SERV_DEPOSIT_ID");//ko cho sua
            pies.Add("REPAY_ID");//ko cho sua
            pies.Add("TDL_PATIENT_TYPE_ID");//ko cho sua
            pies.Add("AMOUNT");//ko cho sua
            pies.Add("TDL_TREATMENT_ID");//ko cho sua
            pies.Add("TDL_SERVICE_REQ_ID");//ko cho sua
            pies.Add("TDL_SERVICE_ID");//ko cho sua
            pies.Add("TDL_VIR_PRICE");//ko cho sua
            pies.Add("TDL_VIR_PRICE_NO_ADD_PRICE");//ko cho sua
            pies.Add("TDL_VIR_HEIN_PRICE");//ko cho sua
            pies.Add("TDL_VIR_TOTAL_PRICE");//ko cho sua
            pies.Add("TDL_VIR_TOTAL_HEIN_PRICE");//ko cho sua
            pies.Add("TDL_VIR_TOTAL_PATIENT_PRICE");//ko cho sua
            pies.Add("TDL_AMOUNT");//ko cho sua
            pies.Add("TDL_IS_EXPEND");//ko cho sua
            pies.Add("TDL_HEIN_PRICE");//ko cho sua
            pies.Add("TDL_HEIN_LIMIT_PRICE");//ko cho sua
            properties[typeof(HIS_SESE_DEPO_REPAY)] = pies;
        }
    }
}
