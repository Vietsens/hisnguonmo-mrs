using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00675
{
    class ManagerSql
    {
        internal List<HIS_SERE_SERV> GetSS(Mrs00675Filter filter)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.* ";
                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_SERVICE_REQ SERE ON SS.SERVICE_REQ_ID = SERE.ID ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE = 0 ";

                if (filter.TIME_TO.HasValue)
                {
                    query += string.Format("AND SERE.INTRUCTION_TIME <= {0} ", filter.TIME_TO.Value);
                }

                if (filter.TIME_FROM.HasValue)
                {
                    query += string.Format("AND SERE.INTRUCTION_TIME >= {0} ", filter.TIME_FROM.Value);
                }

                if (filter.DEPARTMENT_IDs != null && filter.DEPARTMENT_IDs.Count > 0)
                {
                    query += string.Format("AND SERE.REQUEST_DEPARTMENT_ID IN ({0}) ", string.Join(",", filter.DEPARTMENT_IDs));
                }

                if (filter.ROOM_IDs != null && filter.ROOM_IDs.Count > 0)
                {
                    query += string.Format("AND SERE.REQUEST_ROOM_ID IN ({0}) ", string.Join(",", filter.ROOM_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);
                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
