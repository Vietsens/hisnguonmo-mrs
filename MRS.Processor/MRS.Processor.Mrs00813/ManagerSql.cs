using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00813
{
    public class ManagerSql
    {
        public List<PrintLogUnique> GetPrintLog(Mrs00813Filter filter)
        {
            List<PrintLogUnique> result = new List<PrintLogUnique>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select\n";
            query += "unique_code,\n";
            query += "print_time,\n";
            query += "num_order\n";
            query += "from sar_print_log\n";
            query += "where print_type_code='Mps000308'\n";
            query += "and unique_code is not null\n";
            //query += string.Format("and create_time >= {0}\n", filter.TIME_FROM);
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SAR.DAO.Sql.SqlDAO().GetSql<PrintLogUnique>(query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu");

            return result;
        }
    }
}
