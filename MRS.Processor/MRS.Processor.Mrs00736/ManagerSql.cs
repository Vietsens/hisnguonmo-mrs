using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00736
{
    class ManagerSql
    {
        public List<Mrs00736RDO> GetData(Mrs00736Filter filter)
        {
            List<Mrs00736RDO> result = new List<Mrs00736RDO>();
            try
            {
                string query = "--SQL QUERY:\n";
                query += "select \n";
                query += "trea.id, \n";
                query += "trea.treatment_code, \n";
                query += "trea.tdl_patient_name, \n";
                query += "trea.tdl_patient_dob, \n";
                query += "trea.tdl_patient_address, \n";
                query += "ss.other_pay_source_id, \n";
                query += "sum(ss.amount) amount, \n";
                query += "sum(ss.amount*nvl(ss.other_source_price,0)) TOTAL_OTHER_SOURCE_PRICE, \n";
                //query += "ss.other_source_price, \n";
                query += "trea.fee_lock_time transaction_time \n";
                //query += "trunc(trea.fee_lock_time,-6) transaction_date \n";
                query += "from his_sere_serv ss \n";
                //query += "left join his_sere_serv_bill ssb on ss.id = ssb.sere_serv_id and ssb.is_cancel is null\n";
                //query += "left join his_transaction tran on ssb.bill_id = tran.id  and tran.is_cancel is null\n";
                query += "join his_treatment trea on ss.tdl_treatment_id = trea.id \n";
                query += "left join (select treatment_id,max(cashier_room_id) keep(dense_rank last order by id) cashier_room_id from his_transaction where is_cancel is null and transaction_type_id=3 and sale_type_id is null group by treatment_id) tran on tran.treatment_id=trea.id \n";
                query += "where 1=1 and ss.is_delete=0 and ss.is_no_execute is null\n";
                query += string.Format("and trea.fee_lock_time between {0} and {1} and trea.is_active=0\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("and (tran.cashier_room_id is not null and (select branch_id from v_his_cashier_room where id=tran.cashier_room_id) in ({0}) or tran.cashier_room_id is null and trea.end_department_id in (select id from his_department where branch_id in ({0})) )\n", string.Join(",", filter.BRANCH_IDs));
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.OTHER_PAY_SOURCE_IDs != null)
                {
                    query += string.Format("and ss.other_pay_source_id in ({0}) \n", string.Join(",", filter.OTHER_PAY_SOURCE_IDs));
                }
                query += "and ss.other_source_price>0 \n";
                query += "group by\n";
                query += "trea.id, \n";
                query += "trea.treatment_code, \n";
                query += "trea.tdl_patient_name, \n";
                query += "trea.tdl_patient_dob, \n";
                query += "trea.tdl_patient_address, \n";
                query += "ss.other_pay_source_id, \n";
                query += "trea.fee_lock_time ";
                //query += "trunc(trea.fee_lock_time,-6) \n";
                LogSystem.Info("SQL query: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00736RDO>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
        }

        public List<HIS_OTHER_PAY_SOURCE> GetOtherSource()
        {

            List<HIS_OTHER_PAY_SOURCE> result = new List<HIS_OTHER_PAY_SOURCE>();
            try
            {
                string query = "select * from his_other_pay_source";
                LogSystem.Info("SQL query OTHER_PAY_SOURCE: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_OTHER_PAY_SOURCE>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return result;
            }
            return result;
        }

        public List<HIS_BRANCH> GetBranch(Mrs00736Filter filter)
        {

            List<HIS_BRANCH> result = new List<HIS_BRANCH>();
            try
            {
                string query = "select * from his_branch \n";
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("where id in ({0})", string.Join(",", filter.BRANCH_IDs));
                }
                LogSystem.Info("SQL query HIS_BRANCH: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_BRANCH>(query);
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
