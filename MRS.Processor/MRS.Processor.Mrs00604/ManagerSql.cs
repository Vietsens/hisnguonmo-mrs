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

namespace MRS.Processor.Mrs00604
{
    public  class ManagerSql 
    {

        public List<Mrs00604RDO> GetSereServ(Mrs00604Filter filter)
        {
            List<Mrs00604RDO> result = new List<Mrs00604RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "SELECT \n";

            query += "trea.*,  \n";
            query += "ss.tdl_service_type_id,  \n";
            query += "ss.vir_total_price,  \n";
            query += "ss.amount,  \n";
            query += "src.category_code,  \n";
            query += "sr.health_exam_rank_id,  \n";
            query += "her.health_exam_rank_code,  \n";
            query += "her.health_exam_rank_name,  \n";
            query += "ss.tdl_execute_room_id  \n";

            query += "FROM HIS_TREATMENT TREA \n";
            query += "JOIN HIS_SERVICE_REQ SR ON SR.TREATMENT_ID=TREA.ID \n";
            query += "JOIN HIS_SERE_SERV SS ON SR.ID=ss.SERVICE_REQ_ID \n";
            query += "LEFT JOIN V_HIS_SERVICE_RETY_CAT SRC ON SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00600' \n";
            query += "LEFT JOIN HIS_HEALTH_EXAM_RANK HER ON HER.ID=sr.health_exam_rank_id \n";

            query += "WHERE 1=1 \n";

            if (filter.IN_TIME_FROM > 0 && filter.IN_TIME_TO > 0)
            {
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} AND {1} \n", filter.IN_TIME_FROM, filter.IN_TIME_TO);
            }

            query += string.Format("AND SS.IS_DELETE=0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL \n");

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<Mrs00604RDO>(paramGet, query);
                if (paramGet.HasException)
                    throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00604");

            return result;
        }

      
       
    }
}
