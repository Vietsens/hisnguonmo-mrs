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

namespace MRS.Processor.Mrs00445
{
    public class ManagerSql
    {

        public List<V_HIS_TREATMENT> GetTreatment(Mrs00445Filter filter)
        {
            try
            {
                List<V_HIS_TREATMENT> result = new List<V_HIS_TREATMENT>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "trea.*\n";

                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  and tran.is_cancel is null \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";

                query += "WHERE 1=1 ";

                query += "AND SS.IS_NO_EXECUTE IS NULL  and sr.is_delete =0  \n";

                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2} and ss.is_delete =0\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2} and ss.is_delete =0\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);

                }


                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<V_HIS_TREATMENT>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }
        public List<V_HIS_SERE_SERV> GetSereServ(Mrs00445Filter filter)
        {
            try
            {
                List<V_HIS_SERE_SERV> result = new List<V_HIS_SERE_SERV>();
                string query = "-- from Qcs\n";
                query += "SELECT \n";
                query += "ss.*\n";

                query += "FROM HIS_RS.V_HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  and tran.is_cancel is null \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID=TREA.ID  \n";

                query += "WHERE 1=1 ";

                query += "AND SS.IS_NO_EXECUTE IS NULL  and sr.is_delete =0  \n";

                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2} and ss.is_delete =0\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2} and ss.is_delete =0\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1}  and ss.is_delete =0 and ss.is_expend is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);

                }


                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<V_HIS_SERE_SERV>(query);

                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }

    }
}
