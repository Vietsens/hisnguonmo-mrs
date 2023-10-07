using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00668
{
    public class ManagerSql
    {
        public List<TREATMENT> GetTreatment(Mrs00668Filter filter, string query)
        {
            List<TREATMENT> result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.CASHIER_LOGINNAME != null) ? "'" + filter.CASHIER_LOGINNAME + "'" : "''"

, (filter.EXACT_CASHIER_ROOM_ID != null) ? filter.EXACT_CASHIER_ROOM_ID.ToString() : "''"

, (filter.ACCOUNT_BOOK_ID != null) ? filter.ACCOUNT_BOOK_ID.ToString() : "''");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        public List<DEPOSIT> GetDeposit(Mrs00668Filter filter, string query)
        {
            List<DEPOSIT> result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.CASHIER_LOGINNAME != null) ? "'" + filter.CASHIER_LOGINNAME + "'" : "''"

, (filter.EXACT_CASHIER_ROOM_ID != null) ? filter.EXACT_CASHIER_ROOM_ID.ToString() : "''"

, (filter.ACCOUNT_BOOK_ID != null) ? filter.ACCOUNT_BOOK_ID.ToString() : "''");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<DEPOSIT>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<BILL> GetBill(Mrs00668Filter filter, string query)
        {
            List<BILL> result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.CASHIER_LOGINNAME != null) ? "'" + filter.CASHIER_LOGINNAME + "'" : "''"

, (filter.EXACT_CASHIER_ROOM_ID != null) ? filter.EXACT_CASHIER_ROOM_ID.ToString() : "''"

, (filter.ACCOUNT_BOOK_ID != null) ? filter.ACCOUNT_BOOK_ID.ToString() : "''");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<BILL>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
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
