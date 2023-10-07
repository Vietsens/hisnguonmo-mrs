using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00553
{
    class ManagerSql
    {
        public List<HIS_MEDICINE> GetMedicine(Mrs00553Filter filter)
        {
            List<HIS_MEDICINE> result = null;
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "me.* \n";
                query += "FROM HIS_MEDICINE me \n";

                query += "WHERE 1=1 \n";


                query += "AND me.is_delete = 0 \n";
                query += string.Format("AND me.imp_time BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.IMP_SOURCE_IDs != null)
                {
                    query += string.Format("AND me.imp_source_id in ({0}) \n", string.Join(",",filter.IMP_SOURCE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDICINE>(query);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
        public List<HIS_MATERIAL> GetMaterial(Mrs00553Filter filter)
        {
            List<HIS_MATERIAL> result = null;
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "me.* \n";
                query += "FROM HIS_MATERIAL me \n";

                query += "WHERE 1=1 \n";


                query += "AND me.is_delete = 0 \n";
                query += string.Format("AND me.imp_time BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.IMP_SOURCE_IDs != null)
                {
                    query += string.Format("AND me.imp_source_id in ({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MATERIAL>(query);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
