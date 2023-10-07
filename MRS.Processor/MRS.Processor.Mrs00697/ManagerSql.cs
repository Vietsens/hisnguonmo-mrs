using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00697
{
    class ManagerSql
    {
        static List<long> serviceTypeIds = new List<long>() {
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT,
            IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU
        };

        internal static List<HIS_SERE_SERV> GetSSByTreatmentId(List<long> treatmentIds, long? patientTypeId)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.* ";
                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "WHERE 1=1 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL AND SS.IS_DELETE = 0 ";
                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID NOT IN ({0}) ", string.Join(",", serviceTypeIds));
                query += string.Format("AND SS.TDL_TREATMENT_ID IN ({0}) ", string.Join(",", treatmentIds));
                if (patientTypeId.HasValue)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", patientTypeId.Value);
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

        internal static List<HIS_SERE_SERV> GetSSByHeinApprovalId(List<long> heinApprovalId, long? patientTypeId)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.* ";
                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "WHERE 1=1 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL AND SS.IS_DELETE = 0 ";
                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID NOT IN ({0}) ", string.Join(",", serviceTypeIds));
                query += string.Format("AND SS.HEIN_APPROVAL_ID IN ({0}) ", string.Join(",", heinApprovalId));
                if (patientTypeId.HasValue)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", patientTypeId.Value);
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

        internal static List<HIS_SERE_SERV> GetSSByTransactionId(List<long> transactionIds, long? patientTypeId)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.* ";
                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB  ON SS.ID = SSB.SERE_SERV_ID ";
                query += "WHERE 1=1 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL AND SS.IS_DELETE = 0 ";
                query += "AND SS.TDL_TREATMENT_ID IS NOT NULL ";
                query += "AND SSB.IS_CANCEL IS NULL AND SSB.IS_DELETE = 0 ";
                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID NOT IN ({0}) ", string.Join(",", serviceTypeIds));
                query += string.Format("AND SSB.BILL_ID IN ({0})", string.Join(",", transactionIds));
                if (patientTypeId.HasValue)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", patientTypeId.Value);
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
