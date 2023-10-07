using ACS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using LIS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00745
{
    class ManagerSql
    {
        internal List<LIS_MACHINE> GetMachine(Mrs00745Filter filter)
        {
            List<LIS_MACHINE> result = new List<LIS_MACHINE>();
            try
            {
                string query = " --from Qcs \n";
                query += "select *\n";
                query += "from lis_rs.lis_machine \n";
                //query += string.Format("where create_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                LogSystem.Info("GetMachine: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<LIS_MACHINE>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }

        internal List<Mrs00745RDO> GetQC(Mrs00745Filter filter)
        {
            List<Mrs00745RDO> result = new List<Mrs00745RDO>();
            try
            {
                string query = "";
                query += "select qt.qc_type_code, qt.qc_type_name, ma.machine_code, ma.machine_name, qn.modifier, qn.creator\n";
                query += "from his_qc_normation qn \n";
                query += "join his_qc_type qt on qn.qc_type_id = qt.id \n";
                query += "join his_machine ma on qn.machine_id = ma.id \n";
                query += string.Format("where (qn.modify_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("or qn.create_time between {0} and {1}) \n", filter.TIME_FROM, filter.TIME_TO);
                query += "group by qt.qc_type_code, qt.qc_type_name, ma.machine_code, ma.machine_name, qn.modifier, qn.creator \n";
                LogSystem.Info("GetQC: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00745RDO>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }

        public List<DEPARTMENT> GetDepartment(Mrs00745Filter filter)
        {
            List<DEPARTMENT> result = new List<DEPARTMENT>();
            try
            {
                string query = "";
                query += "select dp.*, ro.room_code\n";
                query += "from v_his_room ro\n";
                query += "join his_department dp on ro.department_id = dp.id \n";
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("where department_id in ({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                LogSystem.Info("GetDepartment: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<DEPARTMENT>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }

        public List<ACS_USER> GetUser()
        {
            List<ACS_USER> result = new List<ACS_USER>();
            try
            {
                string query = " --from Qcs \n";
                query += "select *\n";
                query += "from acs_rs.acs_user \n";
                
                LogSystem.Info("GetUser: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<ACS_USER>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }

        public List<HIS_EMPLOYEE> GetEmployee()
        {
            List<HIS_EMPLOYEE> result = new List<HIS_EMPLOYEE>();
            try
            {
                string query = "";
                query += "select *\n";
                query += "from his_employee \n";

                LogSystem.Info("GetEmployee: " + query);
                result = new MOS.DAO.Sql.MyAppContext().GetSql<HIS_EMPLOYEE>(query);
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
