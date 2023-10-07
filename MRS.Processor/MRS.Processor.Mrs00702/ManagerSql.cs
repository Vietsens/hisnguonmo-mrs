using LIS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00702
{
    class ManagerSql
    {
        internal List<V_LIS_SAMPLE> GetLisSample(Mrs00702Filter filter)
        {
            List<V_LIS_SAMPLE> result = new List<V_LIS_SAMPLE>();
            try
            {
                if (filter != null)
                {
                    string queryBase = "SELECT * FROM V_LIS_SAMPLE WHERE 1=1 ";

                    if (filter.TIME_TO.HasValue)
                    {
                        queryBase += string.Format("AND SAMPLE_TIME <= {0} ", filter.TIME_TO.Value);
                    }

                    if (filter.TIME_FROM.HasValue)
                    {
                        queryBase += string.Format("AND SAMPLE_TIME >= {0} ", filter.TIME_FROM.Value);
                    }

                    Inventec.Common.Logging.LogSystem.Info("SQL: " + queryBase);
                    var sample = new LIS.DAO.Sql.SqlDAO().GetSql<V_LIS_SAMPLE>(queryBase);
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

        internal List<HIS_SERVICE_REQ> GetLisServiceReq(List<string> serviceReqCodes, long? patientTypeId)
        {
            List<HIS_SERVICE_REQ> result = null;
            try
            {
                if (serviceReqCodes != null && serviceReqCodes.Count > 0)
                {
                    result = new List<HIS_SERVICE_REQ>();
                    string queryBase = "SELECT * FROM HIS_SERVICE_REQ REQ WHERE SERVICE_REQ_CODE IN ('{0}') ";
                    
                    if (patientTypeId.HasValue)
                    {
                        queryBase += string.Format(" AND (SELECT PATIENT_TYPE_ID FROM HIS_PATIENT_TYPE_ALTER WHERE TREATMENT_ID = REQ.TREATMENT_ID ORDER BY LOG_TIME DESC FETCH FIRST ROW ONLY) = {0} ", patientTypeId.Value);
                    }

                    int skip = 0;
                    while (serviceReqCodes.Count - skip > 0)
                    {
                        var lstCode = serviceReqCodes.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        string query = string.Format(queryBase, string.Join("','", lstCode));
                        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                        var reqs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_REQ>(query);
                        if (reqs != null && reqs.Count > 0)
                        {
                            result.AddRange(reqs);
                        }
                    }
                }
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
