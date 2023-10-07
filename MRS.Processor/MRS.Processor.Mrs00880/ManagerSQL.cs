using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00880
{
    class ManagerSQL
    {
        List<string> _31Days = new List<string>() {"01", "03","05","07","08","10","12"};
        List<string> _30Days = new List<string>() {"04", "06","09","11"};
        long timeFrom = 0;
        long timeTo = 0;
        internal List<Mrs00880RDO> GetListRdo(Mrs00880Filter filter)
        {
            List<Mrs00880RDO> result = new List<Mrs00880RDO>();
            try
            {
                long monthFilter = filter.MONTH ?? 0;
                CheckMonthFilter(monthFilter);
                string query = "select \n";
                query += "a.branch_code,\n";
                query += "a.branch_name,\n";
                query += "e.title,\n";
                query += "e.id employee_id, \n";
                query += "sr.execute_loginname,\n";
                query += "sr.execute_username,\n";
                query += "ss.tdl_service_code,\n";
                query += "ss.tdl_service_name,\n";
                query += "sr.service_req_code\n";
                query += "from v_his_service_req sr \n";
                query += "join v_his_sere_serv ss on sr.id = ss.service_req_id\n";
                query += "join \n";
                query += "(\n";
                query += "select distinct t.id treatment_id, b.branch_code, b.branch_name from his_treatment t\n";
                query += "join his_branch b on t.branch_id = b.id\n";
                query += "join his_service_req s on t.id = s.treatment_id\n";     
                query += "where t.branch_id is not null\n";
                query += "and t.is_delete is null\n";
                if (monthFilter != null && monthFilter > 0)
                {
                    query += string.Format("and s.intruction_time between {0} and {1}\n", timeFrom, timeTo);
                }
                else
                {
                    query += string.Format("and s.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                }
                query += ") a on sr.treatment_id = a.treatment_id\n";
                query += "join his_employee e on sr.execute_loginname = e.loginname\n";
                query += "where 1=1\n";
                
                query += "and (ss.tdl_service_name like 'Điện tim thường%'\n";
                query += "or ss.tdl_service_name like 'Holter%'\n";
                query += "or ss.tdl_service_name like 'Điện cơ%'\n";
                query += "or ss.tdl_service_name like 'Điện não đồ%'\n";
                query += "or ss.tdl_service_name like 'Đo chức năng hô hấp%'\n";
                query += "or ss.tdl_service_name like 'Ghi điện cơ%'\n";
                query += "or ss.tdl_service_name like 'Ghi điện não%'\n";
                query += ")\n";
                query += string.Format("and sr.service_req_stt_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += "and sr.is_no_execute is null\n";
                if (monthFilter != null && monthFilter > 0)
                {
                    query += string.Format("and sr.intruction_time between {0} and {1}\n", timeFrom, timeTo);
                }
                else
                {
                    query += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                }
                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00880RDO>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }

        private void CheckMonthFilter(long? takeMonth)
        {
            if (takeMonth != null && takeMonth > 0)
            {
                string month = takeMonth.ToString().Substring(4, 2);
                string year = takeMonth.ToString().Substring(0, 4);
                string date = year + month + "00000000";
                timeFrom = (Convert.ToInt64(date)) + 1000000;
                if (_31Days.Contains(month))
                {
                    timeTo = (Convert.ToInt64(date)) + 31235959;
                }
                else if (_30Days.Contains(month))
                {
                    timeTo = (Convert.ToInt64(date)) + 30235959;
                }
                else if (month == "02")
                {
                    if (Convert.ToInt32(year) % 4 == 0)
                    {
                        timeTo = (Convert.ToInt64(date)) + 28235959;
                    }
                    else
                    {
                        timeTo = (Convert.ToInt64(date)) + 29235959;
                    }
                }
            }
        }
    }
}
