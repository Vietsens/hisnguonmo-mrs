using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00601
{
    public class ManagerSql
    {
        public System.Data.DataTable GetSum(Mrs00601Filter filter, string query)
        {
            System.Data.DataTable result = null;
            try
            {
                query = string.Format(query, (filter.TRANSACTION_TIME_TO != null) ? filter.TRANSACTION_TIME_TO.ToString() : "''"
, (filter.TRANSACTION_TIME_FROM != null) ? filter.TRANSACTION_TIME_FROM.ToString() : "''"
, (filter.CASHIER_LOGINNAME != null) ? "'" + filter.CASHIER_LOGINNAME+"'" : "''"

, (filter.EXACT_CASHIER_ROOM_ID != null) ? filter.EXACT_CASHIER_ROOM_ID.ToString() : "''"

, (filter.ACCOUNT_BOOK_ID != null) ? filter.ACCOUNT_BOOK_ID.ToString() : "''"

, (filter.BRANCH_ID != null) ? filter.BRANCH_ID.ToString() : "''"

, (filter.PATIENT_TYPE_ID != null) ? filter.PATIENT_TYPE_ID.ToString() : "''"

, (filter.SESE_PATIENT_TYPE_ID != null) ? filter.SESE_PATIENT_TYPE_ID.ToString() : "''"

, (filter.TREATMENT_TYPE_IDs != null) ? string.Join(",", filter.TREATMENT_TYPE_IDs) : "''"

, (filter.IS_TREAT != null) ? filter.IS_TREAT.ToString() : "''"
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
