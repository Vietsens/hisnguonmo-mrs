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
using MRS.Processor.Mrs00691;

namespace MRS.Processor.Mrs00691
{
    public partial class ManagerSql : BusinessBase
    {
        public DataTable GetBillAndOtherCancel(Mrs00691Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "SELECT* ";

                query += "from (select tran.account_book_id,min(tran.num_order) as min_order,max(tran.num_order) as  max_order,sum(tran.amount) as bill_amount,null as amount_bk,null as num_order,null as transaction_code from his_rs.his_transaction tran where 1=1 and (tran.sale_type_id is null or tran.sale_type_id<>1) ";

                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.CREATE_TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID=2 ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID =1 ";
                    }
                }
                query += "and tran.transaction_type_id=3 group by tran.account_book_id union all (select tran.account_book_id, null as min_order, null as max_order,null  as bill_amount,amount as amount_bk,tran.num_order as num_order,tran.transaction_code as transaction_code from his_rs.his_transaction tran where tran.is_cancel is not null  and tran.cancel_time is not null and (tran.sale_type_id is null or tran.sale_type_id<>1) ";

                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME >= {0} ", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME < {0} ", filter.CREATE_TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CANCEL_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CANCEL_CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                query += "and ( ";

                query += string.Format("TRAN.TRANSACTION_TIME <= {0} ", filter.CREATE_TIME_FROM);

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("OR TRAN.CASHIER_LOGINNAME <>'{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("OR TRAN.CASHIER_ROOM_ID <>'{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                query += ") ";

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID=2 ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID =1 ";
                    }
                }
                query += "and tran.transaction_type_id=3)) trans  join his_rs.his_account_book acc on acc.id = trans.account_book_id order by TEMPLATE_CODE ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetSampleCancel(Mrs00691Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "SELECT ";

                query += "tran.num_order,acc.*,  tran.TRANSACTION_CODE,tran.amount from his_rs.his_transaction tran join his_rs.his_account_book acc on acc.id = tran.account_book_id where tran.is_cancel is not null and (tran.sale_type_id is null or tran.sale_type_id<>1) ";

                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME >= {0} ", filter.CREATE_TIME_FROM);
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME < {0} ", filter.CREATE_TIME_TO);
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.CREATE_TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CANCEL_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CANCEL_CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID=2 ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID =1 ";
                    }
                }

                query += "and tran.transaction_type_id=3 order by tran.num_order  ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetCancelOtherDetail(Mrs00691Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "SELECT ";

                query += "Trans.*,acc.*,dp.department_name from(SELECT tran.tdl_patient_code,tran.tdl_patient_name,dpt.treatment_id,tran.account_book_id, tran.num_order, tran.TRANSACTION_CODE, tran.TRANSACTION_TIME,tran.amount,MAX(dpt.department_id) KEEP (DENSE_RANK LAST ORDER BY dpt.id) as curent_department_id from his_rs.his_transaction tran left join his_rs.his_department_tran dpt on (dpt.department_in_time<tran.transaction_time  and dpt.treatment_id = tran.treatment_id) where tran.is_cancel is not null and (tran.sale_type_id is null or tran.sale_type_id<>1)  ";

                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME >= {0} ", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME < {0} ", filter.CREATE_TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CANCEL_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CANCEL_CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                query += "and ( ";

                query += string.Format("TRAN.TRANSACTION_TIME <= {0} ", filter.CREATE_TIME_FROM);

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("OR TRAN.CASHIER_LOGINNAME <>'{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("OR TRAN.CASHIER_ROOM_ID <>'{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                query += ") ";

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID=2 ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID =1 ";
                    }
                }
                query += "and tran.transaction_type_id=3 group by tran.tdl_patient_code,tran.tdl_patient_name,dpt.treatment_id,tran.account_book_id, tran.num_order, tran.TRANSACTION_CODE, tran.TRANSACTION_TIME,tran.amount) trans left join his_rs.his_department dp on dp.id = trans.curent_department_id left join his_rs.his_account_book acc on acc.id = trans.account_book_id  ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetCancelSampleDetail(Mrs00691Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "SELECT ";

                query += "Trans.*,acc.*,dp.department_name from(SELECT tran.tdl_patient_code,tran.tdl_patient_name,dpt.treatment_id,tran.account_book_id, tran.num_order, tran.TRANSACTION_CODE, tran.TRANSACTION_TIME,tran.amount,MAX(dpt.department_id) KEEP (DENSE_RANK LAST ORDER BY dpt.id) as curent_department_id from his_rs.his_transaction tran  left join his_rs.his_department_tran dpt on (dpt.department_in_time<tran.transaction_time  and dpt.treatment_id = tran.treatment_id) where tran.is_cancel is not null and (tran.sale_type_id is null or tran.sale_type_id<>1)  ";

                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME >= {0} ", filter.CREATE_TIME_FROM);
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME < {0} ", filter.CREATE_TIME_TO);
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.CREATE_TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CANCEL_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CANCEL_CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID=2 ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID =1 ";
                    }
                }
                query += "and tran.transaction_type_id=3 group by tran.tdl_patient_code,tran.tdl_patient_name,dpt.treatment_id,tran.account_book_id, tran.num_order, tran.TRANSACTION_CODE, tran.TRANSACTION_TIME,tran.amount) trans left join his_rs.his_department dp on dp.id = trans.curent_department_id  left join his_rs.his_account_book acc on acc.id = trans.account_book_id ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetBillDetail(Mrs00691Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "SELECT ";

                query += "Trans.*,acc.*,dp.department_name from(SELECT tran.tdl_patient_code,tran.tdl_patient_name,dpt.treatment_id,tran.account_book_id, tran.num_order, tran.TRANSACTION_CODE, tran.TRANSACTION_TIME,tran.amount,MAX(dpt.department_id) KEEP (DENSE_RANK LAST ORDER BY dpt.id) as curent_department_id from his_rs.his_transaction tran  left join his_rs.his_department_tran dpt on (dpt.department_in_time<tran.transaction_time  and dpt.treatment_id = tran.treatment_id) where 1=1 and (tran.sale_type_id is null or tran.sale_type_id<>1) ";//Lấy cả giao dịch hủy để phù hợp với báo cáo tổng hợp

                if (filter.CREATE_TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.CREATE_TIME_FROM);
                }
                if (filter.CREATE_TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.CREATE_TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID=2 ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID =1 ";
                    }
                }

                query += "and tran.transaction_type_id=3 group by tran.tdl_patient_code,tran.tdl_patient_name,dpt.treatment_id,tran.account_book_id, tran.num_order, tran.TRANSACTION_CODE, tran.TRANSACTION_TIME,tran.amount) trans left join his_rs.his_department dp on dp.id = trans.curent_department_id  left join his_rs.his_account_book acc on acc.id = trans.account_book_id  order by trans.num_order  ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<TYPE_PRICE> GetTypePrice(Mrs00691Filter filter)
        {
            List<TYPE_PRICE> result = null;
            try
            {
                string query = "";
                query += "SELECT ";

                query += "TRAN.ID AS BILL_ID, ";
                query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID =1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PATIENT_PRICE_BHYT, ";
                query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID <>1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PATIENT_PRICE_VP ";

                query += "FROM HIS_TRANSACTION TRAN ";
                query += "JOIN HIS_SERE_SERV_BILL SSB ON SSB.BILL_ID=TRAN.ID ";
                query += "JOIN HIS_SERE_SERV SS ON SS.ID=SSB.SERE_SERV_ID ";
                query += "WHERE 1=1 and (tran.sale_type_id is null or tran.sale_type_id<>1) ";

                query += string.Format("AND (TRAN.TRANSACTION_TIME BETWEEN {0} AND {1} OR TRAN.CANCEL_TIME BETWEEN {0} AND {1}) ", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO);
                query += "GROUP BY TRAN.ID ";

                result = new MOS.DAO.Sql.SqlDAO().GetSql<TYPE_PRICE>(query);
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
