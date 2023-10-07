using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisBidMedicineType()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("BID_ID");//ko cho sua

            properties[typeof(HIS_BID_MEDICINE_TYPE)] = pies;
        }
    }
}
