using LIS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00700
{
    class ManagerSql
    {
        internal List<V_LIS_SAMPLE> GetLisSample(List<string> serviceReqCodes)
        {
            List<V_LIS_SAMPLE> result = new List<V_LIS_SAMPLE>();
            try
            {
                if (serviceReqCodes != null && serviceReqCodes.Count > 0)
                {
                    string queryBase = "SELECT * FROM V_LIS_SAMPLE WHERE SERVICE_REQ_CODE IN ('{0}') AND SAMPLE_STT_ID IN (2,3,4) ";

                    int skip = 0;
                    while (serviceReqCodes.Count - skip > 0)
                    {
                        var lstCode = serviceReqCodes.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        string query = string.Format(queryBase, string.Join("','", lstCode));
                        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                        var sample = new LIS.DAO.Sql.SqlDAO().GetSql<V_LIS_SAMPLE>(query);
                        if (sample != null && sample.Count > 0)
                        {
                            result.AddRange(sample);
                        }
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

        internal List<V_LIS_RESULT> GetLisResult(List<long> sampleIds)
        {
            List<V_LIS_RESULT> result = new List<V_LIS_RESULT>();
            try
            {
                if (sampleIds != null && sampleIds.Count > 0)
                {
                    string queryBase = "SELECT * FROM V_LIS_RESULT WHERE SAMPLE_ID IN ({0})";

                    int skip = 0;
                    while (sampleIds.Count - skip > 0)
                    {
                        var lstId = sampleIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        string query = string.Format(queryBase, string.Join(",", lstId));
                        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                        var sample = new LIS.DAO.Sql.SqlDAO().GetSql<V_LIS_RESULT>(query);
                        if (sample != null && sample.Count > 0)
                        {
                            result.AddRange(sample);
                        }
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
    }
}
