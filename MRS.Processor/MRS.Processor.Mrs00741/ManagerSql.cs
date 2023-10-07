using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00741
{
    class ManagerSql
    {
        internal List<Mrs00741RDO> GetData(Mrs00741Filter filter)
        {
            List<Mrs00741RDO> result = new List<Mrs00741RDO>();
            try
            {
                string query = "select \n";
                query += "nvl(pc.patient_classify_code,' ') patient_classify_code, \n";
                query += "nvl(pc.patient_classify_name,' ') patient_classify_name, \n";
                query += "pt.patient_type_code, \n";
                query += "pt.patient_type_name, \n";
                query += "ss.actual_price as REAL_PRICE, \n";
                query += "ss.price, \n";
                query += "ss.amount \n";
                query += "from his_service_req sr \n";
                query += "join his_sere_serv ss on sr.id = ss.service_req_id \n";
                query += "left join his_patient_classify pc on sr.tdl_patient_classify_id = pc.id \n";
                query += "left join his_patient_type pt on ss.patient_type_id = pt.id \n";
                query += "left join his_department dp on sr.request_department_id = dp.id \n";
                //query += "left join his_service_paty sp on (ss.service_id = sp.service_id and ss.patient_type_id = sp.patient_type_id and sr.tdl_patient_classify_id = sp.patient_classify_id) \n";
                query += string.Format("where sr.intruction_time BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                
                if (filter.CURRENT_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0}) \n", string.Join(",", filter.CURRENT_ROOM_IDs));
                }
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.request_department_id in ({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                
                query += string.Format("and sr.service_req_stt_id = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("and sr.service_req_type_id = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__AN);
                query += "and ss.is_active = 1 \n";
                query += "and (ss.is_delete = 0 or ss.is_delete is null) \n";
                query += "and pt.patient_type_name like '%Mức%' \n";

                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00741RDO>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }
    }
}
