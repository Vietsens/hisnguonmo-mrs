using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00667
{
    public class ManagerSql
    {
        public System.Data.DataTable GetExmm(Mrs00667Filter filter, string query)
        {
            System.Data.DataTable result = null;
            try
            {

                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.EXECUTE_DEPARRTMENT_ID != null) ? filter.EXECUTE_DEPARRTMENT_ID.ToString() : "''"
, (filter.EXECUTE_ROOM_ID != null) ? string.Join(",", filter.EXECUTE_ROOM_ID).ToString() : "''");
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
