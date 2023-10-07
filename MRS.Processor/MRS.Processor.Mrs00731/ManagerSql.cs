using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00731
{
    internal class ManagerSql
    {
        
        internal List<Mrs00731RDO> Get6MonthsOf2020()
        {
            List<Mrs00731RDO> result = new List<Mrs00731RDO>();
            try
            {
                string query = "select \n";
                query += "ss.id,\n";
                query += "trea.tdl_treatment_type_id,\n";
                query += "ss.tdl_intruction_time,\n";
                query += "st.service_type_code,";
                query += "st.service_type_name,";
                query += "pr.service_name,\n";
                query += "pr.service_code\n";
                
                query += "from his_sere_serv ss\n";
                query += "join his_treatment trea on ss.tdl_treatment_id = trea.id\n";
                query += "join his_service sv on ss.service_id = sv.id\n";
                query += "join his_service pr on sv.parent_id = pr.id\n";
                query += "join his_service_type st on ss.tdl_service_type_id = st.id\n";
                query += "where 1=1\n";
                query += "and (ss.is_delete is null or ss.is_delete = 0)\n";
                query += string.Format("and ss.tdl_intruction_time between 20200101000000 and 20200630235959");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00731RDO>(query);
                LogSystem.Info("SQL: " + query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
        }

        internal List<Mrs00731RDO> Get6MonthsOf2021()
        {
            List<Mrs00731RDO> result = new List<Mrs00731RDO>();
            try
            {
                string query = "select \n";
                query += "ss.id,\n";
                query += "trea.tdl_treatment_type_id,\n";
                query += "ss.tdl_intruction_time,\n";
                query += "st.service_type_code,";
                query += "st.service_type_name,";
                query += "pr.service_name,\n";
                query += "pr.service_code\n";

                query += "from his_sere_serv ss\n";
                query += "join his_treatment trea on ss.tdl_treatment_id = trea.id\n";
                query += "join his_service sv on ss.service_id = sv.id\n";
                query += "join his_service pr on sv.parent_id = pr.id\n";
                query += "join his_service_type st on ss.tdl_service_type_id = st.id\n";
                query += "where 1=1\n";
                query += "and (ss.is_delete is null or ss.is_delete = 0)\n";
                query += string.Format("and ss.tdl_intruction_time between 20210101000000 and 20210630235959");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00731RDO>(query);
                LogSystem.Info("SQL: " + query);
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
