using System;
using System.Collections.Generic;
using Inventec.Common.Logging;
using MOS.DAO.Sql;
using MRS.Processor.Mrs00737;
using MOS.EFMODEL.DataModels;

internal class ManagerSql
{
	internal List<Mrs00737RDO> GetBillAmount(Mrs00737Filter filter)
	{
		List<Mrs00737RDO> list = null;
		try
		{
			string text = " -- thanh toan hoac hoan ung\n";
			text += "select \n";
            text += string.Format("{0} json_pay_form_code,\n", AddJsonPayForm());
			text += "tran.transaction_date,\n";
			text += "trea.in_time,\n";
            text += "de.department_name,\n";
			text += "trea.tdl_patient_name patient_name,\n";
			text += "tran.tdl_treatment_code treatment_code,\n";
			text += "trea.tdl_patient_code patient_code,\n";
			text += "trea.tdl_hein_card_number hein_card_number,\n";
			text += "tran.cashier_loginname bill_cashier_loginname,\n";
            text += "tran.cashier_username bill_cashier_username,\n";
            text += "max(case when acc.bill_type_id=1 and svt.service_type_code <>'AN'  then pf.pay_form_name else ' ' end) pay_form_normal_name,\n";
            text += "max(case when acc.bill_type_id=2  and svt.service_type_code <>'AN'  then pf.pay_form_name else ' ' end) pay_form_service_name,\n";
            text += "max(case when acc.bill_type_id in (1,2) and svt.service_type_code <>'AN' then ' ' else pf.pay_form_name end) pay_form_an_name,\n";
            text += "max(case when acc.bill_type_id=1 and svt.service_type_code <>'AN'  then pf.pay_form_code else ' ' end) pay_form_normal_code,\n";
            text += "max(case when acc.bill_type_id=2  and svt.service_type_code <>'AN'  then pf.pay_form_code else ' ' end) pay_form_service_code,\n";
            text += "max(case when acc.bill_type_id in (1,2) and svt.service_type_code <>'AN' then ' ' else pf.pay_form_code end) pay_form_an_code,\n";
			text += "sum(case when acc.bill_type_id=1 and svt.service_type_code <>'AN'  then nvl(ssb.price,0) else 0 end) bill_amount_vp,\n";
			text += "sum(case when acc.bill_type_id=2  and svt.service_type_code <>'AN'  then nvl(ssb.price,0) else 0 end) bill_amount_dv,\n";
			text += "sum(case when acc.bill_type_id in (1,2) and svt.service_type_code <>'AN' then 0 else nvl(ssb.price,0) end) bill_amount_an,\n";
			text += "max(case when tran.transaction_type_id=2 then tran.transaction_code else null end) rep_transaction_code,\n";
			text += "sum(case when tran.transaction_type_id=2 then tran.amount else 0 end) total_repay_amount,\n";
			text += "sum(nvl(ssb.price,0)-(case when tran.transaction_type_id=2 then tran.amount else 0 end)) diff\n";
			text += "from his_transaction tran\n";
			text += "left join his_sere_serv_bill ssb on ssb.bill_id=tran.id\n";
			text += "join his_treatment trea on trea.id=tran.treatment_id\n";
			text += "join his_account_book acc on acc.id=tran.account_book_id\n";
			text += "join his_pay_form pf on pf.id=tran.pay_form_id\n";
			text += "left join his_service_type svt on svt.id=ssb.tdl_service_type_id\n";
            text += "left join his_department de on de.id=trea.end_department_id\n";
			text += "where 1=1\n";
			text += "--and tran.is_cancel is null\n";
            text += string.Format("and tran.transaction_time between {0} and {1} \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);
			if (filter.TREATMENT_TYPE_IDs != null)
			{
				text += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }
            if (filter.PAY_FORM_IDs != null)
            {
                text += string.Format("AND tran.pay_form_id in ({0}) \n", string.Join(",", filter.PAY_FORM_IDs));
            }
            if (filter.BRANCH_ID != null)
            {
                text += string.Format("AND trea.branch_id ={0} \n", filter.BRANCH_ID);
            }
			if (filter.CASHIER_LOGINNAMEs != null)
			{
				text += string.Format("AND tran.cashier_loginname in ('{0}') \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
			}
			text += "and tran.transaction_type_id in (3,2) \n";
			text += "and (tran.is_cancel is null or tran.transaction_date+1000000<tran.cancel_time)\n";
			text += "group by\n";
			text += "tran.transaction_date,\n";
			text += "trea.in_time,\n";
            text += "de.department_name,\n";
			text += "trea.tdl_patient_name,\n";
			text += "tran.tdl_treatment_code,\n";
			text += "trea.tdl_patient_code,\n";
			text += "trea.tdl_hein_card_number,\n";
			text += "tran.cashier_loginname,\n";
			text += "tran.cashier_username\n";
			LogSystem.Info("SQL: " + text);
			list = new SqlDAO().GetSql<Mrs00737RDO>(text, new object[0]);
		}
		catch (Exception ex)
		{
			list = null;
			LogSystem.Error(ex);
		}
		return list;
	}

    private string AddJsonPayForm()
    {
        string result = "'{'";
        try
        {
            List<HIS_PAY_FORM> listPayForm = new SqlDAO().GetSql<HIS_PAY_FORM>("select * from his_pay_form", new object[0]);
            foreach (var item in listPayForm)
            {
                result += string.Format("||'\"{0}\":'||sum(case when pf.pay_form_code='{0}' then nvl(ssb.price,0) else 0 end)||','", item.PAY_FORM_CODE);
            }
            result += "||'\"0\":0}'";
        }
        catch ( Exception ex)
        {
            LogSystem.Error(ex);
        }
        return result;
    }

	internal List<Mrs00737RDO> GetBillCancel(Mrs00737Filter filter)
	{
		List<Mrs00737RDO> list = null;
		try
		{
			string text = " --huy thanh toan hoac huy hoan ung\n";
            text += "select \n";
            text += string.Format("{0} json_pay_form_code,\n", AddJsonPayForm());
			text += "(tran.cancel_time-mod(tran.cancel_time,1000000)) transaction_date,\n";
			text += "trea.in_time,\n";
            text += "de.department_name,\n";
			text += "trea.tdl_patient_name patient_name,\n";
			text += "tran.tdl_treatment_code treatment_code,\n";
			text += "trea.tdl_patient_code patient_code,\n";
			text += "trea.tdl_hein_card_number hein_card_number,\n";
			text += "tran.cancel_loginname bill_cashier_loginname,\n";
			text += "tran.cancel_username bill_cashier_username,\n";
			text += "max(case when acc.bill_type_id=1 and svt.service_type_code <>'AN'  then pf.pay_form_name else ' ' end) pay_form_normal_name,\n";
            text += "max(case when acc.bill_type_id=2  and svt.service_type_code <>'AN'  then pf.pay_form_name else ' ' end) pay_form_service_name,\n";
            text += "max(case when acc.bill_type_id in (1,2) and svt.service_type_code <>'AN' then ' ' else pf.pay_form_name end) pay_form_an_name,\n";
            text += "max(case when acc.bill_type_id=1 and svt.service_type_code <>'AN'  then pf.pay_form_code else ' ' end) pay_form_normal_code,\n";
            text += "max(case when acc.bill_type_id=2  and svt.service_type_code <>'AN'  then pf.pay_form_code else ' ' end) pay_form_service_code,\n";
            text += "max(case when acc.bill_type_id in (1,2) and svt.service_type_code <>'AN' then ' ' else pf.pay_form_code end) pay_form_an_code,\n";
			text += "sum(case when acc.bill_type_id=1 and svt.service_type_code <>'AN'  then nvl(ssb.price,0) else 0 end) bill_amount_vp,\n";
			text += "sum(case when acc.bill_type_id=2  and svt.service_type_code <>'AN'  then nvl(ssb.price,0) else 0 end) bill_amount_dv,\n";
			text += "sum(case when acc.bill_type_id in (1,2) and svt.service_type_code <>'AN' then 0 else nvl(ssb.price,0) end) bill_amount_an,\n";
			text += "max(case when tran.transaction_type_id=2 then tran.transaction_code else null end) rep_transaction_code,\n";
			text += "sum(case when tran.transaction_type_id=2 then tran.amount else 0 end) total_repay_amount,\n";
			text += "sum(nvl(ssb.price,0)-(case when tran.transaction_type_id=2 then tran.amount else 0 end)) diff\n";
			text += "from his_transaction tran\n";
			text += "left join his_sere_serv_bill ssb on ssb.bill_id=tran.id\n";
			text += "join his_treatment trea on trea.id=tran.treatment_id\n";
			text += "join his_account_book acc on acc.id=tran.account_book_id\n";
			text += "join his_pay_form pf on pf.id=tran.pay_form_id\n";
			text += "left join his_service_type svt on svt.id=ssb.tdl_service_type_id\n";
            text += "left join his_department de on de.id=trea.end_department_id\n";
			text += "where 1=1\n";
			text += "and tran.is_cancel =1\n";
            text += string.Format("and tran.cancel_time between {0} and {1} \n", filter.FEE_LOCK_TIME_FROM, filter.FEE_LOCK_TIME_TO);
			if (filter.TREATMENT_TYPE_IDs != null)
			{
				text += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
			}
			if (filter.PAY_FORM_IDs != null)
			{
				text += string.Format("AND tran.pay_form_id in ({0}) \n", string.Join(",", filter.PAY_FORM_IDs));
			}
			if (filter.CASHIER_LOGINNAMEs != null)
			{
				text += string.Format("AND tran.cancel_loginname in ('{0}') \n", string.Join("','", filter.CASHIER_LOGINNAMEs));
            }
            if (filter.BRANCH_ID != null)
            {
                text += string.Format("AND trea.branch_id ={0} \n", filter.BRANCH_ID);
            }
			text += "and tran.transaction_type_id in (3,2) \n";
			text += "and tran.transaction_date+1000000<tran.cancel_time\n";
			text += "group by\n";
			text += "(tran.cancel_time-mod(tran.cancel_time,1000000)),\n";
			text += "trea.in_time,\n";
            text += "de.department_name,\n";
			text += "trea.tdl_patient_name,\n";
			text += "tran.tdl_treatment_code,\n";
			text += "trea.tdl_patient_code,\n";
			text += "trea.tdl_hein_card_number,\n";
			text += "tran.cancel_loginname,\n";
			text += "tran.cancel_username\n";
			LogSystem.Info("SQL: " + text);
			list = new SqlDAO().GetSql<Mrs00737RDO>(text, new object[0]);
		}
		catch (Exception ex)
		{
			list = null;
			LogSystem.Error(ex);
		}
		return list;
	}
}
