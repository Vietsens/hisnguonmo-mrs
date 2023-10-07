using Inventec.Common.Logging;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00740
{
    internal partial class ManagerSql : BusinessBase
    {
        internal List<Mrs00740RDO> GetData(Mrs00740Filter filter)
        {
            List<Mrs00740RDO> result = new List<Mrs00740RDO>();
            try
            {
                string query = "--start process \n";
                query += "select imb.*, bg.district_name_blood, nvl(bg.execute_time,0) execute_time, imt.imp_mest_type_name \n";
                query += "from v_his_imp_mest_blood imb \n";
                query += "left join his_blood_giver bg on imb.imp_mest_id = bg.imp_mest_id \n";
                query += "join his_imp_mest_type imt on imb.imp_mest_type_id = imt.id \n";
                query += "where 1=1 \n";
                query += string.Format("and imb.imp_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                LogSystem.Info("START QUERY: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00740RDO>(query);
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
