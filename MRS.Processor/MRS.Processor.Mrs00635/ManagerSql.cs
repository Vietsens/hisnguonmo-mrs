using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00635
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00635RDO> GetSereServDO(Mrs00635Filter filter)
        {
            List<Mrs00635RDO> result = new List<Mrs00635RDO>();
            try
            {
                string query = "";
                query += "SELECT   ";
                query += " SS.TDL_REQUEST_USERNAME, ";
                query += " SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, ";
                query += "SRC.SERVICE_TYPE_CODE, ";
                query += "SRC.SERVICE_TYPE_NAME, ";
                query += "SS.VIR_PRICE, ";
                query += "TREA.TREATMENT_CODE, ";
                query += "TREA.TDL_PATIENT_NAME, ";
                query += "SS.TDL_INTRUCTION_DATE, ";
                query += "SUM(SS.AMOUNT) AS AMOUNT, ";
                query += "SUM(SS.VIR_TOTAL_PRICE) AS VIR_TOTAL_PRICE, ";
                //query += "SS.AMOUNT, ";
                query += "PG.PTTT_GROUP_NAME ";
                //query += "SS.VIR_TOTAL_PRICE ";

                query += "FROM HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ";
                query += "JOIN HIS_RS.HIS_SERE_SERV SS ON (SRC.SERVICE_ID = SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE = 'MRS00635' AND SS.IS_DELETE = 0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} ", filter.TIME_TO);
                }
                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_ROOM_ID = {0} ", filter.EXECUTE_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_ROOM_ID = {0} ", filter.EXECUTE_ROOM_ID);
                }

                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID = {0} ", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0} ", filter.EXECUTE_DEPARTMENT_ID);
                }

                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_ROOM_ID IN ({0}) ", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }

                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_ROOM_ID IN ({0}) ", string.Join(",", filter.REQUEST_ROOM_IDs));
                }

                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID IN ({0}) ", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID IN ({0}) ", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                query += ") ";

                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID  ";
                query += "JOIN HIS_SERVICE S ON S.ID = SRC.SERVICE_ID  ";
                query += "LEFT JOIN HIS_PTTT_GROUP PG ON PG.ID = S.PTTT_GROUP_ID ";


                query += "GROUP BY ";
                query += "SS.TDL_REQUEST_USERNAME, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME, ";
                query += "SRC.SERVICE_TYPE_CODE, ";
                query += "SRC.SERVICE_TYPE_NAME, ";
                query += "TREA.TREATMENT_CODE, ";
                query += "TREA.TDL_PATIENT_NAME, ";
                query += "SS.TDL_INTRUCTION_DATE, ";
                query += "SS.VIR_PRICE, ";
                query += "PG.PTTT_GROUP_NAME";





              






                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00635RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
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
