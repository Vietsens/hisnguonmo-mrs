using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00696
{
    class ManagerSql
    {
        internal List<D_HIS_SERE_SERV> GetSS(Mrs00696Filter filter)
        {
            List<D_HIS_SERE_SERV> result = new List<D_HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.*, ";
                query += "T.TDL_PATIENT_TYPE_ID AS TREATMENT_TDL_PATIENT_TYPE_ID, T.IN_TIME AS TREATMENT_IN_TIME ";
                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB  ON SS.ID = SSB.SERE_SERV_ID ";
                query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID = SSB.BILL_ID ";
                query += "JOIN HIS_RS.HIS_TREATMENT T ON T.ID = SS.TDL_TREATMENT_ID ";
                query += "WHERE 1=1 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL AND SS.IS_DELETE = 0 ";
                query += "AND SSB.IS_CANCEL IS NULL AND SSB.IS_DELETE = 0 ";
                query += "AND TRAN.IS_CANCEL IS NULL AND TRAN.IS_DELETE = 0 ";

                query += string.Format("AND (SS.PATIENT_TYPE_ID = {0} OR SS.PRIMARY_PATIENT_TYPE_ID = {0})", HisPatientTypeCFG.PATIENT_TYPE_ID__DV);

                if (filter.TIME_TO.HasValue)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME <= {0} ", filter.TIME_TO.Value);
                }

                if (filter.TIME_FROM.HasValue)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TIME_FROM.Value);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<D_HIS_SERE_SERV>(query);

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

    public class D_HIS_SERE_SERV : HIS_SERE_SERV
    {
        public long? TREATMENT_TDL_PATIENT_TYPE_ID { get; set; }
        public long? TREATMENT_IN_TIME { get; set; }
    }
}
