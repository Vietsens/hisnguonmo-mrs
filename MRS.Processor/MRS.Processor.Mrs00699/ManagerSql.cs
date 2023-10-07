using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00699
{
    partial class ManagerSql : BusinessBase
    {
        public List<V_HIS_TREATMENT> GetTreatmentView(Mrs00699Filter filter)
        {
            List<V_HIS_TREATMENT> result = new List<V_HIS_TREATMENT>();


            string query = "";

            query += "SELECT \n";

            query += "TREA.* \n";
            //
            query += "FROM HIS_RS.V_HIS_TREATMENT TREA \n";
            query += "LEFT  JOIN HIS_RS.HIS_SERVICE_REQ SR ON TREA.ID = SR.TREATMENT_ID and SR.IS_DELETE =0 AND SR.IS_NO_EXECUTE IS NULL AND SR.SERVICE_REQ_TYPE_ID=1\n";
            query += "JOIN HIS_RS.HIS_PATIENT PT ON TREA.PATIENT_ID = PT.ID\n";
            //
            query += "WHERE 1=1\n";

            #region Loại thời gian
            if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
            {
                query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
            {
                query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
            {
                query += string.Format("AND TREA.CLINICAL_IN_TIME BETWEEN {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2} AND TREA.TREATMENT_END_TYPE_ID = 2\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
            {
                query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }
            else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            else
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }
            #endregion

            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
            }

            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }

            if (filter.END_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND TREA.END_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

            {
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TREATMENT>(query);
            }


            Inventec.Common.Logging.LogSystem.Info("Finish Query ");

            return result;
        }


        public List<V_HIS_BABY> GetListBaby(Mrs00699Filter filter) {
            List<V_HIS_BABY> result = new List<V_HIS_BABY>();
            try
            {
                string query = "";
                query += "SELECT * FROM V_HIS_BABY \n";
                query += "WHERE \n";
                query += string.Format("BORN_TIME BETWEEN {0} AND {1}\n", filter.TIME_FROM, filter.TIME_TO);
                query += "AND IS_DELETE = 0 \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_BABY>(query);
            }
            catch (Exception ex)
            {
                result = null;
                LogSystem.Error(ex);
            }
            return result;
        }
    }
}
