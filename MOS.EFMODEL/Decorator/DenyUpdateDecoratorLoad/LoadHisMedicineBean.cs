using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisMedicineBean()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            /*
             * Khong cho phep cap nhat so luong cua bean.
             * TH1: can sua so luong giam xuong ==> can tach bean va xoa bean duoc tach ra
             * TH1: can sua so luong tang len ==> tao them bean moi
             */
            pies.Add("AMOUNT");
            pies.Add("MEDICINE_ID");//ko cho phep update MEDICINE_ID
			pies.Add("IS_TH");//ko cho phep update IS_TH
			pies.Add("TDL_MEDICINE_TYPE_ID");//ko cho phep update TDL_MEDICINE_TYPE_ID

            properties[typeof(HIS_MEDICINE_BEAN)] = pies;
        }
    }
}
