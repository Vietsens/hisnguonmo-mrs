using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisImpMest()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("IMP_MEST_CODE");
            pies.Add("TDL_TREATMENT_ID");
            pies.Add("MEDI_STOCK_ID");
            pies.Add("IMP_MEST_TYPE_ID");
            pies.Add("BID_ID"); //ko cho sua thong tin thau
            pies.Add("CREATE_DATE");
            pies.Add("IMP_DATE");//imp_date duoc trigger tao ra tu imp_time ==> ko cho phep cap nhat tu backend

            properties[typeof(HIS_IMP_MEST)] = pies;
        }
    }
}
