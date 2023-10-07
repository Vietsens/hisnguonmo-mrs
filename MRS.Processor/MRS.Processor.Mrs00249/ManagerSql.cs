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
using MRS.Processor.Mrs00249;

namespace MRS.Processor.Mrs00249
{
    public partial class ManagerSql : BusinessBase
    {
        private const short BILL_TYPE_ID__NORMAL = 1;
        private const short BILL_TYPE_ID__SERVICE = 2;

        public List<TREATMENT_FEE> GetTreatmentFee(Mrs00249Filter filter)
        {
            List<TREATMENT_FEE> result = null;
            try
            {

                string query = "";

                query += " --tong chi phi cua benh nhan\n";
                query += "SELECT \n";
                query += "TREA.ID, \n";
                query += "TREA.TOTAL_PATIENT_PRICE_BHYT, \n";
                query += "TREA.TOTAL_PATIENT_PRICE, \n";
                query += "TREA.TOTAL_OTHER_SOURCE_PRICE, \n";
                query += "TREA.TOTAL_DISCOUNT, \n";
                query += "TREA.TOTAL_DEPOSIT_AMOUNT, \n";
                query += "TREA.TOTAL_REPAY_AMOUNT, \n";
                query += "TREA.TOTAL_HEIN_PRICE, \n";
                query += "TREA.HOSPITALIZE_DEPARTMENT_ID, \n";
                query += "TREA.IN_TIME, \n";
                query += "TREA.OUT_TIME \n";
                query += "FROM HIS_TRANSACTION TRAN \n";
                query += "JOIN (\n";
                query += "SELECT\n";
                query += "TREA.ID,\n";
                query += "TREA.IN_TIME, \n";
                query += "TREA.OUT_TIME, \n";
                query += "(case when trea.tdl_treatment_type_id in (2,3,4) then nvl(trea.hospitalize_department_id,(select min(dpt.department_id) keep(dense_rank first order by dpt.id)  from his_department_tran dpt join his_patient_type_alter pta on pta.department_tran_id=dpt.id and pta.treatment_type_id=trea.tdl_treatment_type_id where trea.id=pta.treatment_id)) when trea.in_treatment_type_id in (2,3,4) then (select min(dpt.department_id) keep(dense_rank last order by dpt.id)  from his_department_tran dpt where trea.id=dpt.treatment_id) else null end) HOSPITALIZE_DEPARTMENT_ID, \n";
                query += @"(
SELECT NVL(SUM(TRAN.AMOUNT),0)
FROM HIS_TRANSACTION TRAN
WHERE (TRAN.TDL_SERE_SERV_DEPOSIT_COUNT IS NULL OR TRAN.TDL_SERE_SERV_DEPOSIT_COUNT =0) and TRAN.TREATMENT_ID = TREA.ID AND (TRAN.IS_CANCEL IS NULL OR TRAN.IS_CANCEL <> 1) AND TRAN.IS_ACTIVE = 1 AND TRAN.TRANSACTION_TYPE_ID = 1 --DEPOSIT
) AS TOTAL_DEPOSIT_AMOUNT,
(
SELECT NVL(SUM(TRAN.AMOUNT),0)
FROM HIS_TRANSACTION TRAN
WHERE TRAN.TREATMENT_ID = TREA.ID AND (TRAN.IS_CANCEL IS NULL OR TRAN.IS_CANCEL <> 1) AND TRAN.TRANSACTION_TYPE_ID = 2 --REPAY
) AS TOTAL_REPAY_AMOUNT,
(
SELECT NVL(SUM(SESE.VIR_TOTAL_HEIN_PRICE),0)
FROM HIS_SERE_SERV SESE
WHERE SESE.HEIN_CARD_NUMBER IS NOT NULL AND SESE.TDL_TREATMENT_ID = TREA.ID AND (SESE.SERVICE_REQ_ID IS NOT NULL AND (SESE.IS_DELETE IS NULL OR SESE.IS_DELETE <> 1))
) AS TOTAL_HEIN_PRICE,
(
SELECT NVL(SUM(SESE.VIR_TOTAL_PATIENT_PRICE),0)
FROM HIS_SERE_SERV SESE
WHERE SESE.TDL_TREATMENT_ID = TREA.ID AND (SESE.SERVICE_REQ_ID IS NOT NULL AND (SESE.IS_DELETE IS NULL OR SESE.IS_DELETE <> 1))
) AS TOTAL_PATIENT_PRICE,
(
SELECT NVL(SUM(SESE.DISCOUNT),0)
FROM HIS_SERE_SERV SESE
WHERE SESE.TDL_TREATMENT_ID = TREA.ID AND (SESE.SERVICE_REQ_ID IS NOT NULL AND (SESE.IS_DELETE IS NULL OR SESE.IS_DELETE <> 1) AND (SESE.IS_NO_EXECUTE IS NULL OR SESE.IS_NO_EXECUTE <> 1) AND (SESE.IS_EXPEND IS NULL OR SESE.IS_EXPEND <> 1))
) AS TOTAL_DISCOUNT,
(
SELECT NVL(SUM(SESE.VIR_TOTAL_PATIENT_PRICE_BHYT),0)
FROM HIS_SERE_SERV SESE
WHERE SESE.TDL_TREATMENT_ID = TREA.ID AND (SESE.SERVICE_REQ_ID IS NOT NULL AND (SESE.IS_DELETE IS NULL OR SESE.IS_DELETE <> 1) AND (SESE.IS_NO_EXECUTE IS NULL OR SESE.IS_NO_EXECUTE <> 1) AND (SESE.IS_EXPEND IS NULL OR SESE.IS_EXPEND <> 1))
) AS TOTAL_PATIENT_PRICE_BHYT,
(
SELECT SUM(NVL(SESE.OTHER_SOURCE_PRICE, 0) * SESE.AMOUNT)
FROM HIS_SERE_SERV SESE
WHERE SESE.TDL_TREATMENT_ID = TREA.ID AND (SESE.SERVICE_REQ_ID IS NOT NULL AND (SESE.IS_DELETE IS NULL OR SESE.IS_DELETE <> 1) AND (SESE.IS_NO_EXECUTE IS NULL OR SESE.IS_NO_EXECUTE <> 1) AND (SESE.IS_EXPEND IS NULL OR SESE.IS_EXPEND <> 1))
) AS TOTAL_OTHER_SOURCE_PRICE ";
                query += "FROM HIS_TREATMENT TREA\n";
                query += "  WHERE 1=1 \n";
                query += ClauseWhereTreatment(filter);
                query += ") TREA ON TRAN.TREATMENT_ID=TREA.ID\n";
                query += "  WHERE 1=1 \n";
                query += ClauseWhereTransaction(filter);


                result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT_FEE>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<TYPE_PRICE> GetTypePrice(Mrs00249Filter filter)
        {
            List<TYPE_PRICE> result = null;
            try
            {
                string query = "";
                if (filter.TAKE_ALL_DISCOUNT == true)
                {
                    query += " --tong tien bao hiem, benh nhan thanh toan, chiet khau duoc gan cho giao dich thanh toan cuoi cung\n";
                    query += "SELECT \n";

                    query += "TRAN.BILL_ID, ";
                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID =1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PATIENT_PRICE_BHYT, \n";
                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID =1 THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0)-nvl(SS.VIR_TOTAL_PATIENT_PRICE_BHYT,0) ELSE 0 END) AS TOTAL_DIFFERENCE_PRICE, \n";
                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID <>1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PATIENT_PRICE_VP, \n";
                    query += "SUM(NVL(SS.OTHER_SOURCE_PRICE,0)*SS.AMOUNT) AS TOTAL_OTHER_SOURCE_PRICE, \n";
                    query += "SUM(NVL(SS.DISCOUNT,0)) AS SS_DISCOUNT \n";

                    query += "FROM ( \n";

                    query += "SELECT \n";
                    query += "TREATMENT_ID, \n";
                    query += "MAX(ID) KEEP(DENSE_RANK LAST ORDER BY TRANSACTION_TIME) AS BILL_ID \n";
                    query += "FROM HIS_TRANSACTION \n";
                    query += "WHERE 1=1 \n";
                    query += "AND IS_CANCEL IS NULL \n";
                    query += "AND TRANSACTION_TYPE_ID = 3 \n";

                    query += string.Format("AND TRANSACTION_TIME BETWEEN {0} AND {1} \n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO);
                    query += "GROUP BY TREATMENT_ID \n";
                    query += ") TRAN \n";
                    query += "JOIN HIS_SERE_SERV SS ON (SS.IS_DELETE = 0 AND SS.IS_NO_EXECUTE IS NULL AND SS.TDL_TREATMENT_ID = TRAN.TREATMENT_ID) \n";
                    query += "GROUP BY TRAN.BILL_ID \n";
                }
                else
                {
                    query += " --tach tong tien bao hiem, benh nhan thanh toan, chiet khau theo tung dot giao dich\n";
                    query += "SELECT \n";

                    query += "TRAN.ID AS BILL_ID, \n";
                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID =1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PATIENT_PRICE_BHYT, \n";
                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID =1 THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0)-nvl(SS.VIR_TOTAL_PATIENT_PRICE_BHYT,0) ELSE 0 END) AS TOTAL_DIFFERENCE_PRICE, \n";
                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID <>1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PATIENT_PRICE_VP, \n";
                    query += "SUM(NVL(SS.OTHER_SOURCE_PRICE,0)*SS.AMOUNT) AS TOTAL_OTHER_SOURCE_PRICE, \n";
                    query += "SUM(NVL(SS.DISCOUNT,0)) AS SS_DISCOUNT \n";

                    query += "FROM HIS_TRANSACTION TRAN \n";
                    query += "JOIN HIS_SERE_SERV_BILL SSB ON  SSB.BILL_ID=TRAN.ID \n";
                    query += "JOIN HIS_SERE_SERV SS ON SS.ID=SSB.SERE_SERV_ID \n";
                    query += "WHERE 1=1 \n";
                    query += "AND TRAN.TRANSACTION_TYPE_ID = 3 \n";

                    query += ClauseWhereTransaction(filter);

                    query += "GROUP BY TRAN.ID \n";
                }

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

        public List<TYPE_PRICE> GetDepDiscount(Mrs00249Filter filter)
        {
            List<TYPE_PRICE> result = null;
            try
            {
                string query = "";
                if (filter.TAKE_ALL_DISCOUNT == true)
                {
                    query += " --giao dich thanh toan cuoi cung moi lay tong tien bao hiem, benh nhan thanh toan, chiet khau\n";
                    query += "\n";
                }
                else
                {
                    query += " --tach tong tien bao hiem, benh nhan thanh toan, chiet khau theo tung dot giao dich\n";
                    query += "SELECT \n";

                    query += "TRAN.ID AS BILL_ID, ";

                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID =1 THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0)-nvl(SS.VIR_TOTAL_PATIENT_PRICE_BHYT,0) ELSE 0 END) AS TOTAL_DIFFERENCE_PRICE, \n";

                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID =1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PATIENT_PRICE_BHYT, \n";
                    query += "SUM(CASE WHEN SS.PATIENT_TYPE_ID <>1 THEN SS.VIR_TOTAL_PATIENT_PRICE ELSE 0 END) AS TOTAL_PATIENT_PRICE_VP, \n";
                    query += "SUM(NVL(SS.DISCOUNT,0)) AS SS_DISCOUNT \n";

                    query += "FROM HIS_TRANSACTION TRAN \n";

                    query += "JOIN HIS_SERE_SERV_DEPOSIT SSB ON SSB.DEPOSIT_ID=TRAN.ID \n";
                    query += "JOIN HIS_SERE_SERV SS ON SS.ID=SSB.SERE_SERV_ID \n";
                    query += "WHERE 1=1 AND SS.DISCOUNT>0 \n";
                    query += "AND TRAN.TRANSACTION_TYPE_ID = 1 \n";

                    query += ClauseWhereTransaction(filter);

                    query += "GROUP BY TRAN.ID \n";

                    result = new MOS.DAO.Sql.SqlDAO().GetSql<TYPE_PRICE>(query);
                    Inventec.Common.Logging.LogSystem.Info(query);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }


        public System.Data.DataTable GetSum(Mrs00249Filter filter, string query)
        {
            System.Data.DataTable result = null;
            try
            {
                query = string.Format(query, (filter.CREATE_TIME_TO != null) ? filter.CREATE_TIME_TO.ToString() : "''"
, (filter.CREATE_TIME_FROM != null) ? filter.CREATE_TIME_FROM.ToString() : "''"
, (filter.CASHIER_LOGINNAME != null) ? "'" + filter.CASHIER_LOGINNAME + "'" : "''"

, (filter.EXACT_CASHIER_ROOM_ID != null) ? filter.EXACT_CASHIER_ROOM_ID.ToString() : "''"

, (filter.EXACT_CASHIER_ROOM_IDs != null) ? filter.EXACT_CASHIER_ROOM_IDs.ToString() : "''"

, (filter.ACCOUNT_BOOK_ID != null) ? filter.ACCOUNT_BOOK_ID.ToString() : "''"

, (filter.IS_BILL_NORMAL != null) ? filter.IS_BILL_NORMAL == true ? "1" : "2" : "''"

, (filter.BRANCH_ID != null) ? filter.BRANCH_ID.ToString() : "''"

, (filter.TREATMENT_TYPE_IDs != null) ? string.Join(",", filter.TREATMENT_TYPE_IDs) : "''"

, (filter.PAY_FORM_IDs != null) ? string.Join(",", filter.PAY_FORM_IDs) : "''"

, (filter.CASHIER_LOGINNAMEs != null) ? string.Join("','", filter.CASHIER_LOGINNAMEs) : "''"
, (filter.ACCOUNT_BOOK_IDs != null) ? string.Join(",", filter.ACCOUNT_BOOK_IDs) : "''"


, (filter.REQUEST_DEPARTMENT_IDs != null) ? string.Join(",", filter.REQUEST_DEPARTMENT_IDs) : "''"
, (filter.TRANSACTION_TYPE_IDs != null) ? string.Join(",", filter.TRANSACTION_TYPE_IDs) : "''"

);
                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            if (ContainsSelectClause(query) && NullOrEmptyRow(result))
            {
                result = new DataTable();
                DataRow row = result.NewRow();
                result.Rows.Add(row);
            }
            return result;
        }

        private bool NullOrEmptyRow(DataTable result)
        {
            return (result == null || result.Rows == null || result.Rows.Count == 0);
        }

        private bool ContainsSelectClause(string query)
        {
            return string.IsNullOrWhiteSpace(query) == false && query.ToUpper().Contains("SELECT");
        }

        internal List<Mrs00249RDO> GetTransaction(Mrs00249Filter filter)
        {
            List<Mrs00249RDO> result = null;
            try
            {
                string query = "";
                query += string.Format(" --danh sach giao dich\n");
                query += string.Format("select \n");
                query += string.Format("tran.PAY_FORM_ID AS PAY_FORM_ID,\n");
                query += string.Format("tran.PAY_FORM_NAME AS PAY_FORM_NAME,\n");
                query += string.Format("tran.PAY_FORM_CODE AS PAY_FORM_CODE,\n");

                


                query += string.Format("(case when tran.is_cancel is null then 1 else 0 end) is_on_time,\n");
                query += string.Format("tran.cashier_loginname,\n");
                query += string.Format("tran.cashier_username,\n");
                query += string.Format("tran.transaction_time,\n");
                query += string.Format("tran.transaction_date,\n");
                query += string.Format("tran.amount,\n");
                query += string.Format("trea.IN_TIME, \n");
                query += string.Format("trea.OUT_TIME, \n");
                query += string.Format("trea.TREATMENT_DAY_COUNT, \n");
                query += string.Format("(case when tran.is_cancel=1 then tran.amount else 0 end) cancel_amount,\n");
                query += string.Format("tran.kc_amount,\n");
                query += string.Format("tran.exemption,\n");
                query += string.Format("cr.CASHIER_ROOM_CODE,\n");
                query += string.Format("cr.CASHIER_ROOM_NAME,\n");
                query += string.Format("tran.*\n");
                query += string.Format("from v_his_transaction tran\n");
                query += string.Format("join v_his_cashier_room cr on cr.id=tran.cashier_room_id\n");
                query += string.Format("left join his_treatment trea on trea.id=tran.treatment_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and tran.is_delete=0\n");

                query += ClauseWhereTreatment(filter);

                query += string.Format("and tran.transaction_time between {0} and {1}\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO);
                if (filter.IS_ONLY_CANCEL == true)
                {
                    query += string.Format("and 0=1\n");
                }
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("and tran.cashier_loginname='{0}'\n", filter.CASHIER_LOGINNAME);
                }
                if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                {
                    query += string.Format("and tran.cashier_loginname in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("and tran.cashier_room_id={0}\n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("and tran.cashier_room_id in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }
                if (filter.ACCOUNT_BOOK_ID != null)
                {
                    query += string.Format("and tran.account_book_id={0}\n", filter.ACCOUNT_BOOK_ID);
                }
                if (filter.ACCOUNT_BOOK_IDs != null)
                {
                    query += string.Format("and tran.account_book_id in ({0})\n", string.Join(",", filter.ACCOUNT_BOOK_IDs));
                }
                if (filter.TRANSACTION_TYPE_IDs != null)
                {
                    query += string.Format("and tran.transaction_type_id in ({0})\n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
                }
                //if (filter.REQUEST_DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("and ss.tdl_request_department_id in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                //}
                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("and tran.pay_form_id in ({0})\n", string.Join(",", filter.PAY_FORM_IDs));

                   // query += string.Format("AND (CASE WHEN tran.IS_CANCEL = 1 and tran.PAY_FORM_ID = 8 THEN 1 ELSE tran.PAY_FORM_ID END) in({0}) ", string.Join(",", filter.PAY_FORM_IDs));


                   
                }


                // loc theo trang thai giao dich
                if (filter.INPUT_DATA_ID_STTRAN_TYPE == 1)
                {
                    query += string.Format("and tran.is_active={0}\n",IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                if (filter.INPUT_DATA_ID_STTRAN_TYPE == 2)
                {
                    query += string.Format("and tran.is_active={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and cr.branch_id={0}\n", filter.BRANCH_ID);
                }
                if(filter.INPUT_DATA_ID_SALE_TYPE != null)
                {
                    if (filter.INPUT_DATA_ID_SALE_TYPE == 1)
                    {
                        query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_EXP);
                    }
                    else if(filter.INPUT_DATA_ID_SALE_TYPE == 2)
                    {
                        query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER);
                    }
                    else if(filter.INPUT_DATA_ID_SALE_TYPE == 3)
                    {
                        query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_VACCIN);
                    }
                    else// if(filter.INPUT_DATA_ID_SALE_TYPE == 4)
                    {
                        query += string.Format("and tran.sale_type_id is null\n");
                    }
                }    
                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += string.Format("and tran.bill_type_id={0}\n", BILL_TYPE_ID__NORMAL);
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += string.Format("and tran.bill_type_id={0}\n", BILL_TYPE_ID__SERVICE);
                    }
                }
                if (filter.IS_REMOVE_DUPPLICATE == true)
                {
                    string checkLoginname = "";// " and tran.cashier_loginname = tran.cancel_loginname";
                    string checkCashierRooom = "";// "  and tran.cashier_room_id = tran.cancel_cashier_room_id";
                    List<string> cashierLoginnames = new List<string>();
                    if (filter.CASHIER_LOGINNAME != null)
                    {
                        cashierLoginnames.Add(filter.CASHIER_LOGINNAME ?? "");
                    }
                    if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                    {
                        cashierLoginnames.AddRange(filter.CASHIER_LOGINNAMEs);
                    }
                    if (cashierLoginnames.Count > 0)
                    {
                        checkLoginname = string.Format(" and tran.cashier_loginname in ('{0}') and tran.cancel_loginname in ('{0}')", string.Join("','", cashierLoginnames));
                    }
                    List<long> cashierRoomIds = new List<long>();
                    if (filter.EXACT_CASHIER_ROOM_ID != null)
                    {
                        cashierRoomIds.Add(filter.EXACT_CASHIER_ROOM_ID ?? 0);
                    }
                    if (IsNotNullOrEmpty(filter.EXACT_CASHIER_ROOM_IDs))
                    {
                        cashierRoomIds.AddRange(filter.EXACT_CASHIER_ROOM_IDs);
                    }
                    if (cashierRoomIds.Count > 0)
                    {
                        checkCashierRooom = string.Format(" and tran.cashier_room_id in ({0}) and tran.cancel_cashier_room_id in ({0})", string.Join(",", cashierRoomIds));
                    }
                    query += string.Format("and (case when (tran.is_cancel = 1 and tran.transaction_time between {0} and {1} and tran.cancel_time between {0} and {1}{2}{3}) then 1 else 0 end) =0\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO, checkLoginname,checkCashierRooom);
                }
                if (filter.TAKE_CANCEL == true)
                {
                    query += string.Format("union all\n");
                    query += string.Format("select \n");
                    query += string.Format("(CASE WHEN tran.IS_CANCEL = 1 and tran.PAY_FORM_ID = 8 THEN 1 ELSE tran.PAY_FORM_ID END) AS PAY_FORM_ID,\n");
                    query += string.Format("(CASE WHEN tran.IS_CANCEL = 1 AND tran.PAY_FORM_ID = 8 THEN 'Tiền mặt' ELSE tran.PAY_FORM_NAME END) AS PAY_FORM_NAME,\n");
                    query += string.Format("(CASE WHEN tran.IS_CANCEL = 1 AND tran.PAY_FORM_ID = 8 THEN '01' ELSE tran.PAY_FORM_CODE END) AS PAY_FORM_CODE,\n");

                    query += string.Format("0 is_on_time,\n");
                    query += string.Format("tran.cancel_loginname cashier_loginname,\n");
                    query += string.Format("tran.cancel_username cashier_username,\n");
                    query += string.Format("tran.cancel_time transaction_time,\n");
                    query += string.Format("(tran.cancel_time - mod(tran.cancel_time,1000000)) transaction_date,\n");
                    query += string.Format("-tran.amount amount,\n");
                    query += string.Format("trea.IN_TIME, \n");
                    query += string.Format("trea.OUT_TIME, \n");
                    query += string.Format("trea.TREATMENT_DAY_COUNT, \n");
                    query += string.Format("(case when tran.is_cancel=1 then -tran.amount else 0 end) cancel_amount,\n");
                    query += string.Format("-tran.kc_amount kc_amount,\n");
                    query += string.Format("-tran.exemption exemption,\n");
                    query += string.Format("cr.CASHIER_ROOM_CODE,\n");
                    query += string.Format("cr.CASHIER_ROOM_NAME,\n");
                    query += string.Format("tran.*\n");
                    query += string.Format("from v_his_transaction tran\n");
                    query += string.Format("join v_his_cashier_room cr on cr.id=tran.cancel_cashier_room_id\n");
                    query += string.Format("left join his_treatment trea on trea.id=tran.treatment_id\n");
                    query += string.Format("where 1=1\n");
                    query += string.Format("and tran.is_delete=0\n");

                    query += ClauseWhereTreatment(filter);
                    query += string.Format("and tran.is_cancel=1\n");
                    query += string.Format("and tran.cancel_time between {0} and {1}\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO);
                    if (filter.CASHIER_LOGINNAME != null)
                    {
                        query += string.Format("and tran.cancel_loginname='{0}'\n", filter.CASHIER_LOGINNAME);
                    }
                    if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                    {
                        query += string.Format("and tran.cancel_loginname in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                    }
                    if (filter.EXACT_CASHIER_ROOM_ID != null)
                    {
                        query += string.Format("and tran.cancel_cashier_room_id={0}\n", filter.EXACT_CASHIER_ROOM_ID);
                    }
                    if (filter.EXACT_CASHIER_ROOM_IDs != null)
                    {
                        query += string.Format("and tran.cancel_cashier_room_id in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                    }
                    if (filter.ACCOUNT_BOOK_ID != null)
                    {
                        query += string.Format("and tran.account_book_id={0}\n", filter.ACCOUNT_BOOK_ID);
                    }
                    if (filter.ACCOUNT_BOOK_IDs != null)
                    {
                        query += string.Format("and tran.account_book_id in ({0})\n", string.Join(",", filter.ACCOUNT_BOOK_IDs));
                    }
                    if (filter.TRANSACTION_TYPE_IDs != null)
                    {
                        query += string.Format("and tran.transaction_type_id in ({0})\n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
                    }
                    //if (filter.REQUEST_DEPARTMENT_IDs != null)
                    //{
                    //    query += string.Format("and ss.tdl_request_department_id in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                    //}
                    if (filter.PAY_FORM_IDs != null)
                    {
                       // query += string.Format("and tran.pay_form_id in ({0})\n", string.Join(",", filter.PAY_FORM_IDs));
                        query += string.Format("AND (CASE WHEN tran.IS_CANCEL = 1 and tran.PAY_FORM_ID = 8 THEN 1 ELSE tran.PAY_FORM_ID END) in({0}) ", string.Join(",", filter.PAY_FORM_IDs));

                    }
                    if (filter.BRANCH_ID != null)
                    {
                        query += string.Format("and cr.branch_id={0}\n", filter.BRANCH_ID);
                    }
                    if (filter.INPUT_DATA_ID_SALE_TYPE != null)
                    {
                        if (filter.INPUT_DATA_ID_SALE_TYPE == 1)
                        {
                            query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_EXP);
                        }
                        else if (filter.INPUT_DATA_ID_SALE_TYPE == 2)
                        {
                            query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER);
                        }
                        else if (filter.INPUT_DATA_ID_SALE_TYPE == 3)
                        {
                            query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_VACCIN);
                        }
                        else// if(filter.INPUT_DATA_ID_SALE_TYPE == 4)
                        {
                            query += string.Format("and tran.sale_type_id is null\n");
                        }
                    }
                    if (filter.IS_BILL_NORMAL.HasValue)
                    {
                        if (!filter.IS_BILL_NORMAL.Value)
                        {
                            query += string.Format("and tran.bill_type_id={0}\n", BILL_TYPE_ID__NORMAL);
                        }
                        else if (filter.IS_BILL_NORMAL.Value)
                        {
                            query += string.Format("and tran.bill_type_id={0}\n", BILL_TYPE_ID__SERVICE);
                        }
                    }
                    if (filter.IS_REMOVE_DUPPLICATE == true)
                    {
                        string checkLoginname = "";// " and tran.cashier_loginname = tran.cancel_loginname";
                        string checkCashierRooom = "";// "  and tran.cashier_room_id = tran.cancel_cashier_room_id";
                        List<string> cashierLoginnames = new List<string>();
                        if (filter.CASHIER_LOGINNAME != null)
                        {
                            cashierLoginnames.Add(filter.CASHIER_LOGINNAME ?? "");
                        }
                        if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                        {
                            cashierLoginnames.AddRange(filter.CASHIER_LOGINNAMEs);
                        }
                        if (cashierLoginnames.Count > 0)
                        {
                            checkLoginname = string.Format(" and tran.cashier_loginname in ('{0}') and tran.cancel_loginname in ('{0}')", string.Join("','", cashierLoginnames));
                        }
                        List<long> cashierRoomIds = new List<long>();
                        if (filter.EXACT_CASHIER_ROOM_ID != null)
                        {
                            cashierRoomIds.Add(filter.EXACT_CASHIER_ROOM_ID ?? 0);
                        }
                        if (IsNotNullOrEmpty(filter.EXACT_CASHIER_ROOM_IDs))
                        {
                            cashierRoomIds.AddRange(filter.EXACT_CASHIER_ROOM_IDs);
                        }
                        if (cashierRoomIds.Count > 0)
                        {
                            checkCashierRooom = string.Format(" and tran.cashier_room_id in ({0}) and tran.cancel_cashier_room_id in ({0})", string.Join(",", cashierRoomIds));
                        }
                        query += string.Format("and (case when (tran.is_cancel = 1 and tran.transaction_time between {0} and {1} and tran.cancel_time between {0} and {1}{2}{3}) then 1 else 0 end) =0\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO, checkLoginname, checkCashierRooom);
                    }
                }
                else if (filter.ADD_CANCEL_INFO != true)
                {
                    query += string.Format("and tran.is_cancel is null\n");
                }


                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00249RDO>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<LAST_ALTER_INFO> GetLastAlterInfo(Mrs00249Filter filter)
        {
            List<LAST_ALTER_INFO> result = null;
            try
            {
                string query = "";
                query += string.Format(" -- thong tin chuyen doi gan nhat\n");
                query += string.Format(" select \n");
                query += string.Format("tran.id transaction_id,\n");

                query += string.Format("max(pta.ksk_contract_id) keep(dense_rank last order by nvl(pta.log_time,0)) ksk_contract_id,\n");
                query += string.Format("max(pta.patient_type_id) keep(dense_rank last order by nvl(pta.log_time,0)) patient_type_id,\n");
                query += string.Format("max(pta.hein_card_number) keep(dense_rank last order by nvl(pta.log_time,0)) hein_card_number,\n");
                query += string.Format("max(pta.treatment_type_id) keep(dense_rank last order by nvl(pta.log_time,0)) treatment_type_id,\n");
                query += string.Format("max(dpt.department_id) keep(dense_rank last order by nvl(dpt.department_in_time,0)) department_id\n");
                query += string.Format("from his_transaction tran\n");
                query += string.Format("join v_his_cashier_room cr on cr.id=tran.cashier_room_id\n");
                query += string.Format("left join his_patient_type_alter pta on pta.treatment_id=tran.treatment_id and pta.log_time<=tran.transaction_time\n");
                query += string.Format("left join his_department_tran dpt on dpt.treatment_id=tran.treatment_id and dpt.department_in_time<=tran.transaction_time\n");
                query += string.Format("where 1=1\n");
                query += ClauseWhereTransaction(filter);
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.tdl_request_department_id in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and cr.branch_id={0}\n", filter.BRANCH_ID);
                }

                query += string.Format(" group by \n");
                query += string.Format("tran.id\n");


                result = new MOS.DAO.Sql.SqlDAO().GetSql<LAST_ALTER_INFO>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<CARD_INFO> GetCardInfo(Mrs00249Filter filter)
        {
            List<CARD_INFO> result = null;
            try
            {
                string query = "";
                query += string.Format(" -- thong tin the thong minh\n");
                query += string.Format(" select \n");

                query += string.Format("tran.id transaction_id,\n");
                query += string.Format("card.BANK_CARD_CODE,\n");
                query += string.Format("card.CARD_CODE,\n");
                query += string.Format("card.CARD_MAC,\n");
                query += string.Format("card.CARD_NUMBER\n");
                query += string.Format("from his_transaction tran\n");
                query += string.Format("join v_his_cashier_room cr on cr.id=tran.cashier_room_id\n");
                query += string.Format("join his_card card on card.patient_id=tran.tdl_patient_id\n");
                query += string.Format("where 1=1\n");

                query += ClauseWhereTransaction(filter);

                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.tdl_request_department_id in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and cr.branch_id={0}\n", filter.BRANCH_ID);
                }

                result = new MOS.DAO.Sql.SqlDAO().GetSql<CARD_INFO>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<SSB> GetSSB(Mrs00249Filter filter)
        {
            List<SSB> result = null;
            try
            {
                string query = "";
                query += string.Format(" -- thong tin chi tiet hoa don\n");
                query += string.Format(" select \n");
                query += string.Format("tran.id as TRANSACTION_ID ,\n");
                query += string.Format("sv.parent_id,\n");
                query += string.Format("ss.tdl_request_department_id,\n");
                query += string.Format("ss.tdl_request_room_id,\n");
                query += string.Format("ss.tdl_execute_room_id,\n");
                query += string.Format("ssb.bill_id,\n");
                query += string.Format("ssb.tdl_service_type_id,\n");
                query += string.Format("svt.service_type_code,\n");
                query += string.Format("hsvt.hein_service_type_code,\n");
                query += string.Format("sv.service_name,\n");
                query += string.Format("src.category_code,\n");
                query += string.Format("ssb.tdl_patient_type_id,\n");
                query += string.Format("sum(ssb.price) price,\n");
                query += string.Format("sum(ssb.TDL_TOTAL_HEIN_PRICE) TDL_TOTAL_HEIN_PRICE,\n");
                query += string.Format("sum(ssb.TDL_TOTAL_PATIENT_PRICE) TDL_TOTAL_PATIENT_PRICE,\n");
                query += string.Format("sum(ssb.TDL_TOTAL_PATIENT_PRICE_BHYT) TDL_TOTAL_PATIENT_PRICE_BHYT,\n");
                query += string.Format("sum(ssb.tdl_amount*nvl(ssb.TDL_REAL_PRICE,0)) TDL_TOTAL_PRICE,\n");
                query += string.Format("sum(ss.amount*ss.price) TOTAL_PRICE_SV\n");
                query += string.Format("from his_transaction tran\n");
                query += string.Format("join v_his_cashier_room cr on cr.id=tran.cashier_room_id\n");
                query += string.Format("join his_sere_serv_bill ssb on ssb.bill_id=tran.id\n");
                query += string.Format("join his_sere_serv ss on ss.id=ssb.sere_serv_id\n");
                query += string.Format("join his_service sv on sv.id=ssb.tdl_service_id\n");
                query += string.Format("join his_service_type svt on svt.id=ssb.tdl_service_type_id\n");
                query += string.Format("left join his_hein_service_type hsvt on hsvt.id=ssb.tdl_hein_service_type_id\n");
                query += string.Format("left join v_his_service_rety_cat src on src.service_id=ssb.tdl_service_id and src.report_type_code='MRS00249'\n");
                query += string.Format("where 1=1\n");
                query += ClauseWhereTransaction(filter);
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.tdl_request_department_id in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and cr.branch_id={0}\n", filter.BRANCH_ID);
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and ss.PATIENT_TYPE_ID in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                query += string.Format(" group by \n");
                query += string.Format("sv.parent_id,\n");
                query += string.Format("ss.tdl_request_department_id,\n");
                query += string.Format("ss.tdl_request_room_id,\n");
                query += string.Format("ss.tdl_execute_room_id,\n");
                query += string.Format("ssb.bill_id,\n");
                query += string.Format("ssb.tdl_patient_type_id,\n");
                query += string.Format("ssb.tdl_service_type_id,\n");
                query += string.Format("hsvt.hein_service_type_code,\n");
                query += string.Format("svt.service_type_code,\n");
                query += string.Format("tran.id,\n");
                query += string.Format("sv.service_name,\n");
                query += string.Format("src.category_code\n");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SSB>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        private string ClauseWhereTransaction(Mrs00249Filter filter)
        {
            string query = "";
            query += string.Format("and tran.is_delete=0\n");
            if (filter.TAKE_CANCEL == true)
            {
                query += string.Format("and (\n");
                query += string.Format("case when \n");
                query += string.Format("tran.transaction_time between {0} and {1}\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO);
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("and tran.cashier_loginname='{0}'\n", filter.CASHIER_LOGINNAME);
                }
                if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                {
                    query += string.Format("and tran.cashier_loginname in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }


                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("and tran.cashier_room_id={0}\n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("and tran.cashier_room_id in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }
                query += string.Format("then 1 \n");
                query += string.Format("when\n");
                query += string.Format("tran.is_cancel=1\n");
                query += string.Format("and tran.cancel_time between {0} and {1}\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO);
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("and tran.cancel_loginname='{0}'\n", filter.CASHIER_LOGINNAME);
                }
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("and tran.cancel_loginname in('{0}')\n", string.Join("','",filter.CASHIER_LOGINNAMEs));
                }


                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("and tran.cancel_cashier_room_id={0}\n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("and tran.cancel_cashier_room_id in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }
                query += string.Format("then 0\n");
                query += string.Format("else null end) in (0,1)\n");
            }
            else
            {

                query += string.Format("and tran.is_cancel is null\n");
                query += string.Format("and tran.transaction_time between {0} and {1}\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO);
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("and tran.cashier_loginname='{0}'\n", filter.CASHIER_LOGINNAME);
                }
                if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                {
                    query += string.Format("and tran.cashier_loginname in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
            }


            if (filter.ACCOUNT_BOOK_ID != null)
            {
                query += string.Format("and tran.account_book_id={0}\n", filter.ACCOUNT_BOOK_ID);
            }
            if (filter.ACCOUNT_BOOK_IDs != null)
            {
                query += string.Format("and tran.account_book_id in ({0})\n", string.Join(",", filter.ACCOUNT_BOOK_IDs));
            }
            if (filter.TRANSACTION_TYPE_IDs != null)
            {
                query += string.Format("and tran.transaction_type_id in ({0})\n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
            }
            if (filter.INPUT_DATA_ID_SALE_TYPE != null)
            {
                if (filter.INPUT_DATA_ID_SALE_TYPE == 1)
                {
                    query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_EXP);
                }
                else if (filter.INPUT_DATA_ID_SALE_TYPE == 2)
                {
                    query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER);
                }
                else if (filter.INPUT_DATA_ID_SALE_TYPE == 3)
                {
                    query += string.Format("and tran.sale_type_id={0}\n", IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_VACCIN);
                }
                else// if(filter.INPUT_DATA_ID_SALE_TYPE == 4)
                {
                    query += string.Format("and tran.sale_type_id is null\n");
                }
            }
            if (filter.IS_BILL_NORMAL.HasValue)
            {
                if (!filter.IS_BILL_NORMAL.Value)
                {
                    query += string.Format("and tran.bill_type_id={0}\n", BILL_TYPE_ID__NORMAL);
                }
                else if (filter.IS_BILL_NORMAL.Value)
                {
                    query += string.Format("and tran.bill_type_id={0}\n", BILL_TYPE_ID__SERVICE);
                }
            }
            if (filter.PAY_FORM_IDs != null)
            {
                query += string.Format("and tran.pay_form_id in ({0})\n", string.Join(",", filter.PAY_FORM_IDs));
            }

            // loc theo trang thai giao dich
            if (filter.INPUT_DATA_ID_STTRAN_TYPE == 1)
            {
                query += string.Format("and tran.is_active={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
            }
            if (filter.INPUT_DATA_ID_STTRAN_TYPE == 2)
            {
                query += string.Format("and tran.is_active={0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
            }

            if (filter.IS_REMOVE_DUPPLICATE == true)
            {
                string checkLoginname = "";// " and tran.cashier_loginname = tran.cancel_loginname";
                string checkCashierRooom = "";// "  and tran.cashier_room_id = tran.cancel_cashier_room_id";
                List<string> cashierLoginnames = new List<string>();
                if (filter.CASHIER_LOGINNAME != null)
                {
                    cashierLoginnames.Add(filter.CASHIER_LOGINNAME ?? "");
                }
                if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                {
                    cashierLoginnames.AddRange(filter.CASHIER_LOGINNAMEs);
                }
                if (cashierLoginnames.Count > 0)
                {
                    checkLoginname = string.Format(" and tran.cashier_loginname in ('{0}') and tran.cancel_loginname in ('{0}')", string.Join("','", cashierLoginnames));
                }
                List<long> cashierRoomIds = new List<long>();
                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    cashierRoomIds.Add(filter.EXACT_CASHIER_ROOM_ID ?? 0);
                }
                if (IsNotNullOrEmpty(filter.EXACT_CASHIER_ROOM_IDs))
                {
                    cashierRoomIds.AddRange(filter.EXACT_CASHIER_ROOM_IDs);
                }
                if (cashierRoomIds.Count > 0)
                {
                    checkCashierRooom = string.Format(" and tran.cashier_room_id in ({0}) and tran.cancel_cashier_room_id in ({0})", string.Join(",", cashierRoomIds));
                }
                query += string.Format("and (case when (tran.is_cancel = 1 and tran.transaction_time between {0} and {1} and tran.cancel_time between {0} and {1}{2}{3}) then 1 else 0 end) =0\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO, checkLoginname, checkCashierRooom);
            }
            return query;
        }
        private string ClauseWhereTreatment(Mrs00249Filter filter)
        {
            string query = "";
            //query += string.Format("and trea.is_delete=0\n");

            if (filter.TDL_TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("and trea.TDL_TREATMENT_TYPE_ID in ({0})\n", string.Join(",", filter.TDL_TREATMENT_TYPE_IDs));
            }
            if (filter.TDL_PATIENT_TYPE_IDs != null)
            {
                query += string.Format("and trea.TDL_PATIENT_TYPE_ID in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
            }
            if (filter.PATIENT_CLASSIFY_IDs != null)
            {
                query += string.Format("and trea.tdl_patient_classify_id in ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
            }
            if (IsNotNullOrEmpty(filter.DEPARTMENT_IDs))
            {
                query += string.Format("and trea.LAST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
            }

            return query;
        }

        public List<COUNT_FEE_LOCK> GetCountFeeLock(Mrs00249Filter filter)
        {
            List<COUNT_FEE_LOCK> result = null;
            try
            {
                string query = "";
                query += string.Format(" -- tong so bn khoa vien phi theo khoa va nguoi khoa\n");
                query += string.Format(" select \n");
                query += string.Format("round(fee_lock_time,-6) fee_lock_date,\n");
                query += string.Format("trea.FEE_LOCK_LOGINNAME,\n");
                query += string.Format("DP.DEPARTMENT_CODE END_DEPARTMENT_CODE,\n");
                query += string.Format("count(1) COUNT_TREATMENT\n");

                query += string.Format("from his_treatment trea\n");
                query += string.Format("join his_department dp on dp.id=trea.end_department_id\n");
                query += string.Format("where 1=1 and trea.is_active=0\n");

                query += string.Format("and trea.fee_lock_time between {0} and {1}\n", filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO);
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("and trea.FEE_LOCK_LOGINNAME='{0}'\n", filter.CASHIER_LOGINNAME);
                }
                if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs))
                {
                    query += string.Format("and trea.FEE_LOCK_LOGINNAME in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                query += ClauseWhereTreatment(filter);
                query += string.Format(" group by \n");
                query += string.Format("round(fee_lock_time,-6),\n");
                query += string.Format("trea.FEE_LOCK_LOGINNAME,\n");
                query += string.Format("DP.DEPARTMENT_CODE\n");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<COUNT_FEE_LOCK>(query);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<HISTORY_TIME> GetHistoryTime(Mrs00249Filter filter)
        {
            List<HISTORY_TIME> result = null;
            try
            {
                string query = "select tran.id transaction_id, tran1.transaction_time\n";
                query += "from his_transaction tran \n";
                query += string.Format("join his_transaction tran1 on tran1.treatment_id=tran.treatment_id and tran.transaction_type_id=tran1.transaction_type_id and tran1.transaction_time<tran.transaction_time and tran1.tdl_sere_serv_deposit_count is null and tran1.tdl_sese_depo_repay_count is null and (tran.sale_type_id is null and tran1.sale_type_id is null or tran.sale_type_id = tran1.sale_type_id)\n");
                query += "where 1=1 \n";

                query += ClauseWhereTransaction(filter);
                query += "order by tran.id, tran1.transaction_time \n";
                LogSystem.Info("SQL Query: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HISTORY_TIME>(query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                return result;
            }
            return result;
        }

        internal List<HIS_PATIENT_CLASSIFY> GetPatientClassify()
        {
            return new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>("select * from HIS_PATIENT_CLASSIFY where 1=1");
        }

        //internal List<REQUEST_DEPARTMENT_ID> GetClinicalDepa(Mrs00249Filter filter)
        //{
        //    List<REQUEST_DEPARTMENT_ID> result = null;
        //    try
        //    {
        //        string query = " -- khoa nhap vien\n";
        //        query += string.Format("select\n");
        //        query += string.Format("tran.id tran_id,\n");
        //        query += string.Format("min(dpt.department_id) keep(dense_rank first order by dpt.department_in_time) req_id\n");
        //        query += string.Format("from his_transaction tran\n");
        //        query += string.Format("join his_patient_type_alter pta on pta.treatment_id=tran.treatment_id and pta.treatment_type_id <>1\n");
        //        query += string.Format("join his_department_tran dpt on dpt.id=pta.department_tran_id\n");
        //        query += string.Format("where 1=1\n");

        //        query += ClauseWhereTransaction(filter);
        //        query += string.Format("group by\n");
        //        query += string.Format("tran.id\n");
        //        LogSystem.Info("SQL Query: " + query);
        //        result = new MOS.DAO.Sql.SqlDAO().GetSql<REQUEST_DEPARTMENT_ID>(query);
        //    }
        //    catch (Exception e)
        //    {
        //        LogSystem.Error(e);
        //        return result;
        //    }
        //    return result;
        //}
    }
}
