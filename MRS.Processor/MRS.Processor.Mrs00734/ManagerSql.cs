using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00734
{
    class ManagerSql
    {
        public List<Mrs00734RDO> GetTimeData(Mrs00734Filter filter)
        {
            List<Mrs00734RDO> result = new List<Mrs00734RDO>();
            try
            {
                string query = "--danh sach user hoat dong \n";
                query += "select \n";
                query += "tok.login_name, tok.user_name, tok.login_time, \n";
                query += "al.activity_type_id, al.activity_time, \n";
                query += "at.activity_type_code, at.activity_type_name \n";
                query += "from acs_rs.acs_activity_log al \n";
                query += "join acs_rs.acs_activity_type at on al.activity_type_id = at.id \n";
                query += "join acs_rs.acs_token tok on al.loginname = tok.login_name \n";
                query += "where 1=1 \n";
                query += string.Format("and tok.login_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                query += "order by tok.login_time \n";
                LogSystem.Info("SQL query: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00734RDO>(query);
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
        }
    }
}
