using Inventec.Common.Logging;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00729
{
    internal class ManagerSql
    {
        internal List<Mrs00729RDO.Sheet1> GetDataNgoaiTru(Mrs00729Filter filter)
        {
            List<Mrs00729RDO.Sheet1> result = new List<Mrs00729RDO.Sheet1>();
            try
            {
                string query = "--sheet 1\n";
                query += "select \n";
                query += "ro.room_code,\n";
                query += "ro.room_name,\n";
                query += "trea.tdl_patient_dob,\n";
                query += "trea.patient_id,\n";
                query += "trea.tdl_patient_type_id,\n";
                query += "ss.tdl_service_type_id, \n";
                query += "sr.exam_end_type, \n";
                query += "sr.previous_service_req_id, \n";
                query += "sr.exam_treatment_end_type_id, \n";
                query += "trea.tdl_treatment_type_id \n";
                
                query += "from his_sere_serv ss\n";
                query += "join his_service_req sr on sr.id = ss.service_req_id\n";
                query += "join v_his_room ro on ss.tdl_execute_room_id = ro.id\n";
                query += "join his_treatment trea on ss.tdl_treatment_id = trea.id\n";
                query += "where 1=1\n";
                //query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                query += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("and ro.is_exam = 1\n");
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                }
                
                
                //query += string.Format("and sr.service_req_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00729RDO.Sheet1>(query);
                LogSystem.Info("SQL: " + query);
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
        }

        internal List<Mrs00729RDO.Sheet2> GetDataNoiTru(Mrs00729Filter filter)
        {
            List<Mrs00729RDO.Sheet2> result = new List<Mrs00729RDO.Sheet2>();
            try
            {
                string query = "--sheet 2\n";
                query += "select \n";
                query += "dp.id department_id, \n";
                query += "dp.department_code,\n";
                query += "dp.department_name,\n";
                query += "ro.room_code,\n";
                query += "ro.room_name,\n";
                query += "trea.tdl_patient_dob,\n";
                query += "trea.patient_id,\n";
                query += "trea.in_time,\n";
                query += "trea.out_time,\n";
                query += "trea.clinical_in_time,\n";
                query += "trea.tdl_patient_type_id,\n";
                query += "ss.tdl_service_type_id, \n";
                query += "sr.exam_end_type, \n";
                query += "sr.previous_service_req_id, \n";
                query += "sr.exam_treatment_end_type_id, \n";
                query += "trea.tdl_treatment_type_id, \n";
                query += "trea.treatment_day_count\n";
                query += "from his_sere_serv ss\n";
                query += "join his_service_req sr on sr.id = ss.service_req_id\n";
                query += "join his_department dp on ss.tdl_execute_department_id = dp.id\n";
                query += "left join v_his_room ro on ss.tdl_execute_room_id = ro.id\n";
                query += "join his_treatment trea on ss.tdl_treatment_id = trea.id\n";
                query += "where 1=1\n";
                query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                query += string.Format("and (trea.create_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("or trea.in_time <= {0} and trea.out_time is null) \n", filter.TIME_FROM);
                
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                
                //query += string.Format("and sr.service_req_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00729RDO.Sheet2>(query);
                LogSystem.Info("SQL: " + query);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
        }

        //internal List<Mrs00729RDO.Sheet2> GetDataOldNoiTru(Mrs00729Filter filter)
        //{
        //    List<Mrs00729RDO.Sheet2> result = new List<Mrs00729RDO.Sheet2>();
        //    try
        //    {
        //        string query = "--sheet 2\n";
        //        query += "select \n";
        //        query += "dp.id department_id, \n";
        //        query += "dp.department_code,\n";
        //        query += "dp.department_name,\n";
        //        query += "ro.room_code,\n";
        //        query += "ro.room_name,\n";
        //        query += "count(distinct(sr.tdl_patient_id)) count_old\n";
        //        query += "from his_sere_serv ss\n";
        //        query += "join his_service_req sr on sr.id = ss.service_req_id\n";
        //        query += "join his_department dp on ss.tdl_execute_department_id = dp.id\n";
        //        query += "left join v_his_room ro on ss.tdl_execute_room_id = ro.id\n";
        //        query += "join his_treatment trea on ss.tdl_treatment_id = trea.id\n";
        //        query += "where 1=1\n";
        //        query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
        //        query += string.Format("and trea.in_time <= {0} \n", filter.TIME_FROM);
        //        query += string.Format("and trea.out_time is null\n");

        //        if (filter.DEPARTMENT_IDs != null)
        //        {
        //            query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
        //        }
        //        query += "group by dp.id, dp.department_code, dp.department_name, ro.room_code, ro.room_name\n";
        //        //query += string.Format("and sr.service_req_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK);

        //        result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00729RDO.Sheet2>(query);
        //        LogSystem.Info("SQL: " + query);

        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        return result;
        //    }
        //    return result;
        //}

        internal List<Mrs00729RDO.Sheet3> GetDataDepartment(Mrs00729Filter filter)
        {
            List<Mrs00729RDO.Sheet3> result = new List<Mrs00729RDO.Sheet3>();
            try
            {
                string query = "--sheet 3\n";
                query += "select \n";
                query += "sr.execute_department_id,\n";
                query += "sr.request_department_id,\n";
                query += "ss.tdl_service_type_id,\n";
                query += "ssp.pttt_group_id\n";
                query += "from his_sere_serv ss\n";
                query += "join his_service_req sr on sr.id = ss.service_req_id\n";
                query += "join his_sere_serv_pttt ssp on ss.id = ssp.sere_serv_id\n";
                query += "where 1=1\n";
                
                query += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);


                //query += string.Format("and sr.service_req_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00729RDO.Sheet3>(query);
                LogSystem.Info("SQL: " + query);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
        }
    }
}
