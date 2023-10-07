using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.DAO.Sql;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00582
{
    class ManagerSql
    {

        public List<SereServDO> GetSereServ(Mrs00582Filter filter)
        {
            List<SereServDO> result = new List<SereServDO>();
            string query = "";
            query += "SELECT \n";
            query += "SRC.REPORT_TYPE_CAT_ID, \n";

            query += "SS.SERVICE_ID, \n";
            query += "SS.TDL_SERVICE_TYPE_ID, \n";
            query += "SS.TDL_TREATMENT_ID, \n";
            query += "SS.AMOUNT, \n";
            query += "SS.SERVICE_REQ_ID, \n";
            query += "SS.TDL_SERVICE_NAME, \n";
            query += "PR.SERVICE_CODE PARENT_SERVICE_CODE, \n";
            query += "PR.SERVICE_NAME PARENT_SERVICE_NAME, \n";

            query += "SR.REQUEST_DEPARTMENT_ID, \n";
            query += "SR.EXECUTE_DEPARTMENT_ID, \n";

            query += "SS.PRICE, \n";
            query += "TREA.TDL_TREATMENT_TYPE_ID, \n";
            query += "TREA.TDL_PATIENT_TYPE_ID, \n";
            query += "TREA.IN_TIME, \n";
            query += "TREA.TDL_PATIENT_DOB, \n";
            query += "TREA.TDL_PATIENT_GENDER_ID, \n";
            query += "TREA.TDL_PATIENT_ETHNIC_NAME \n";

            query += "FROM HIS_RS.HIS_SERE_SERV SS\n ";
            query += "JOIN HIS_SERVICE SV ON SV.ID = SS.SERVICE_ID\n";
            query += "LEFT JOIN HIS_SERVICE PR ON SV.PARENT_ID = PR.ID\n";
            query += "JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID = SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE = 'MRS00582') \n";
            query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
            query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID \n";


            if (filter.INTRUCTION_TIME_FROM != null)
            {
                query += string.Format("AND SR.INTRUCTION_TIME between {0} and {1} \n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO);
            }
            if (filter.FINISH_TIME_FROM != null)
            {
                query += string.Format("AND SR.FINISH_TIME between {0} and {1} \n", filter.FINISH_TIME_FROM, filter.FINISH_TIME_TO);
            }
            if (filter.INTRUCTION_DATE_FROM != null)
            {
                query += string.Format("AND SR.INTRUCTION_DATE between {0} and {1} \n", filter.INTRUCTION_DATE_FROM, filter.INTRUCTION_DATE_TO);
            }
            if (filter.START_TIME_FROM != null)
            {
                query += string.Format("AND SR.START_TIME between {0} and {1} \n", filter.START_TIME_FROM, filter.START_TIME_TO);
            }

            query += "WHERE SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL \n";
            if (filter.HAS_RESULT == true)
            {
                query += "AND EXISTS (SELECT 1 FROM HIS_SERE_SERV_TEIN WHERE SERE_SERV_ID = SS.ID AND VALUE IS NOT NULL) \n";
            }
            if (filter.REPORT_TYPE_CAT_ID != null)
            {
                query += string.Format("AND SRC.REPORT_TYPE_CAT_ID= {0}\n", filter.REPORT_TYPE_CAT_ID);
            }
            if (filter.REPORT_TYPE_CAT_IDs != null)
            {
                query += string.Format("AND SRC.REPORT_TYPE_CAT_ID in ({0})\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
            }
            if (filter.EXECUTE_DEPARTMENT_ID != null)
            {
                query += string.Format("AND sr.EXECUTE_DEPARTMENT_ID= {0}\n", filter.EXECUTE_DEPARTMENT_ID);
            }
            if (filter.BRANCH_ID != null)
            {
                query += string.Format("AND TREA.BRANCH_ID= {0}\n", filter.BRANCH_ID);
            }
            if (filter.EXECUTE_ROOM_ID != null)
            {
                query += string.Format("AND sr.EXECUTE_ROOM_ID= {0}\n", filter.EXECUTE_ROOM_ID);
            }
            if (filter.EXECUTE_ROOM_IDs != null)
            {
                query += string.Format("AND sr.EXECUTE_ROOM_ID in ({0})\n", string.Join(",", filter.EXECUTE_ROOM_IDs));
            }
            if (filter.NOT_IN_SERVICE_REQ_TYPE_IDs != null)
            {
                query += string.Format("AND sr.SERVICE_REQ_TYPE_ID not in ({0})\n", string.Join(",", filter.NOT_IN_SERVICE_REQ_TYPE_IDs));
            }
            if (filter.REQUEST_DEPARTMENT_ID != null)
            {
                query += string.Format("AND sr.REQUEST_DEPARTMENT_ID= {0}\n", filter.REQUEST_DEPARTMENT_ID);
            }
            if (filter.REQUEST_ROOM_ID != null)
            {
                query += string.Format("AND sr.REQUEST_ROOM_ID= {0}\n", filter.REQUEST_ROOM_ID);
            }
            if (filter.REQUEST_ROOM_IDs != null)
            {
                query += string.Format("AND sr.REQUEST_ROOM_ID in ({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
            }
            if (filter.SERVICE_REQ_STT_IDs != null)
            {
                query += string.Format("AND sr.SERVICE_REQ_STT_ID in ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
            }

            if (filter.SERVICE_REQ_STT_ID != null)
            {
                query += string.Format("AND sr.SERVICE_REQ_STT_ID= {0}\n", filter.SERVICE_REQ_STT_ID);
            }
            if (filter.SERVICE_REQ_TYPE_IDs != null)
            {
                query += string.Format("AND sr.SERVICE_REQ_TYPE_ID in ({0})\n", string.Join(",", filter.SERVICE_REQ_TYPE_IDs));
            }

            if (filter.SERVICE_REQ_TYPE_ID != null)
            {
                query += string.Format("AND sr.SERVICE_REQ_TYPE_ID= {0}\n", filter.SERVICE_REQ_TYPE_ID);
            }

            if (!string.IsNullOrWhiteSpace(filter.TREATMENT_CODE__EXACT))
            {
                query += string.Format("AND trea.treatment_code = '{0}'\n", string.Format("{0:000000000000}",filter.TREATMENT_CODE__EXACT));
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<SereServDO>(query);
            Inventec.Common.Logging.LogSystem.Info("Finish Query ");

            return result;
        }
    }
}
