using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00701
{
    class ManagerSql
    {
        internal List<HIS_SERE_SERV> GetLisSereServ(Mrs00701Filter filter)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                string queryBase = "SELECT ";
                queryBase += "* ";
                queryBase += "FROM HIS_SERE_SERV SS ";
                queryBase += "WHERE 1=1 ";
                if (filter.HAS_BILL==true)
                {
                queryBase += "AND EXISTS (SELECT 1 FROM HIS_SERE_SERV_BILL WHERE SS.ID = SERE_SERV_ID AND (IS_CANCEL IS NULL OR IS_CANCEL <> 1)) ";
                }
                queryBase += string.Format("AND TDL_SERVICE_TYPE_ID = {0} ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA);
                queryBase += "AND SERVICE_REQ_ID IS NOT NULL ";
                queryBase += "AND (IS_NO_EXECUTE IS NULL OR IS_NO_EXECUTE <> 1) ";
                queryBase += "AND (IS_EXPEND IS NULL OR IS_EXPEND <> 1) ";
                queryBase += "AND IS_DELETE=0 ";

                if (filter.TIME_TO.HasValue)
                {
                    queryBase += string.Format("AND SS.TDL_INTRUCTION_TIME <= {0} ", filter.TIME_TO.Value);
                }

                if (filter.TIME_FROM.HasValue)
                {
                    queryBase += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TIME_FROM.Value);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + queryBase);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(queryBase);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<HIS_SERE_SERV_EXT> GetLisSereServExt(List<long> sereServIds)
        {
            List<HIS_SERE_SERV_EXT> result = new List<HIS_SERE_SERV_EXT>();
            try
            {
                string queryBase = "SELECT * FROM HIS_SERE_SERV_EXT SS WHERE SERE_SERV_ID IN ({0}) AND NUMBER_OF_FILM IS NOT NULL";

                int skip = 0;
                while (sereServIds.Count - skip > 0)
                {
                    var lstId = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    string query = string.Format(queryBase, string.Join(",", lstId));
                    Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                    var ext = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV_EXT>(query);
                    if (ext != null && ext.Count > 0)
                    {
                        result.AddRange(ext);
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

        internal List<HIS_SERVICE> GetLisService(Mrs00701Filter castFilter)
        {
            List<HIS_SERVICE> result = new List<HIS_SERVICE>();
            try
            {
                string query = "SELECT * FROM HIS_SERVICE SV WHERE NUMBER_OF_FILM IS NOT NULL";

                    Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                    var ext = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE>(query);
                    if (ext != null && ext.Count > 0)
                    {
                        result.AddRange(ext);
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
