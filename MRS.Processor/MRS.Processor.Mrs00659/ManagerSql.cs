using LIS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00659
{
    class ManagerSql
    {
        internal List<LIS_SAMPLE> GetLisSample(Mrs00659Filter filter)
        {
            List<LIS_SAMPLE> result = new List<LIS_SAMPLE>();
            try
            {
                string query = string.Format("SELECT * FROM LIS_SAMPLE WHERE INTRUCTION_TIME BETWEEN {0} and {1}",filter.TIME_FROM,filter.TIME_TO);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new LIS.DAO.Sql.SqlDAO().GetSql<LIS_SAMPLE>(query);
               
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
