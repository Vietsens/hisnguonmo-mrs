using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00439
{
    public partial class Mrs00439RDOManager : BusinessBase
    {
        public List<long> GetMrs00439RDOByIntructionTime(long? INTRUCTION_TIME_FROM, long? INTRUCTION_TIME_TO)
        {
            try
            {
                List<long> result = new List<long>();
                string query = "";
                query += "SELECT DISTINCT(S.SERVICE_ID) FROM HIS_SERE_SERV S ";
                query += "WHERE S.IS_NO_EXECUTE IS NULL ";
                query += "AND S.IS_DELETE = 0 ";
                query += "AND S.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND S.TDL_SERVICE_TYPE_ID NOT IN (1,4,5,6,7,8,11,12,13,14,15) ";
                query += "AND NOT EXISTS ( ";
                query += "SELECT 1 FROM HIS_SERE_SERV S1  "; 
                query += "WHERE S1.IS_NO_EXECUTE IS NULL ";
                query += "AND S1.IS_DELETE = 0 ";
                query += "AND S1.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND S1.TDL_SERVICE_TYPE_ID NOT IN (1,4,5,6,7,8,11,12,13,14,15) ";
                query += "AND S1.ID<>S.ID ";
                query += "AND S1.SERVICE_ID=S.SERVICE_ID ";
                if (INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND S1.TDL_INTRUCTION_TIME < {0} ", INTRUCTION_TIME_FROM);
                }
                query += ") ";
                if (INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND S.TDL_INTRUCTION_TIME >= {0} ", INTRUCTION_TIME_FROM);
                }
                if (INTRUCTION_TIME_FROM != null)
                {
                    query += string.Format("AND S.TDL_INTRUCTION_TIME < {0} ", INTRUCTION_TIME_TO);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = DAOWorker.SqlDAO.GetSql<long>(query);
                    return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
