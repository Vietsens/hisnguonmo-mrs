using IMSys.DbConfig.HIS_RS;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00738
{
    class ManagerSql
    {
        public List<SDA_EVENT_LOG> GetEventLog(Mrs00738Filter filter)
        {

            List<SDA_EVENT_LOG> result = null;
            try
            {
                string query = "select \n";
                
                query += "el.* \n";
                query += "from sda_rs.sda_event_log el \n";
                query += "where 1=1 \n";
                query += string.Format("and create_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("and (description like 'Tiếp nhận chuyển khoa%' or description like 'Chuyển vào buồng%')\n");
                
                //query += string.Format("and description like '%TREATMENT_CODE: {0}%' \n", treatmentCode);
                //query += string.Format("and description like '%Buồng bệnh: %' \n");
                result = new MOS.DAO.Sql.MyAppContext().GetSql<SDA_EVENT_LOG>(query);
                LogSystem.Info("SQL: " + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                return result;
            }
            return result;
        }

        public List<TREATMENT_BED_LOG> GetTreatmentAndBed(Mrs00738Filter filter)
        {
            List<TREATMENT_BED_LOG> result = null;
            
            try
            {
                string query = "select \n";
                
                query += "(select max(department_in_time) department_in_time from his_department_tran where treatment_id = bl.treatment_id and department_id = bl.department_id and department_in_time <= bl.start_time) department_in_time, \n";
                query += "trea.in_time, \n";
                query += "trea.treatment_code, \n";
                query += "trea.tdl_patient_code, \n";
                query += "trea.tdl_patient_name, \n";
                query += "trea.tdl_patient_dob, \n";
                query += "trea.tdl_patient_gender_name, \n";
                query += "bl.creator, \n";
                query += "bl.bed_code, \n";
                query += "bl.bed_name, \n";
                query += "bl.bed_room_name, \n";
                query += "bl.finish_time, \n";
                query += "bl.start_time \n";
                query += "from v_his_bed_log bl \n";
                //query += "left join his_department_tran dt on (bl.treatment_id = dt.treatment_id and bl.department_id = dt.department_id) \n";
                query += "join his_treatment trea on bl.treatment_id = trea.id \n";
                query += "where 1=1 \n";
                query += string.Format("and trea.in_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and bl.department_id in ({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += "order by trea.treatment_code, bl.start_time, bl.finish_time \n";
                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT_BED_LOG>(query);
                LogSystem.Info("SQL: " + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                return result;
            }
            return result;
        }
    }
}
