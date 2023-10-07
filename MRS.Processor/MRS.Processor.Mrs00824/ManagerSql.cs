using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Sql;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00824
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00824RDO> GetListRdo(Mrs00824Filter filter) {

            List<Mrs00824RDO> result = new List<Mrs00824RDO>();
            CommonParam paramGet = new CommonParam();
            try
            {
                string query = "";
                query += "select  \n";
                query += "ss.* , \n";
                query += "trea.branch_id, \n";
                query += "trea.tdl_patient_code, \n";
                query += "trea.tdl_patient_name, \n";
                query += "trea.treatment_code, \n";
                query += "trea.tdl_treatment_type_id, \n";
                query += "trea.tdl_hein_card_number, \n";
                query += "case when trea.end_department_name is null then depa.department_name else trea.end_department_name end as department_name, \n";
                query += "case when trea.tdl_treatment_type_id =3 then trea.end_room_name end as room_name, \n";
                query += "case when trea.tdl_treatment_type_id <>3 then trea.end_room_name end as exam_room_name \n";
                query += "from \n";
                query += "his_sere_serv ss \n";
                query += "join v_his_treatment trea on ss.tdl_treatment_id = trea.id \n";
                query += "left join his_hein_approval hap on hap.id = ss.hein_approval_id \n";
                query += "join his_service sv on sv.id = ss.service_id \n";
                query += "left join his_department depa on depa.id = trea.last_department_id \n";
                query += "left join his_patient_type paty on paty.id = ss.patient_type_id \n";
                query += "where  \n";
                query += "1 =1  \n";
                query += "and ss.IS_DELETE = 0 \n";
                query += "and lower(paty.patient_type_name) like '%bhyt%' \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE.HasValue && filter.INPUT_DATA_ID_TIME_TYPE.Value == 1)
                {
                    query += string.Format("and trea.in_time between {0} and {1} \n",filter.TIME_FROM,filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE.HasValue && filter.INPUT_DATA_ID_TIME_TYPE.Value == 2)
                {
                    query += string.Format("and trea.out_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE.HasValue && filter.INPUT_DATA_ID_TIME_TYPE.Value == 3)
                {
                    query += string.Format("and ss.tdl_intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE.HasValue && filter.INPUT_DATA_ID_TIME_TYPE.Value == 4)
                {
                    query += string.Format("and hap.execute_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("and hap.execute_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
               
                    if (filter.IS_TREATMENT_TYPE.HasValue && filter.IS_TREATMENT_TYPE.Value == 0)
                    {
                        query += string.Format("and trea.tdl_treatment_type_id = {0} \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                    else if (filter.IS_TREATMENT_TYPE.HasValue && filter.IS_TREATMENT_TYPE.Value == 1)
                    {
                        query += string.Format("and trea.tdl_treatment_type_id in ({0},{1}) \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY);
                    }
                    else if (filter.IS_TREATMENT_TYPE.HasValue && filter.IS_TREATMENT_TYPE.Value == 2)
                    {
                        query += string.Format("and trea.tdl_treatment_type_id = {0} \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                    }
                
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<Mrs00824RDO>(paramGet, query);
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
