using Inventec.Core;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00274
{
    class ManagerSql
    {
        internal static List<V_HIS_EXP_MEST> GetLisExpMest(Mrs00274Filter filter)
        {
            List<V_HIS_EXP_MEST> result = new List<V_HIS_EXP_MEST>();
            try
            {
                if (filter != null)
                {
                    string query = "SELECT * FROM V_HIS_EXP_MEST WHERE CREATE_TIME BETWEEN :TIME_FROM AND :TIME_TO AND BILL_ID IS NOT NULL ";

                    if (!String.IsNullOrWhiteSpace(filter.LOGINNAME_SALE))
                    {
                        query += string.Format(" AND REQ_LOGINNAME = '{0}'", filter.LOGINNAME_SALE);
                    }
                    if (!string.IsNullOrWhiteSpace(filter.REQUEST_LOGINNAME))
                    {
                        query += string.Format(" AND TDL_PRESCRIPTION_REQ_LOGINNAME = '{0}'", filter.REQUEST_LOGINNAME);
                    }

                    Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                    var sample = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_EXP_MEST>(query, filter.TIME_FROM, filter.TIME_TO);
                    if (sample != null && sample.Count > 0)
                    {
                        result.AddRange(sample);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<PrintLogUnique> GetPrintLog(Mrs00274Filter filter,long? minCreateExpMest)
        {
            List<PrintLogUnique> result = new List<PrintLogUnique>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select\n";
            query += "unique_code\n";
            query += "from sar_print_log\n";
            query += "where print_type_code='Mps000339'\n";
            query += "and unique_code is not null\n";
            query += string.Format("and create_time >= {0}\n", minCreateExpMest);
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SAR.DAO.Sql.SqlDAO().GetSql<PrintLogUnique>(query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00274");

            return result;
        }
    }
}
