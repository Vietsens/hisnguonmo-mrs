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

namespace MRS.Processor.Mrs00050
{
    internal class ManagerSql
    {

        internal List<Mrs00050RDO> GetTransaction(Mrs00050Filter filter)
        {
            List<Mrs00050RDO> result = new List<Mrs00050RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += " --danh sach giao dich huy va giao dich lai\n";
            query += "select \n";
            query += "nvl(tran.cancel_reason,crs.cancel_reason_name) cancel_reason, \n";
            query += "tran.*,\n";
            query += "re_tran.re_transaction_time,\n";
            query += "re_tran.re_einvoice_num_order,\n";
            query += "re_tran.re_cashier_loginname,\n";
            query += "re_tran.re_cashier_username,\n";
            query += "re_tran.re_amount\n";
            query += "from v_his_transaction tran\n";
            query += "left join lateral\n";
            query += "(\n";
            query += "select\n";
            query += "max(re_tran.transaction_time) keep (dense_rank last order by re_tran.transaction_time,re_tran.id) re_transaction_time,\n";
            query += "max(re_tran.einvoice_num_order) keep (dense_rank last order by re_tran.transaction_time,re_tran.id) re_einvoice_num_order,\n";
            query += "max(re_tran.cashier_loginname) keep (dense_rank last order by re_tran.transaction_time,re_tran.id) re_cashier_loginname,\n";
            query += "max(re_tran.cashier_username) keep (dense_rank last order by re_tran.transaction_time,re_tran.id) re_cashier_username,\n";
            query += "max(re_tran.amount) keep (dense_rank last order by re_tran.transaction_time,re_tran.id) re_amount\n";
            query += "from his_transaction re_tran\n";
            query += "where 1=1 and re_tran.treatment_id=tran.treatment_id and re_tran.transaction_type_id=tran.transaction_type_id\n";
            query += "and re_tran.is_cancel is null\n";
            query += "and re_tran.transaction_time > {0}\n";
            if (filter.TRANSACTION_TYPE_IDs != null)
            {
                query += string.Format("and re_tran.transaction_type_id in ({0}) \n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
            }

            if (filter.TRANSACTION_TYPE_ID != null)
            {
                query += string.Format("and re_tran.transaction_type_id = {0} \n", filter.TRANSACTION_TYPE_ID);
            }
            query += ") re_tran on 1=1\n";
            query += "left join his_cancel_reason crs on crs.id=tran.cancel_reason_id\n";
            query += "where 1=1 \n";
            query += "and tran.is_cancel =1\n";
            query += "and tran.cancel_time between {0} and {1}\n";
            if (filter.TRANSACTION_TYPE_IDs != null)
            {
                query += string.Format("and tran.transaction_type_id in ({0}) \n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
            }

            if (filter.TRANSACTION_TYPE_ID != null)
            {
                query += string.Format("and tran.transaction_type_id = {0} \n", filter.TRANSACTION_TYPE_ID);
            }

            query = string.Format(query, filter.TIME_FROM, filter.TIME_TO);
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00050RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00050");

            return result;
        }

      
    }
}
