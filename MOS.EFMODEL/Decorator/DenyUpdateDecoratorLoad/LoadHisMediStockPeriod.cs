using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisMediStockPeriod()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("MEDI_STOCK_ID"); //ko cho sua MEDI_STOCK_ID
            pies.Add("PREVIOUS_ID"); //ko cho sua PREVIOUS_ID
            pies.Add("COUNT_MEDICINE_TYPE"); //ko cho sua COUNT_MEDICINE_TYPE
            pies.Add("COUNT_MATERIAL_TYPE"); //ko cho sua COUNT_MATERIAL_TYPE
            pies.Add("COUNT_IMP_MEST"); //ko cho sua COUNT_IMP_MEST
            pies.Add("COUNT_EXP_MEST"); //ko cho sua COUNT_EXP_MEST


            properties[typeof(HIS_MEDI_STOCK_PERIOD)] = pies;
        }
    }
}
