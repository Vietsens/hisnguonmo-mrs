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
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00547
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00547RDO> GetAll(Mrs00547Filter filter)
        {
            List<Mrs00547RDO> result = new List<Mrs00547RDO>();
            try
            {
                string query = "";
                query += "select  ";
                query += "ss.*, \n";
                query += "sr.icd_code, \n";
                query += "sr.icd_name, \n";
                query += "sr.execute_loginname, \n";
                query += "sr.execute_username, \n";
                query += "sr.request_loginname, \n";
                query += "sr.request_username, \n";
                query += "sse.note, \n";
                query += "sse.conclude, \n";
                query += "sr.tdl_patient_id, \n";
                query += "trea.tdl_patient_code, \n";
                query += "trea.tdl_treatment_type_id, \n";
                query += "sr.tdl_treatment_code, \n";
                query += "trea.tdl_patient_name, \n";
                query += "trea.tdl_patient_gender_name, \n";
                query += "trea.tdl_patient_gender_id, \n";
                query += "trea.tdl_patient_dob, \n";
                query += "trea.tdl_patient_address, \n";
                query += "br.bed_room_code, \n";
                query += "br.bed_room_name, \n";
                query += "src.category_code, \n";
                query += "src.category_name, \n";
                query += "sr.intruction_time, \n";
                query += "sr.intruction_date, \n";
                query += "(select max(bed_name) keep(dense_rank last order by start_time) from v_his_bed_log where bed_room_id=br.id and start_time<sr.intruction_time) bed_name, \n";
                query += "(select max(bed_code) keep(dense_rank last order by start_time) from v_his_bed_log where bed_room_id=br.id and start_time<sr.intruction_time) bed_code \n";
                query += "from \n";
                query += "his_sere_serv ss  \n";
                query += "join his_service_req sr on sr.id=ss.service_req_id \n";
                query += "join his_treatment trea on trea.id=ss.tdl_treatment_id \n";
                query += "join v_his_service_rety_cat src on src.service_id=ss.service_id and src.report_type_code='MRS00547' \n";
                query += "left join his_bed_room br on br.room_id=sr.request_room_id \n";
                query += "left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id \n";
                query += "where 1=1 \n";
                query += "and ss.is_delete =0  \n";
                query += "and ss.is_no_execute is null \n";

                //query += string.Format("AND sr.intruction_time between {0} and {1} ", filter.INTRUCTION_TIME_FROM ?? filter.FINISH_TIME_FROM ?? filter.START_TIME_FROM ?? 0, filter.INTRUCTION_TIME_TO ?? filter.FINISH_TIME_TO ?? filter.START_TIME_TO ?? 0);

                if (filter.INTRUCTION_TIME_FROM != null && filter.INTRUCTION_TIME_TO != null)
                {
                    query += string.Format("AND sr.intruction_time between {0} and {1} \n", filter.INTRUCTION_TIME_FROM, filter.INTRUCTION_TIME_TO);
                }

                if (filter.FINISH_TIME_FROM != null && filter.FINISH_TIME_TO != null)
                {
                    query += string.Format("AND sr.finish_time between {0} and {1} \n", filter.FINISH_TIME_FROM, filter.FINISH_TIME_TO);
                }

                if (filter.START_TIME_FROM != null && filter.START_TIME_TO != null)
                {
                    query += string.Format("AND sr.start_time between {0} and {1} \n", filter.START_TIME_FROM, filter.START_TIME_TO);
                }


                if (filter.REPORT_TYPE_CAT_ID != null)
                {
                    query += string.Format("AND src.report_type_cat_id={0} \n", filter.REPORT_TYPE_CAT_ID);
                }

                if (filter.REPORT_TYPE_CAT_IDs != null)
                {
                    query += string.Format("AND src.report_type_cat_id in({0}) \n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                }


                if (filter.IS_TREAT != null)
                {
                    if (filter.IS_TREAT == true)
                    {
                        query += string.Format("AND trea.tdl_treatment_type_id=3 \n");
                    }
                    if (filter.IS_TREAT == false)
                    {
                        query += string.Format("AND trea.tdl_treatment_type_id<>3 \n");
                    }
                }

                if (filter.EXE_ROOM_ID != null)
                {
                    query += string.Format("AND sr.execute_room_id={0} \n", filter.EXE_ROOM_ID);
                }

                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("AND sr.execute_room_id={0} \n", filter.EXECUTE_ROOM_ID);
                }

                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND sr.REQUEST_DEPARTMENT_ID={0} \n", filter.REQUEST_DEPARTMENT_ID);
                }

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND ss.PATIENT_TYPE_ID={0} \n", filter.PATIENT_TYPE_ID);
                }

                if (filter.REQUEST_LOGINNAME != null)
                {
                    query += string.Format("AND sr.REQUEST_LOGINNAME='{0}' \n", filter.REQUEST_LOGINNAME);
                }

                if (filter.REQUEST_LOGINNAMEs != null)
                {
                    query += string.Format("AND sr.REQUEST_LOGINNAME in ('{0}') \n", string.Join("','", filter.REQUEST_LOGINNAMEs));
                }

                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND sr.REQUEST_DEPARTMENT_ID in ('{0}') \n", string.Join("','", filter.REQUEST_DEPARTMENT_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00547RDO>(query);
                if (rs != null)
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
