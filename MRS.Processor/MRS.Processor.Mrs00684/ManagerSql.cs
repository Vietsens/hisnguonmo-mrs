using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00684
{
    public class ManagerSql
    {
        public DataTable GetSum(Mrs00684Filter filter, string query)
        {
            DataTable result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.CASHIER_LOGINNAME != null) ? "'" + filter.CASHIER_LOGINNAME + "'" : "''"

, (filter.EXACT_CASHIER_ROOM_ID != null) ? filter.EXACT_CASHIER_ROOM_ID.ToString() : "''"

, (filter.ACCOUNT_BOOK_ID != null) ? filter.ACCOUNT_BOOK_ID.ToString() : "''"

, (filter.ACCOUNT_BOOK_IDs != null) ? string.Join(",", filter.ACCOUNT_BOOK_IDs) : "''"

, (filter.IS_TREAT_IN != null) ? (filter.IS_TREAT_IN == true ? "1" : "0") : "''"
);
                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

    }
}
