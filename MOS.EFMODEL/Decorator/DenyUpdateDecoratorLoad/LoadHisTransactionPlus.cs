using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DenyUpdateDecorator
    {
        private static void LoadHisTransaction()
        {
            List<string> pies = new List<string>();
            pies.Add("CREATOR");
            pies.Add("APP_CREATOR");
            pies.Add("CREATE_TIME");
            //ko cho sua cac thong tin sau, mot khi giao dich da duoc tao ra
            pies.Add("TREATMENT_ID");
            pies.Add("AMOUNT");
            pies.Add("ACCOUNT_BOOK_ID");
            pies.Add("TRANSACTION_TYPE_ID");
            pies.Add("TRANSACTION_CODE");
            pies.Add("TDL_TREATMENT_CODE");
            pies.Add("NUM_ORDER");
            pies.Add("AMOUNT");//ko cho sua
            pies.Add("KC_AMOUNT");//ko cho sua
            pies.Add("EXEMPTION");//ko cho sua
            pies.Add("TDL_BILL_FUND_AMOUNT");//ko cho sua
            pies.Add("TRANSACTION_DATE"); //TRANSACTION_DATE duoc trigger tao ra tu transaction_time

            properties[typeof(HIS_TRANSACTION)] = pies;
        }
    }
}
