using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00733
{
    class ManagerSql
    {
        public List<Mrs00733RDO> GetMedicine()
        {
            List<Mrs00733RDO> result = new List<Mrs00733RDO>();
            try
            {
                string query = "--danh sach thuoc + thuoc xung dot \n";
                query += "select distinct\n";
                query += "mt.medicine_type_code, mt.medicine_type_name, aingr.ACTIVE_INGREDIENT_CODE active_ingr_bhyt_code,aingr.ACTIVE_INGREDIENT_NAME active_ingr_bhyt_code, \n";
                query += "conf.active_ingredient_code, conf.active_ingredient_name, \n";
                query += "ai.description, ig.interactive_grade_name as GRADE \n";
                query += "from his_acin_interactive ai \n";
                query += "join his_active_ingredient conf on ai.conflict_id = conf.id \n";
                query += "join his_active_ingredient aingr on ai.active_ingredient_id = aingr.id \n";
                query += "left join his_interactive_grade ig on ai.interactive_grade_id = ig.id \n";
                query += "left join his_medicine_type_acin mta on mta.active_ingredient_id = ai.active_ingredient_id \n";
                query += "left join his_medicine_type mt on mta.medicine_type_id = mt.id \n";
                query += "where 1=1 \n";

                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00733RDO>(query);
                LogSystem.Info("SQL query: " + query);
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
