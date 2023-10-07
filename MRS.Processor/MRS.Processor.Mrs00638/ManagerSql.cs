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
using MRS.Proccessor.Mrs00638;

namespace MRS.Processor.Mrs00638
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00638RDO> GetSereServDO(Mrs00638Filter filter)
        {
            List<Mrs00638RDO> result = new List<Mrs00638RDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.TDL_TREATMENT_ID, ";
                query += "SS.TDL_SERVICE_CODE, ";
                query += "SS.TDL_TREATMENT_CODE, ";
                query += "TREA.TDL_PATIENT_CODE, ";
                query += "TREA.TDL_PATIENT_NAME, ";
                query += "SS.IS_EXPEND, ";
                query += "(CASE WHEN (TREA.IS_PAUSE =0 OR TREA.IS_PAUSE IS NULL) THEN 1 ELSE (CASE WHEN TREA.IS_ACTIVE = 0 THEN 0 ELSE NULL END) END) AS IS_TREATING_OR_FEE_LOCK, ";
                query += "(CASE WHEN SS.TDL_SERVICE_TYPE_ID IN (1,4,11) THEN SS.TDL_EXECUTE_DEPARTMENT_ID ELSE SS.TDL_REQUEST_DEPARTMENT_ID END) AS DEPARTMENT_ID, ";
                query += "(CASE WHEN SS.TDL_SERVICE_TYPE_ID IN (1,4,11) THEN SS.TDL_EXECUTE_ROOM_ID ELSE SS.TDL_REQUEST_DEPARTMENT_ID END) AS ROOM_ID, ";
                query += "SS.AMOUNT, ";
                query += "NVL(SS.VIR_TOTAL_PRICE,0) AS VIR_TOTAL_PRICE, ";
                query += "SUM(SS1.AMOUNT*SS1.PRICE) AS PTTT_FOLLOW_TOTAL_PRICE ";

                query += "FROM HIS_SERE_SERV SS ";
                query += "LEFT JOIN HIS_SERE_SERV SS1 ON SS1.PARENT_ID = SS.ID, ";
                query += "HIS_TREATMENT TREA ";
                query += "WHERE SS.IS_DELETE = 0 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND TREA.ID = SS.TDL_TREATMENT_ID ";

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0} ", filter.PATIENT_TYPE_ID);
                }


                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_ROOM_ID IN ({0}) ", string.Join(",", filter.EXAM_ROOM_IDs));
                }

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) ", string.Join(",",filter.TREATMENT_TYPE_IDs));
                }

                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} ", filter.TREATMENT_TYPE_ID);
                }

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} ", filter.TIME_TO);
                }

                query += "GROUP BY ";
                query += "SS.ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.TDL_TREATMENT_ID, ";
                query += "SS.TDL_SERVICE_CODE, ";
                query += "SS.TDL_TREATMENT_CODE, ";
                query += "TREA.TDL_PATIENT_CODE, ";
                query += "TREA.TDL_PATIENT_NAME, ";
                query += "SS.IS_EXPEND, ";
                query += "(CASE WHEN (TREA.IS_PAUSE =0 OR TREA.IS_PAUSE IS NULL) THEN 1 ELSE (CASE WHEN TREA.IS_ACTIVE = 0 THEN 0 ELSE NULL END) END), ";
                query += "(CASE WHEN SS.TDL_SERVICE_TYPE_ID IN (1,4,11) THEN SS.TDL_EXECUTE_DEPARTMENT_ID ELSE SS.TDL_REQUEST_DEPARTMENT_ID END), ";
                query += "(CASE WHEN SS.TDL_SERVICE_TYPE_ID IN (1,4,11) THEN SS.TDL_EXECUTE_ROOM_ID ELSE SS.TDL_REQUEST_DEPARTMENT_ID END), ";
                query += "SS.AMOUNT, ";
                query += "NVL(SS.VIR_TOTAL_PRICE,0) ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00638RDO>(query);
               
                if (rs != null)
                {
                    result.AddRange(rs);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
    }
}
