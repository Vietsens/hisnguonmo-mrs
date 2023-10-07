using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisMediStock()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("ROOM_ID");
            pies.Add("IS_CABINET"); //ko cho phep sua truong la tu truc
            pies.Add("IS_BUSINESS");//ko cho phep sua truong la ko kinh doanh

            properties[typeof(HIS_MEDI_STOCK)] = pies;
        }
    }
}
