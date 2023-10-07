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
using System.Data;
using MRS.Processor.Mrs00329;

namespace MRS.Processor.Mrs00329
{
    public class ManagerSql
    {
        public List<V_HIS_TREATMENT> GetTreatment(Mrs00329Filter filter)
        {
            List<V_HIS_TREATMENT> result = null;
            try
            {
                string query = "";

                query += " --danh sach benh nhan bao hiem duyet ho so\n";
                query += "SELECT \n";
                query += "treal.loginname creator,\n";
                query += "trea.* \n";
                query += "from v_his_treatment trea\n";
                query += "left join  \n";
                query += "( \n";
                query += "select treatment_id, \n";
                query += "max(loginname) keep(dense_rank last order by create_time) loginname \n";
                query += "from his_treatment_logging \n";
                query += "where 1=1 \n";
                query += "and treatment_log_type_id=3 \n";
                query += "group by  \n";
                query += "treatment_id \n";
                query += ") treal on treal.treatment_id=trea.id \n";
                if (filter.IS_PAY == true)
                {
                    query += "join his_transaction tran on tran.treatment_id = trea.id \n";

                    query += "and tran.is_cancel is null and tran.transaction_type_id = 3 \n";

                    query += string.Format("and tran.transaction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);

                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                    if (filter.END_DEPARTMENT_ID != null)
                    {
                        query += string.Format("and trea.end_department_id = {0}\n", filter.END_DEPARTMENT_ID);
                    }
                    if (filter.IS_TREAT != null)
                    {
                        if (filter.IS_TREAT == true)
                        {
                            query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                        }
                        else
                        {
                            query += string.Format("and trea.tdl_treatment_type_id <> {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                        }

                    }
                    if (filter.STATUS_TREATMENT == null)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FEE_LOCK_LOGINNAME))
                        {
                            query += string.Format("and treal.loginname = {0}\n", filter.FEE_LOCK_LOGINNAME);
                        }
                        query += string.Format("and trea.is_active = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                    }
                    if (filter.STATUS_TREATMENT == 0)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FEE_LOCK_LOGINNAME))
                        {
                            query += string.Format("and trea.creator = {0}\n", filter.FEE_LOCK_LOGINNAME);
                        }
                        query += string.Format("and trea.is_pause is null \n");
                    }
                    if (filter.STATUS_TREATMENT == 1)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FEE_LOCK_LOGINNAME))
                        {
                            query += string.Format("and trea.end_loginname = {0}\n", filter.FEE_LOCK_LOGINNAME);
                        }
                        query += string.Format("and trea.is_pause = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                    }
                    if (filter.STATUS_TREATMENT == 2)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FEE_LOCK_LOGINNAME))
                        {
                            query += string.Format("and treal.loginname = {0}\n", filter.FEE_LOCK_LOGINNAME);
                        }
                        query += string.Format("and trea.is_lock_hein = {0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                    }
                }
                else
                {
                    query += "where 1=1\n";
                    query += string.Format("and trea.branch_id = {0}\n", filter.BRANCH_ID);
                    if (filter.END_DEPARTMENT_ID != null)
                    {
                        query += string.Format("and trea.end_department_id = {0}\n", filter.END_DEPARTMENT_ID);
                    }
                    if (filter.IS_TREAT != null)
                    {
                        if (filter.IS_TREAT == true)
                        {
                            query += string.Format("and trea.tdl_treatment_type_id = {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                        }
                        else
                        {
                            query += string.Format("and trea.tdl_treatment_type_id <> {0}\n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                        }

                    }
                    if (filter.STATUS_TREATMENT == null)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FEE_LOCK_LOGINNAME))
                        {
                            query += string.Format("and treal.loginname = {0}\n", filter.FEE_LOCK_LOGINNAME);
                        }
                        query += string.Format("and trea.is_active = 0\n");
                        query += string.Format("and trea.fee_lock_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                    }
                    if (filter.STATUS_TREATMENT == 0)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FEE_LOCK_LOGINNAME))
                        {
                            query += string.Format("and trea.creator = {0}\n", filter.FEE_LOCK_LOGINNAME);
                        }
                        query += string.Format("and trea.is_pause is null\n");
                        query += string.Format("and trea.in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                    }
                    if (filter.STATUS_TREATMENT == 1)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FEE_LOCK_LOGINNAME))
                        {
                            query += string.Format("and trea.end_loginname = {0}\n", filter.FEE_LOCK_LOGINNAME);
                        }
                        query += string.Format("and trea.is_pause = {0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                        query += string.Format("and trea.out_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                    }
                    if (filter.STATUS_TREATMENT == 2)
                    {
                        if (!string.IsNullOrWhiteSpace(filter.FEE_LOCK_LOGINNAME))
                        {
                            query += string.Format("and treal.loginname = {0}\n", filter.FEE_LOCK_LOGINNAME);
                        }
                        query += string.Format("and trea.is_lock_hein ={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                        query += string.Format("and trea.fee_lock_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                    }
                }
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TREATMENT>(query);
                if (result != null)
                {
                    result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                }
                Inventec.Common.Logging.LogSystem.Info(query);
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
