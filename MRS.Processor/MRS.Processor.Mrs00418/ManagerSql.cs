using Inventec.Core;
using MOS.DAO.Sql;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00418
{
    class ManagerSql
    {

        internal List<SeSe> GetSS(Mrs00418Filter filter)
        {
            List<SeSe> result = new List<SeSe>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("-- thong tin dich vu su dung\n");

            query += string.Format("select\n");
            query += string.Format("ss.ADD_PRICE	,\n");
            query += string.Format("ss.AMOUNT	,\n");
            query += string.Format("ss.AMOUNT_TEMP	,\n");
            query += string.Format("ss.CREATE_TIME	,\n");
            query += string.Format("ss.CREATOR	,\n");
            query += string.Format("ss.DISCOUNT	,\n");
            query += string.Format("ss.DISCOUNT_LOGINNAME	,\n");
            query += string.Format("ss.DISCOUNT_USERNAME	,\n");
            query += string.Format("ss.EKIP_ID	,\n");
            query += string.Format("ss.EQUIPMENT_SET_ID	,\n");
            query += string.Format("ss.EQUIPMENT_SET_ORDER	,\n");
            query += string.Format("ss.EXECUTE_TIME	,\n");
            query += string.Format("ss.EXP_MEST_MATERIAL_ID	,\n");
            query += string.Format("ss.EXP_MEST_MEDICINE_ID	,\n");
            query += string.Format("ss.EXPEND_TYPE_ID	,\n");
            query += string.Format("ss.GROUP_CODE	,\n");
            query += string.Format("ss.HEIN_APPROVAL_ID	,\n");
            query += string.Format("ss.HEIN_CARD_NUMBER	,\n");
            query += string.Format("ss.HEIN_LIMIT_PRICE	,\n");
            query += string.Format("ss.HEIN_LIMIT_RATIO	,\n");
            query += string.Format("ss.HEIN_NORMAL_PRICE	,\n");
            query += string.Format("ss.HEIN_PRICE	,\n");
            query += string.Format("ss.HEIN_RATIO	,\n");
            query += string.Format("ss.ID	,\n");
            query += string.Format("ss.INVOICE_ID	,\n");
            query += string.Format("ss.IS_ACCEPTING_NO_EXECUTE	,\n");
            query += string.Format("ss.IS_ACTIVE	,\n");
            query += string.Format("ss.IS_ADDITION	,\n");
            query += string.Format("ss.IS_DELETE	,\n");
            query += string.Format("ss.IS_EXPEND	,\n");
            query += string.Format("ss.IS_FUND_ACCEPTED	,\n");
            query += string.Format("ss.IS_NO_EXECUTE	,\n");
            query += string.Format("ss.IS_NO_HEIN_DIFFERENCE	,\n");
            query += string.Format("ss.IS_NO_PAY	,\n");
            query += string.Format("ss.IS_NOT_PRES	,\n");
            query += string.Format("ss.IS_NOT_USE_BHYT	,\n");
            query += string.Format("ss.IS_OTHER_SOURCE_PAID	,\n");
            query += string.Format("ss.IS_OUT_PARENT_FEE	,\n");
            query += string.Format("ss.IS_SENT_EXT	,\n");
            query += string.Format("ss.IS_SPECIMEN	,\n");
            query += string.Format("ss.IS_TEMP_BED_PROCESSED	,\n");
            query += string.Format("ss.IS_USER_PACKAGE_PRICE	,\n");
            query += string.Format("ss.JSON_PATIENT_TYPE_ALTER	,\n");
            query += string.Format("ss.LIMIT_PRICE	,\n");
            query += string.Format("ss.MATERIAL_ID	,\n");
            query += string.Format("ss.MEDICINE_ID	,\n");
            query += string.Format("ss.MODIFIER	,\n");
            query += string.Format("ss.MODIFY_TIME	,\n");
            query += string.Format("ss.NO_EXECUTE_REASON	,\n");
            query += string.Format("ss.ORIGINAL_PRICE	,\n");
            query += string.Format("ss.OTHER_PAY_SOURCE_ID	,\n");
            query += string.Format("ss.OTHER_SOURCE_PRICE	,\n");
            query += string.Format("ss.OVERTIME_PRICE	,\n");
            query += string.Format("ss.PACKAGE_ID	,\n");
            query += string.Format("ss.PACKAGE_PRICE	,\n");
            query += string.Format("ss.PARENT_ID	,\n");
            query += string.Format("ss.PATIENT_PRICE_BHYT	,\n");
            query += string.Format("ss.PATIENT_TYPE_ID	,\n");
            query += string.Format("ss.PRICE	,\n");
            query += string.Format("ss.PRIMARY_PATIENT_TYPE_ID	,\n");
            query += string.Format("ss.PRIMARY_PRICE	,\n");
            query += string.Format("ss.SERVICE_CONDITION_ID	,\n");
            query += string.Format("ss.SERVICE_ID	,\n");
            query += string.Format("ss.SERVICE_REQ_ID	,\n");
            query += string.Format("ss.SHARE_COUNT	,\n");
            query += string.Format("ss.STENT_ORDER	,\n");
            query += string.Format("ss.TDL_ACTIVE_INGR_BHYT_CODE	,\n");
            query += string.Format("ss.TDL_ACTIVE_INGR_BHYT_NAME	,\n");
            query += string.Format("ss.TDL_BILL_OPTION	,\n");
            query += string.Format("ss.TDL_EXECUTE_BRANCH_ID	,\n");
            query += string.Format("ss.TDL_EXECUTE_DEPARTMENT_ID	,\n");
            query += string.Format("ss.TDL_EXECUTE_GROUP_ID	,\n");
            query += string.Format("ss.TDL_EXECUTE_ROOM_ID	,\n");
            query += string.Format("ss.TDL_HEIN_ORDER	,\n");
            query += string.Format("ss.TDL_HEIN_SERVICE_BHYT_CODE	,\n");
            query += string.Format("ss.TDL_HEIN_SERVICE_BHYT_NAME	,\n");
            query += string.Format("ss.TDL_HEIN_SERVICE_TYPE_ID	,\n");
            query += string.Format("ss.TDL_HST_BHYT_CODE	,\n");
            query += string.Format("ss.TDL_INTRUCTION_DATE	,\n");
            query += string.Format("ss.TDL_INTRUCTION_TIME	,\n");
            query += string.Format("ss.TDL_IS_MAIN_EXAM	,\n");
            query += string.Format("ss.TDL_IS_OUT_OF_DRG	,\n");
            query += string.Format("ss.TDL_IS_SPECIFIC_HEIN_PRICE	,\n");
            query += string.Format("ss.TDL_MATERIAL_GROUP_BHYT	,\n");
            query += string.Format("ss.TDL_MEDICINE_BID_NUM_ORDER	,\n");
            query += string.Format("ss.TDL_MEDICINE_CONCENTRA	,\n");
            query += string.Format("ss.TDL_MEDICINE_PACKAGE_NUMBER	,\n");
            query += string.Format("ss.TDL_MEDICINE_REGISTER_NUMBER	,\n");
            query += string.Format("ss.TDL_PACS_TYPE_CODE	,\n");
            query += string.Format("ss.TDL_PATIENT_ID	,\n");
            query += string.Format("ss.TDL_REQUEST_DEPARTMENT_ID	,\n");
            query += string.Format("ss.TDL_REQUEST_LOGINNAME	,\n");
            query += string.Format("ss.TDL_REQUEST_ROOM_ID	,\n");
            query += string.Format("ss.TDL_REQUEST_USER_TITLE	,\n");
            query += string.Format("ss.TDL_REQUEST_USERNAME	,\n");
            query += string.Format("ss.TDL_SERVICE_CODE	,\n");
            query += string.Format("ss.TDL_SERVICE_DESCRIPTION	,\n");
            query += string.Format("ss.TDL_SERVICE_NAME	,\n");
            query += string.Format("ss.TDL_SERVICE_REQ_CODE	,\n");
            query += string.Format("ss.TDL_SERVICE_REQ_TYPE_ID	,\n");
            query += string.Format("ss.TDL_SERVICE_TAX_RATE_TYPE	,\n");
            query += string.Format("ss.TDL_SERVICE_TYPE_ID	,\n");
            query += string.Format("ss.TDL_SERVICE_UNIT_ID	,\n");
            query += string.Format("ss.TDL_SPECIALITY_CODE	,\n");
            query += string.Format("ss.TDL_TREATMENT_CODE	,\n");
            query += string.Format("ss.TDL_TREATMENT_ID	,\n");
            query += string.Format("ss.USE_ORIGINAL_UNIT_FOR_PRES	,\n");
            query += string.Format("ss.USER_PRICE	,\n");
            query += string.Format("ss.VAT_RATIO	,\n");
            query += string.Format("ss.VIR_HEIN_PRICE	,\n");
            query += string.Format("ss.VIR_PATIENT_PRICE	,\n");
            query += string.Format("ss.VIR_PATIENT_PRICE_BHYT	,\n");
            query += string.Format("ss.VIR_PRICE	,\n");
            query += string.Format("ss.VIR_PRICE_NO_ADD_PRICE	,\n");
            query += string.Format("ss.VIR_PRICE_NO_EXPEND	,\n");
            query += string.Format("ss.VIR_TOTAL_HEIN_PRICE	,\n");
            query += string.Format("ss.VIR_TOTAL_PATIENT_PRICE	,\n");
            query += string.Format("ss.VIR_TOTAL_PATIENT_PRICE_BHYT	,\n");
            query += string.Format("ss.VIR_TOTAL_PATIENT_PRICE_NO_DC	,\n");
            query += string.Format("ss.VIR_TOTAL_PATIENT_PRICE_TEMP	,\n");
            query += string.Format("ss.VIR_TOTAL_PRICE	,\n");
            query += string.Format("ss.VIR_TOTAL_PRICE_NO_ADD_PRICE	,\n");
            query += string.Format("ss.VIR_TOTAL_PRICE_NO_EXPEND	\n");


            query += string.Format("from his_rs.his_sere_serv ss\n");
            query += string.Format("join his_rs.his_service_req sr on sr.id=ss.service_req_id\n");
            query += string.Format("left join his_rs.his_sere_serv_tein sst on sst.tdl_service_req_id=sr.id\n");
            query += string.Format("where 1=1\n");
            if (filter.TRUE_OR_FALSE == true)
            {
                query += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("and sr.service_req_stt_id in ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }
            }
            else
            {
                query += string.Format("and (sst.id is not null and sst.value is not null and sst.modify_time between {0} and {1} or sst.id is null and sr.finish_time between {0} and {1} and sr.service_req_stt_id = {2})\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
            }
            query += string.Format("and ss.is_delete=0\n");
            query += string.Format("and ss.is_no_execute is null\n");

            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("and ss.tdl_execute_department_id = {0}\n", filter.DEPARTMENT_ID);
            }

            if (filter.DEPARTMENT_IDs != null)
            {
                query += string.Format("and ss.tdl_execute_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SqlDAO().GetSql<SeSe>(paramGet, query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<SeSe>(paramGet, query);
            if (result != null)
            {
                Inventec.Common.Logging.LogSystem.Info("SQLCount: " + result.Count);
            }
            return result;
        }
    }
}
