using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00719
{
    internal class ManagerSql
    {

        internal List<Mrs00719RDO> GetMachine(Mrs00719Filter filter)
        {
            List<Mrs00719RDO> result = new List<Mrs00719RDO>();
            try
            {
                
                string query = "SELECT \n";
                query += "DP.DEPARTMENT_CODE MA_KP, \n";
                query += "DP.DEPARTMENT_NAME TEN_KP, \n";
                query += "MA.MACHINE_CODE MA_MAY, \n";
                query += "MA.MACHINE_NAME TEN_MAY, \n";
                //query += "MSM.EXPEND_AMOUNT, \n";
                query += "SSB.TDL_OTHER_SOURCE_PRICE THANH_TIEN, \n";
                query += "SSB.TDL_PATIENT_TYPE_ID DOI_TUONG_TT \n";
                query += "FROM HIS_DEPARTMENT DP \n";
                query += "JOIN HIS_SERVICE_REQ SR ON DP.ID = SR.REQUEST_DEPARTMENT_ID \n";
                query += "JOIN HIS_SERE_SERV SS ON SR.ID = SS.SERVICE_REQ_ID \n";
                query += "JOIN HIS_SERVICE_MACHINE SM ON SS.SERVICE_ID = SM.SERVICE_ID \n";
                query += "JOIN HIS_MACHINE MA ON SM.MACHINE_ID = MA.ID \n";
                query += "JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID \n";
                query += "WHERE 1=1 \n";
                query += "AND SS.IS_NO_EXECUTE IS NULL \n";
                query += "AND SS.IS_EXPEND = 1 \n";
                query += "AND SR.IS_DELETE = 0 \n";
                query += "AND SS.IS_DELETE = 0 \n";
                query += "AND SM.IS_DELETE = 0 \n";
                query += "AND SM.IS_ACTIVE = 1 \n";
                query += "AND SSB.TDL_OTHER_SOURCE_PRICE > 0 \n";
                query += string.Format(" \n");
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }

                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00719RDO>(paramGet, query);


                Inventec.Common.Logging.LogSystem.Info("Result: " + result);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }
    }
}
