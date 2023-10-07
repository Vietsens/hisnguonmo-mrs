using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00482
{
    public class ManagerSql
    {
        public List<Mrs00482RDO> GetDeposit(Mrs00482Filter filter)
        {
            List<Mrs00482RDO> result = new List<Mrs00482RDO>();
            try
            {
                string query = " --danh sach tam ung\n";
                query += "select \n ";
                query += "tran.ID, \n";
                query += "tran.transaction_type_id, \n";
                query += "tran.transaction_time, \n";
                query += "tran.transaction_code, \n";
                query += "trea.treatment_code, \n";
                query += "trea.in_time,\n";
                query += "trea.out_time, \n";
                query += "trea.fee_lock_time, \n";
                query += "trea.tdl_patient_code, \n";
                query += "trea.tdl_patient_name VIR_PATIENT_NAME, \n";
                query += "trea.tdl_patient_gender_NAME, \n";
                query += "trea.tdl_patient_address PATIENT_ADDRESS, \n";
                if (filter.ADD_INFO_HOPITAL_FEE == true)
                {
                    query += "treaf.TOTAL_PATIENT_PRICE, \n";
                }
                query += "ptt.patient_type_name, ";
                query += "dp.department_name NOW_DEPARTMENT_NAME, \n";
                query += "tran.transaction_date, \n";
                query += "acc.account_book_name, \n";
                query += "tran.account_book_id, \n";
                query += "tran.num_order NUM_ORDER, \n";
                query += "tran.einvoice_num_order, \n";
                query += "tran.bank_transaction_code, \n";
                query += "tran.bank_transaction_time, \n";
                query += "tran.is_cancel cancel_value, \n";
                query += "tran.cashier_loginname, \n";
                query += "tran.cashier_username, \n";
                query += "cr.cashier_room_code, \n";
                query += "cr.cashier_room_name, \n";
                query += "tran.pay_form_id, \n";
                query += "pf.pay_form_name, \n";
                query += "pf.pay_form_code, \n";
                query += "tran.cancel_loginname, \n";
                query += "tran.cancel_username, \n";
                query += "tran.description, \n";
                query += "tran.cancel_time, \n";
                query += "trea.tdl_hein_card_number, \n";
                query += "trea.tdl_patient_dob, \n";
                query += "nvl(tran.transfer_amount,0) transfer_amount, \n";
                query += "tran.AMOUNT \n";
                query += "from his_treatment trea  \n";
                query += "join his_rs.his_transaction tran on tran.treatment_id=trea.id \n";
                query += "left join his_pay_form pf on pf.id=tran.pay_form_id  \n";
                query += "left join his_account_book acc on acc.id=tran.account_book_id  \n";
                query += "left join v_his_cashier_room cr on cr.id=tran.cashier_room_id  \n";
                query += "left join his_patient_type  ptt on ptt.id=trea.tdl_patient_type_id \n";
                query += "left join his_department dp on dp.id=trea.last_department_id \n";
                if(filter.ADD_INFO_HOPITAL_FEE==true)
                {
                    query += " join v_his_treatment_fee treaf on treaf.id=trea.id \n";
                }
                if (filter.IS_DEPOSIT_NO_PAY.HasValue&& filter.IS_DEPOSIT_NO_PAY==true)
                {
                    query += "left join his_transaction bill on tran.treatment_id = bill.treatment_id and bill.transaction_type_id =3 \n";
                }
                query += "where 1=1  \n";
                query += "and tran.transaction_type_id=1 \n";
                if (filter.IS_DEPOSIT_NO_PAY.HasValue && filter.IS_DEPOSIT_NO_PAY==true)
                {
                    query += "and bill.id is null \n";
                }
                query += "and tran.is_delete=0 \n";
                if (filter.IS_ACTIVE.HasValue && filter.IS_ACTIVE == true)
                {
                    query += "and trea.IS_ACTIVE !=0 \n";
                }
                query += "and tran.TDL_SERE_SERV_DEPOSIT_COUNT is null \n";
                query += string.Format("and tran.transaction_time between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("and (tran.cashier_loginname='{0}')  \n", filter.CASHIER_LOGINNAME);
                }
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("and (tran.cashier_loginname in ('{0}')) \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.ACCOUNT_BOOK_ID != null)
                {
                    query += string.Format("and (tran.account_book_id ={0})  \n", filter.ACCOUNT_BOOK_ID);
                }
                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("and (tran.cashier_room_id={0})  \n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and (cr.branch_id={0})  \n", filter.BRANCH_ID);
                }
                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("and (tran.PAY_FORM_ID={0})  \n", filter.PAY_FORM_ID);
                }
                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("and (tran.PAY_FORM_ID in({0})) \n", string.Join(",", filter.PAY_FORM_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and (trea.TDL_PATIENT_TYPE_ID in({0})) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.DEPARTMENT_IDs!=null)
                {
                    query += string.Format("and (trea.LAST_DEPARTMENT_ID in({0})) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += "order by cashier_username,transaction_date,account_book_name,num_order,tran.AMOUNT desc ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00482RDO>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }

            return result;
        }

        public List<Mrs00482RDO> GetRepay(Mrs00482Filter filter)
        {
            List<Mrs00482RDO> result = new List<Mrs00482RDO>();
            try
            {
                string query = " --danh sach hoan ung\n";
                query += "select \n ";
                query += "tran.ID, \n";
                query += "tran.transaction_type_id, \n";
                query += "tran.transaction_time, \n";
                query += "tran.transaction_code, \n";
                query += "trea.treatment_code, \n";
                query += "trea.in_time,\n";
                query += "trea.out_time, \n";
                query += "trea.fee_lock_time, \n";
                query += "trea.tdl_patient_code, \n";
                query += "trea.tdl_patient_name VIR_PATIENT_NAME, \n";
                query += "trea.tdl_patient_gender_NAME, \n";
                query += "trea.tdl_patient_address PATIENT_ADDRESS, \n";
                query += "ptt.patient_type_name, ";
                query += "dp.department_name NOW_DEPARTMENT_NAME, \n";
                query += "tran.transaction_date, \n";
                query += "acc.account_book_name, \n";
                query += "tran.account_book_id, \n";
                query += "tran.num_order NUM_ORDER, \n";
                query += "tran.einvoice_num_order, \n";
                query += "tran.bank_transaction_code, \n";
                query += "tran.bank_transaction_time, \n";
                query += "tran.is_cancel cancel_value, \n";
                query += "tran.cashier_loginname, \n";
                query += "tran.cashier_username, \n";
                query += "cr.cashier_room_code, \n";
                query += "cr.cashier_room_name, \n";
                query += "tran.pay_form_id, \n";
                query += "pf.pay_form_name, \n";
                query += "pf.pay_form_code, \n";
                query += "tran.cancel_loginname, \n";
                query += "tran.cancel_username, \n";
                query += "tran.description, \n";
                query += "tran.cancel_time, \n";
                query += "trea.tdl_hein_card_number, \n";
                query += "trea.tdl_patient_dob, \n";
                query += "nvl(tran.transfer_amount,0) transfer_amount, \n";
                query += "tran.AMOUNT \n";
                query += "from his_treatment trea  \n";
                query += "join his_rs.his_transaction tran on tran.treatment_id=trea.id \n";
                query += "left join his_pay_form pf on pf.id=tran.pay_form_id  \n";
                query += "left join his_account_book acc on acc.id=tran.account_book_id  \n";
                query += "left join v_his_cashier_room cr on cr.id=tran.cashier_room_id  \n";
                query += "left join his_patient_type  ptt on ptt.id=trea.tdl_patient_type_id \n";
                query += "left join his_department dp on dp.id=trea.last_department_id \n";
                query += "where 1=1  \n";
                query += "and tran.transaction_type_id=2 \n";
                query += "and tran.is_delete=0 \n";
                query += "and tran.is_cancel is null \n";
                if (filter.IS_ACTIVE.HasValue&&filter.IS_ACTIVE==true)
                {
                    query += "and trea.IS_ACTIVE !=0 \n";
                }
                query += "and tran.TDL_SESE_DEPO_REPAY_COUNT is null \n";
                query += string.Format("and tran.transaction_time between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("and (tran.cashier_loginname='{0}')  \n", filter.CASHIER_LOGINNAME);
                }
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("and (tran.cashier_loginname in ('{0}')) \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.ACCOUNT_BOOK_ID != null)
                {
                    query += string.Format("and (tran.account_book_id ={0})  \n", filter.ACCOUNT_BOOK_ID);
                }
                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("and (tran.cashier_room_id={0})  \n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and (cr.branch_id={0})  \n", filter.BRANCH_ID);
                }
                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("and (tran.PAY_FORM_ID={0})  \n", filter.PAY_FORM_ID);
                }
                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("and (tran.PAY_FORM_ID in({0})) \n", string.Join(",", filter.PAY_FORM_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and (trea.TDL_PATIENT_TYPE_ID in({0})) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.DEPARTMENT_IDs!=null)
                {
                    query += string.Format("and (trea.LAST_DEPARTMENT_ID in({0})) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += "order by cashier_username,transaction_date,account_book_name,num_order,tran.AMOUNT desc ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00482RDO>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }

            return result;
        }

        public List<Mrs00482RDO> GetDepositCancel(Mrs00482Filter filter)
        {
            List<Mrs00482RDO> result = new List<Mrs00482RDO>();
            try
            {
                string query = " --danh sach huy tam ung\n";
                query += "select \n ";
                query += "tran.transaction_type_id, \n";
                query += "tran.ID, \n";
                query += "tran.cancel_time transaction_time, \n";
                query += "tran.transaction_code, \n";
                query += "trea.treatment_code, \n";
                query += "trea.in_time,\n";
                query += "trea.out_time, \n";
                query += "trea.fee_lock_time, \n";
                query += "trea.tdl_patient_code, \n";
                query += "trea.tdl_patient_name VIR_PATIENT_NAME, \n";
                query += "trea.tdl_patient_gender_NAME, \n";
                query += "trea.tdl_patient_address PATIENT_ADDRESS, \n";
                query += "ptt.patient_type_name, ";
                query += "dp.department_name NOW_DEPARTMENT_NAME, \n";
                query += "tran.cancel_time - mod(tran.cancel_time,1000000) transaction_date, \n";
                query += "acc.account_book_name, \n";
                query += "tran.account_book_id, \n";
                query += "tran.num_order NUM_ORDER, \n";
                query += "tran.einvoice_num_order, \n";
                query += "tran.bank_transaction_code, \n";
                query += "tran.bank_transaction_time, \n";
                query += "tran.is_cancel cancel_value, \n";
                query += "tran.cancel_loginname cashier_loginname, \n";
                query += "tran.cancel_username cashier_username, \n";
                query += "cr.cashier_room_code, \n";
                query += "cr.cashier_room_name, \n";
                query += "tran.pay_form_id, \n";
                query += "pf.pay_form_name, \n";
                query += "pf.pay_form_code, \n";
                query += "tran.cancel_loginname, \n";
                query += "tran.cancel_username, \n";
                query += "tran.description, \n";
                query += "tran.cancel_time, \n";
                query += "trea.tdl_hein_card_number, \n";
                query += "trea.tdl_patient_dob, \n";
                query += "nvl(tran.transfer_amount,0) transfer_amount, \n";
                query += "tran.AMOUNT amount_cancel \n";
                query += "from his_treatment trea  \n";
                query += "join his_rs.his_transaction tran on tran.treatment_id=trea.id \n";
                query += "left join his_pay_form pf on pf.id=tran.pay_form_id  \n";
                query += "left join his_account_book acc on acc.id=tran.account_book_id  \n";
                query += "left join v_his_cashier_room cr on cr.id=tran.cashier_room_id  \n";
                query += "left join his_patient_type  ptt on ptt.id=trea.tdl_patient_type_id \n";
                query += "left join his_department dp on dp.id=trea.last_department_id \n";
                query += "where 1=1  \n";
                query += "and tran.transaction_type_id=1 \n";
                query += "and tran.is_delete=0 \n";
                query += "and tran.is_cancel=1 \n";
                if (filter.IS_ACTIVE.HasValue && filter.IS_ACTIVE == true)
                {
                    query += "and trea.IS_ACTIVE !=0 \n";
                }
                query += "and tran.TDL_SESE_DEPO_REPAY_COUNT is null \n";
                query += string.Format("and tran.cancel_time between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("and (tran.cancel_loginname='{0}')  \n", filter.CASHIER_LOGINNAME);
                }
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("and (tran.cancel_loginname in ('{0}')) \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.ACCOUNT_BOOK_ID != null)
                {
                    query += string.Format("and (tran.account_book_id ={0})  \n", filter.ACCOUNT_BOOK_ID);
                }
                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("and (tran.cashier_room_id={0})  \n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and (cr.branch_id={0})  \n", filter.BRANCH_ID);
                }
                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("and (tran.PAY_FORM_ID={0})  \n", filter.PAY_FORM_ID);
                }
                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("and (tran.PAY_FORM_ID in({0})) \n", string.Join(",", filter.PAY_FORM_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and (trea.TDL_PATIENT_TYPE_ID in({0})) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.DEPARTMENT_IDs!=null)
                {
                    query += string.Format("and (trea.LAST_DEPARTMENT_ID in({0})) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                query += "order by cashier_username,transaction_date,account_book_name,num_order,tran.AMOUNT desc ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00482RDO>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }

            return result;
        }
    }
}
