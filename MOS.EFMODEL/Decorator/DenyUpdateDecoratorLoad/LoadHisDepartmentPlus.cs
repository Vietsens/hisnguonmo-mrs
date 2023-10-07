using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisDepartment()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            pies.Add("BRANCH_ID"); //khong cho phep thay doi chi nhanh cua khoa, dieu nay se dan den sai du lieu

            properties[typeof(HIS_DEPARTMENT)] = pies;
        }
    }
}
