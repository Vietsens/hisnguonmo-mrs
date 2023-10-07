using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00730
{
    internal class ManagerSql
    {
        internal List<Mrs00730ServiceRDO> GetService()
        {
            List<Mrs00730ServiceRDO> result = new List<Mrs00730ServiceRDO>();
            try
            {
                string query = "select distinct pr.id parent_id, pr.service_name parent_name, pr.service_code parent_code\n";
                query += "from his_service pr join his_service sv on pr.id = sv.parent_id\n";
                query += string.Format("where pr.service_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC);

                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00730ServiceRDO>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return result;
            }
            return result;
        }
    }
}
