using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00673
{
    public class ManagerSql
    {
        public DataTable GetSum(Mrs00673Filter filter, string query)
        {
            DataTable result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.SERVICE_TYPE_IDs != null) ? string.Join(",", filter.SERVICE_TYPE_IDs) : "''"
, (filter.EXACT_PARENT_SERVICE_IDs != null) ? string.Join(",", filter.EXACT_PARENT_SERVICE_IDs) : "''"
, (filter.REQUEST_DEPARTMENT_ID != null) ? filter.REQUEST_DEPARTMENT_ID.ToString() : "''"
, (filter.REQUEST_ROOM_ID != null) ? filter.REQUEST_ROOM_ID.ToString() : "''"
, (filter.EXECUTE_DEPARTMENT_ID != null) ? filter.EXECUTE_DEPARTMENT_ID.ToString() : "''"
, (filter.EXECUTE_ROOM_ID != null) ? filter.EXECUTE_ROOM_ID.ToString() : "''"
, (filter.REQUEST_ROOM_IDs != null) ? string.Join(",", filter.REQUEST_ROOM_IDs) : "''"
, (filter.EXECUTE_ROOM_IDs != null) ? string.Join(",", filter.EXECUTE_ROOM_IDs) : "''"
, (filter.REQUEST_LOGINNAMEs != null) ? "'" + string.Join("','", filter.REQUEST_LOGINNAMEs) + "'" : "''"
, (filter.EXECUTE_LOGINNAMEs != null) ? "'" + string.Join("','", filter.EXECUTE_LOGINNAMEs) + "'" : "''"
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
