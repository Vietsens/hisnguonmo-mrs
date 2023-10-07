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

namespace MRS.Processor.Mrs00707
{
    internal class ManagerSql
    {
        //internal List<Mrs00707RDO> GetTransaction(Mrs00707Filter filter)
        //{
        //    List<Mrs00707RDO> result = new List<Mrs00707RDO>();
        //    CommonParam paramGet = new CommonParam();
        //    string query = "";
        //    query += "select \n";
        //    query += "tran.transaction_code,\n";
        //    query += "tran.transaction_time,\n";
        //    query += "tran.transaction_date,\n";
        //    query += "tran.is_cancel,\n";
        //    query += "tran.cashier_loginname,\n";
        //    query += "tran.cashier_username,\n";
        //    query += "cr.cashier_room_code,\n";
        //    query += "cr.cashier_room_name,\n";
        //    query += "ms.medi_stock_code,\n";
        //    query += "ms.medi_stock_name,\n";
        //    query += "nvl(ex.tdl_total_price,0) tdl_total_price,\n";
        //    query += "ex.exp_mest_code,\n";
        //    query += "ex.pres_number,\n";
        //    query += "nvl(tran.amount,0) amount\n";
        //    query += "from his_transaction tran\n";
        //    query += "join his_cashier_room cr on cr.id=tran.cashier_room_id\n";
        //    query += "left join his_exp_mest ex on ex.bill_id=tran.id\n";
        //    query += "left join his_medi_stock ms on ms.id=ex.medi_stock_id\n";
        //    query += "where \n";
        //    query += "tran.sale_type_id =1\n";
        //    if (filter.EXACT_CASHIER_ROOM_IDs != null)
        //    {
        //        query += string.Format("and tran.cashier_room_id in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
        //    }
        //    if (filter.CASHIER_LOGINNAMEs != null)
        //    {
        //        query += string.Format("and tran.cashier_loginname in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
        //    }
        //    if (filter.MEDI_STOCK_BUSINESS_IDs != null)
        //    {
        //        query += string.Format("and (ex.medi_stock_id in ({0}))\n", string.Join(",", filter.MEDI_STOCK_BUSINESS_IDs));
        //    }
        //    if (filter.IS_CANCEL != null)
        //    {
        //        if (filter.IS_CANCEL == false)
        //        {
        //            query += "and tran.is_cancel is null \n";
        //        }
        //        else if (filter.IS_CANCEL == true)
        //        {
        //            query += "and tran.is_cancel = 1 \n";
        //        }

        //    }
        //    query += string.Format("and tran.transaction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
        //    Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
        //    result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00707RDO>(paramGet, query);
        //    if (paramGet.HasException)
        //        throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00707");

        //    return result;
        //}
        internal List<Mrs00707RDO> GetTransaction(Mrs00707Filter filter)
        {
            List<Mrs00707RDO> result = new List<Mrs00707RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select \n";
            query += "tran.transaction_code,\n";
            query += "tran.transaction_time,\n";
            query += "tran.transaction_date,\n";
            query += "tran.is_cancel,\n";
            query += "tran.cashier_loginname,\n";
            query += "tran.cashier_username,\n";
            query += "cr.cashier_room_code,\n";
            query += "cr.cashier_room_name,\n";
            query += "ms.medi_stock_code,\n";
            query += "ms.medi_stock_name,\n";
            query += "nvl(ex.tdl_total_price,0) tdl_total_price,\n";
            query += "nvl(ex.exp_mest_code,trane.tdl_exp_mest_code) exp_mest_code,\n";
            query += "nvl(ex.pres_number,exd.pres_number) pres_number,\n";
            //query += "ex.pres_number,\n";
            query += "nvl(tran.amount,0) amount\n";
            query += "from his_transaction tran\n";
            query += "join his_cashier_room cr on cr.id=tran.cashier_room_id\n";
            query += "left join his_exp_mest ex on ex.bill_id=tran.id\n";
            query += "left join his_transaction_exp trane on tran.id=trane.transaction_id and tran.is_cancel =1\n";
            query += "left join his_exp_mest_deleted exd on trane.tdl_exp_mest_code=exd.exp_mest_code\n";
            query += "left join his_medi_stock ms on ms.id=nvl(ex.medi_stock_id,exd.medi_stock_id)\n";
            query += "where \n";
            query += "tran.sale_type_id =1\n";
            if (filter.EXACT_CASHIER_ROOM_IDs != null)
            {
                query += string.Format("and tran.cashier_room_id in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
            }
            if (filter.CASHIER_LOGINNAMEs != null)
            {
                query += string.Format("and tran.cashier_loginname in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
            }
            if (filter.MEDI_STOCK_BUSINESS_IDs != null)
            {
                query += string.Format("and (ms.id in ({0}))\n", string.Join(",", filter.MEDI_STOCK_BUSINESS_IDs));
            }
            if (filter.IS_CANCEL != null)
            {
                if (filter.IS_CANCEL == false)
                {
                    query += "and tran.is_cancel is null \n";
                }
                else if (filter.IS_CANCEL == true)
                {
                    query += "and tran.is_cancel = 1 \n";
                }

            }
            query += string.Format("and tran.transaction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00707RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00707");

            return result;
        }

        internal List<PrintLogUnique> GetPrintLog(Mrs00707Filter filter)
        {
            List<PrintLogUnique> result = new List<PrintLogUnique>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select\n";
            query += "unique_code\n";
            query += "from sar_print_log\n";
            query += "where print_type_code='Mps000339'\n";
            query += "and unique_code is not null\n";
            query += string.Format("and create_time >= {0}\n", filter.TIME_FROM);
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SAR.DAO.Sql.SqlDAO().GetSql<PrintLogUnique>(query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00707");

            return result;
        }
    }
}
