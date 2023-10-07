using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00300
{
    public class ManagerSql
    {
        public List<PTTT_INFO> GetSum(Mrs00300Filter castFilter)
        {
            List<PTTT_INFO> result = null;
            try
            {
                string query = "";
                query += "SELECT ";
                query += "PT.RELATIVE_PHONE, ";
                query += "PT.RELATIVE_MOBILE, ";
                query += "TREA.ID, ";
                query += "SS.* ";

                query += "FROM HIS_TREATMENT TREA ";
                query += "JOIN HIS_PATIENT PT ON PT.ID=TREA.PATIENT_ID ";
                query += "LEFT JOIN ( ";

                query += "SELECT  ";
                query += "SS.TDL_TREATMENT_ID, ";
                query += "SSP.PTTT_GROUP_ID, ";
                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "EU.LOGINNAME, ";
                query += "EU.USERNAME, ";
                query += "SSP.PTTT_METHOD_ID, ";
                query += "SSP.EMOTIONLESS_METHOD_ID, ";
                query += "SSP.MANNER, ";
                query += "SS.TDL_SERVICE_NAME ";
                query += "FROM  ";
                query += "HIS_SERE_SERV SS ";
                query += "JOIN HIS_SERE_SERV_PTTT SSP ON SSP.SERE_SERV_ID=SS.ID ";
                query += "JOIN HIS_EKIP_USER EU ON EU.EKIP_ID=SS.EKIP_ID ";
                query += "JOIN HIS_EXECUTE_ROLE ER ON ER.ID=EU.EXECUTE_ROLE_ID ";
                query += "WHERE SS.IS_DELETE =0 ";
                query += "AND SS.IS_NO_EXECUTE IS NULL  ";
                query += "AND ER.IS_SURG_MAIN =1 ";
                query += ") SS ON SS.TDL_TREATMENT_ID=TREA.ID ";
                query += "WHERE 1=1 ";
                if (castFilter.TRUE_FALSE.HasValue && !castFilter.TRUE_FALSE.Value)
                {
                    query += string.Format("AND TREA.IS_PAUSE=1 AND TREA.OUT_TIME BETWEEN {0} AND {1} ", castFilter.TIME_FROM, castFilter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND TREA.STORE_TIME BETWEEN {0} AND {1} ", castFilter.TIME_FROM, castFilter.TIME_TO);
                }
                if (castFilter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND TREA.END_DEPARTMENT_ID ={0} ", castFilter.DEPARTMENT_ID);
                }
                if (castFilter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND TREA.END_DEPARTMENT_ID in ('{0}') ", string.Join("','",castFilter.DEPARTMENT_IDs));
                }
                if (castFilter.BRANCH_ID != null)
                {
                    query += string.Format("AND TREA.BRANCH_ID ={0} ", castFilter.BRANCH_ID);
                }
               
                result = new MOS.DAO.Sql.SqlDAO().GetSql<PTTT_INFO>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL:" + query);
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
