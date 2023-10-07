using Inventec.Common.Logging;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Text;

namespace MRS.Processor.Mrs00887
{
    class ManagerSql
    {
        internal List<Mrs00887RDO> GetRdo(Mrs00887Filter filter)
        {
            List<Mrs00887RDO> result = new List<Mrs00887RDO>();
            try
            {
                string query = "select \n";
                query += "t.*, t.in_time MONTH, i.icd_group_id \n";
                query += "from v_his_treatment t \n";
                query += "join his_icd i on t.icd_code = i.icd_code \n";
                query += "where 1=1 \n";
                query += "and t.is_delete = 0 \n";
                query += string.Format("and t.in_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00887RDO>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }
       
    }
}
