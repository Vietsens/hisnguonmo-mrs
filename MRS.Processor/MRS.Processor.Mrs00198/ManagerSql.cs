using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00198
{
    class ManagerSql
    {

        internal  List<V_HIS_TREATMENT> GetTreatment(Mrs00198Filter filter)
        {
            List<V_HIS_TREATMENT> result = new List<V_HIS_TREATMENT>();
            try
            {
                string query = "";
                query += "select \n";
                query += "trea.* \n";
              
                query += "from v_his_treatment trea \n";
                query += "left join his_treatment trea1 on trea1.appointment_code=trea.treatment_code \n";
                query += "where 1=1 \n";
                query += "and trea.is_delete =0 \n";
                query += string.Format("and trea.treatment_end_type_id ={0} \n",IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN);

                query += string.Format("and trea.appointment_time between {0} and {1} \n", filter.DATE_FROM, filter.DATE_TO);

                if (filter.IS_CAME == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    query += string.Format("and trea1.id is not null \n");
                }
                if (filter.IS_CAME == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE)
                {
                    query += string.Format("and trea1.id is null \n");
                }
                if (filter.EXAM_ROOM_IDs !=null)
                {
                    string inRoom = "";
                    foreach (var item in filter.EXAM_ROOM_IDs)
                    {
                        inRoom += string.Format("or ','||trea.APPOINTMENT_EXAM_ROOM_IDS||',' like '%,{0},%' \n", item);
                    }
                    query += string.Format("and trea.APPOINTMENT_EXAM_ROOM_IDS is not null and (1=0 {0}) \n", inRoom);
                }
                if (filter.TREATMENT_TYPE_IDs !=null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id in({0}) \n", String.Join(",",filter.TREATMENT_TYPE_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TREATMENT>(query);
                if (rs != null && rs.Count > 0)
                {
                    result.AddRange(rs);
                }
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
