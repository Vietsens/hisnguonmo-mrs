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
using System.Reflection;
using MRS.Proccessor.Mrs00518;

namespace MRS.Processor.Mrs00518
{
    public partial class ManagerSql : BusinessBase
    {
        public List<SERE_SERV> GetSereServ(Mrs00518Filter filter)
        {
            List<SERE_SERV> result = null;
            try
            {
                string query = "";
                query += string.Format("--danh sach dich vu chi tiet\n");
                query += string.Format("select\n");
                query += string.Format("trea.IN_TIME,\n");
                query += string.Format("trea.CLINICAL_IN_TIME,\n");
                query += string.Format("trea.OUT_TIME,\n");
                query += string.Format("trea.TDL_PATIENT_DOB,\n");
                query += string.Format("trea.TDL_TREATMENT_TYPE_ID,\n");
                query += string.Format("trea.TDL_PATIENT_TYPE_ID,\n");
                query += string.Format("trea.TREATMENT_END_TYPE_ID,\n");
                query += string.Format("trea.END_ROOM_ID,\n");
                query += string.Format("trea.IS_EMERGENCY,\n");
                query += string.Format("ss.AMOUNT,\n");
                query += string.Format("ss.HEIN_CARD_NUMBER,\n");
                query += string.Format("ss.ID,\n");
                query += string.Format("ss.IS_DELETE,\n");
                query += string.Format("ss.IS_EXPEND,\n");
                query += string.Format("ss.IS_NO_EXECUTE,\n");
                query += string.Format("ss.PATIENT_TYPE_ID,\n");
                query += string.Format("(case when ss.PATIENT_TYPE_ID = {0} then 'BHYT' when ss.PATIENT_TYPE_ID = {1} then 'FEE' when ss.PATIENT_TYPE_ID = {2} then 'IS_FREE' when ss.PATIENT_TYPE_ID = {3} then 'DV' when ss.PATIENT_TYPE_ID = {4} then 'BUYMEDI' when ss.PATIENT_TYPE_ID = {5} then 'KSK' when ss.PATIENT_TYPE_ID = {6} then 'KSKHD' end) PATIENT_TYPE,\n", HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT, HisPatientTypeCFG.PATIENT_TYPE_ID__FEE, HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE, HisPatientTypeCFG.PATIENT_TYPE_ID__DV, HisPatientTypeCFG.PATIENT_TYPE_ID__BUYMEDI, HisPatientTypeCFG.PATIENT_TYPE_ID__KSK, HisPatientTypeCFG.PATIENT_TYPE_ID__KSKHD);
                query += string.Format("sr.EXAM_END_TYPE,\n");//Loai xu tri ket thuc kham. 1: Kham them, 2: Nhap vien, 3: Ket thuc dieu tri, 4: Ket thuc kham
                query += string.Format("ss.SERVICE_ID,\n");
                query += string.Format("ss.SERVICE_REQ_ID,\n");
                query += string.Format("ss.TDL_EXECUTE_ROOM_ID,\n");
                query += string.Format("ss.TDL_INTRUCTION_DATE,\n");
                query += string.Format("ss.TDL_INTRUCTION_TIME,\n");
                query += string.Format("ss.TDL_PATIENT_ID,\n");
                query += string.Format("ss.TDL_REQUEST_DEPARTMENT_ID,\n");
                query += string.Format("ss.TDL_EXECUTE_DEPARTMENT_ID,\n");
                query += string.Format("ss.TDL_REQUEST_LOGINNAME,\n");
                query += string.Format("ss.TDL_REQUEST_ROOM_ID,\n");
                query += string.Format("ss.TDL_REQUEST_USERNAME,\n");
                query += string.Format("ss.TDL_SERVICE_CODE,\n");
                query += string.Format("ss.TDL_SERVICE_DESCRIPTION,\n");
                query += string.Format("ss.TDL_SERVICE_NAME,\n");
                query += string.Format("ss.TDL_SERVICE_REQ_CODE,\n");
                query += string.Format("ss.TDL_SERVICE_REQ_TYPE_ID,\n");
                query += string.Format("ss.TDL_SERVICE_TYPE_ID,\n");
                query += string.Format("ss.TDL_SERVICE_UNIT_ID,\n");
                query += string.Format("ss.TDL_TREATMENT_CODE,\n");
                query += string.Format("ss.TDL_TREATMENT_ID\n");

                query += string.Format("from his_sere_serv ss\n");
                query += string.Format("left join lateral (select 1 from his_service_req sr where sr.id=ss.service_req_id) sr on sr.id=ss.service_req_id\n");
                query += string.Format("left join his_treatment trea on trea.id=ss.tdl_treatment_id\n");
                query += string.Format("left join his_sere_serv_bill ssb on ssb.sere_serv_id=SS.ID\n");
                query += string.Format("left join his_transaction tran on tran.id=ssb.bill_id\n");
                query += string.Format("where 1=1\n");
                query += string.Format("and ss.is_delete=0\n");
                query += string.Format("and ss.is_no_execute is null\n");
                query += string.Format("and ss.tdl_service_type_id in (1,4,11)\n");
                //- Điều kiện là BHyt phải được duyệt giám định BHYT, BN viện phí phải có giao dịch thanh toán mới hiển thị trên báo cáo
                query += string.Format("and (trea.clinical_in_time is null or ss.tdl_intruction_time<trea.clinical_in_time)\n");
                query += string.Format("and (trea.tdl_patient_type_id={2} and trea.is_active=0 and trea.fee_lock_time between {0} and {1} or trea.tdl_patient_type_id<>{2} and tran.is_cancel is null and tran.sale_type_id is null and tran.transaction_type_id=3 and tran.transaction_time between {0} and {1})\n", filter.TIME_FROM, filter.TIME_TO,HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERE_SERV>(query);
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
