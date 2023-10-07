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
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.DAO.Sql;
using System.Data;

namespace MRS.Processor.Mrs00062
{
    public  class ManagerSql
    {

        public List<EXAM_IN_TREAT> GetExamInTreat(Mrs00062Filter filter)
        {
            List<EXAM_IN_TREAT> result = new List<EXAM_IN_TREAT>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT \n";
            query += "TREA.ID TREATMENT_ID \n";

            query += "FROM HIS_TREATMENT TREA \n";
            query += "JOIN HIS_SERVICE_REQ SR ON TREA.ID=SR.TREATMENT_ID AND SR.SERVICE_REQ_TYPE_ID=1 AND SR.EXAM_END_TYPE=2 \n";
            query += "JOIN HIS_ROOM RO ON RO.ID=TREA.TDL_FIRST_EXAM_ROOM_ID \n";

            query += "WHERE 1=1 \n";

            if (filter.TIME_FROM.HasValue && filter.TIME_TO.HasValue)
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }

            if (filter.IN_TIME_FROM.HasValue && filter.IN_TIME_TO.HasValue)
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} AND {1} \n", filter.IN_TIME_FROM, filter.IN_TIME_TO);
            }

            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("AND TREA.LAST_DEPARTMENT_ID ={0} \n", filter.DEPARTMENT_ID);
            }

            if (filter.FIRST_DEPARTMENT_ID != null)
            {
                query += string.Format("AND RO.DEPARTMENT_ID ={0} \n", filter.FIRST_DEPARTMENT_ID);
            }


            if (filter.MY_DEPARTMENT_ID != null)
            {
                query += string.Format("AND TREA.LAST_DEPARTMENT_ID ={0} \n", filter.MY_DEPARTMENT_ID);
            }
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }

            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("AND TREA.TDL_FIRST_EXAM_ROOM_ID IN({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<EXAM_IN_TREAT>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00062");

            return result;
        }

        public List<CATEGORY> GetCategory(Mrs00062Filter filter)
        {
            List<CATEGORY> result = new List<CATEGORY>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT \n";
            query += "SRC.CATEGORY_CODE, \n";
            query += "SRC.CATEGORY_NAME, \n";
            query += "SUM(SS.AMOUNT) AMOUNT \n";

            query += "FROM HIS_SERE_SERV SS \n";
            query += "JOIN HIS_TREATMENT TREA ON TREA.ID=SS.TDL_TREATMENT_ID \n";
            query += "JOIN HIS_ROOM RO ON RO.ID=TREA.TDL_FIRST_EXAM_ROOM_ID \n";
            query += "JOIN V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00062') \n";

            query += "WHERE SS.IS_NO_EXECUTE IS NULL AND SS.IS_DELETE =0  AND SS.IS_EXPEND IS NULL \n";

            if (filter.TIME_FROM.HasValue && filter.TIME_TO.HasValue)
            {
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
            }

            if (filter.IN_TIME_FROM.HasValue && filter.IN_TIME_TO.HasValue)
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} AND {1} \n", filter.IN_TIME_FROM, filter.IN_TIME_TO);
            }

            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("AND Trea.Last_department_id ={0} \n", filter.DEPARTMENT_ID);
            }

            if (filter.FIRST_DEPARTMENT_ID != null)
            {
                query += string.Format("AND RO.DEPARTMENT_ID ={0} \n", filter.FIRST_DEPARTMENT_ID);
            }

            if (filter.MY_DEPARTMENT_ID != null)
            {
                query += string.Format("AND Trea.Last_department_id ={0} \n", filter.MY_DEPARTMENT_ID);
            }
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }

            if (filter.EXAM_ROOM_IDs != null)
            {
                query += string.Format("AND TREA.TDL_FIRST_EXAM_ROOM_ID IN({0}) \n", string.Join(",", filter.EXAM_ROOM_IDs));
            }
            query += "GROUP BY \n";
            query += "SRC.CATEGORY_CODE, \n";
            query += "SRC.CATEGORY_NAME \n";

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<CATEGORY>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00062");

            return result;
        }
    }
}
