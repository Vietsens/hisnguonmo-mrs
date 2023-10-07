using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00677
{
    public class ManagerSql
    {
        public List<SURGERY> GetSuregry(Mrs00677Filter filter, string query)
        {
            List<SURGERY> result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.REQUEST_ROOM_ID != null) ? filter.REQUEST_ROOM_ID.ToString() : "''"
, (filter.REQUEST_DEPARTMENT_ID != null) ? filter.REQUEST_DEPARTMENT_ID.ToString() : "''"
, (filter.EXECUTE_ROOM_ID != null) ? filter.EXECUTE_ROOM_ID.ToString() : "''"
, (filter.EXECUTE_DEPARTMENT_ID != null) ? filter.EXECUTE_DEPARTMENT_ID.ToString() : "''"
);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SURGERY>(query);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", query));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable Get(Mrs00677Filter filter, string query)
        {
            DataTable result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.REQUEST_ROOM_ID != null) ? filter.REQUEST_ROOM_ID.ToString() : "''"
, (filter.REQUEST_DEPARTMENT_ID != null) ? filter.REQUEST_DEPARTMENT_ID.ToString() : "''"
, (filter.EXECUTE_ROOM_ID != null) ? filter.EXECUTE_ROOM_ID.ToString() : "''"
, (filter.EXECUTE_DEPARTMENT_ID != null) ? filter.EXECUTE_DEPARTMENT_ID.ToString() : "''"
);
                List<string> error = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref error);
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
