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
using ACS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00290
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00290RDO> GetSereServDO(Mrs00290Filter filter)
        {
            List<Mrs00290RDO> result = new List<Mrs00290RDO>();
            try
            {
               
                string query = " --chi tiet dich vu thuc hien\n";
                query += "SELECT\n";
                query += "SS.*,\n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("TRAN.TRANSACTION_DATE REPORT_DATE,\n");
                    query += string.Format("TRAN.TRANSACTION_TIME REPORT_TIME,\n");
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("ROUND(TREA.FEE_LOCK_TIME,-6) REPORT_DATE,\n");
                    query += string.Format("TREA.FEE_LOCK_TIME REPORT_TIME,\n");
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("TREA.OUT_DATE REPORT_DATE,\n");
                    query += string.Format("TREA.OUT_TIME REPORT_TIME,\n");
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("ROUND(sr.finish_time,-6) REPORT_DATE,\n");
                    query += string.Format("sr.finish_time REPORT_TIME,\n");
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("ROUND(sr.start_time,-6) REPORT_DATE,\n");
                    query += string.Format("sr.start_time REPORT_TIME,\n");
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("SR.INTRUCTION_DATE REPORT_DATE,\n");
                    query += string.Format("sr.intruction_time REPORT_TIME,\n");
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("TREA.IN_DATE REPORT_DATE,\n");
                    query += string.Format("TREA.in_time REPORT_TIME,\n");
                }
                else
                {
                    query += string.Format("SR.INTRUCTION_DATE REPORT_DATE,\n");
                    query += string.Format("sr.intruction_time REPORT_TIME,\n");

                }
                query += "SR.EXECUTE_LOGINNAME,\n";
                query += "SR.EXECUTE_USERNAME,\n";
                query += "SR.ICD_CODE,\n";
                query += "SR.ICD_NAME,\n";
                query += "SR.ICD_TEXT,\n";
                query += "SR.START_TIME,\n";
                query += "SR.TDL_PATIENT_CODE PATIENT_CODE,\n";
                query += "SR.TDL_PATIENT_NAME PATIENT_NAME,\n";
                query += "SR.TDL_PATIENT_NATIONAL_NAME,\n";
                query += "SR.TDL_PATIENT_DOB,\n";
                query += "TREA.TDL_TREATMENT_TYPE_ID,\n";
                query += "TREA.TDL_PATIENT_TYPE_ID,\n";
                query += "TREA.IN_TIME,\n";
                query += "TREA.OUT_TIME,\n";
                query += "TREA.FEE_LOCK_TIME,\n";
                query += "TREA.tdl_hein_card_number,\n";
                query += "TREA.TDL_PATIENT_ADDRESS,\n";
                query += "TREA.TDL_PATIENT_CAREER_NAME,\n";
                query += "TREA.trea_icd_code,\n";
                query += "TREA.trea_icd_name,\n";
                query += "TREA.last_department_code,\n";
                query += "TREA.last_department_name,\n";
                query += "fdp.first_department_code,\n";
                query += "fdp.first_department_name,\n";
                query += "TREA.treatment_end_type_code,\n";
                query += "TREA.treatment_end_type_name,\n";
                query += "TREA.treatment_result_code,\n";
                query += "TREA.treatment_result_name,\n";
                query += "TREA.treatment_day_count,\n";
                query += "TST.TEST_SAMPLE_TYPE_NAME,\n";
                query += "trea.branch_id,\n";
                query += "trea.tdl_hein_medi_org_code trea_hein_medi_org_code,\n";
                //query += "br.ACCEPT_HEIN_MEDI_ORG_CODE,\n";
                query += "Tran.Einvoice_num_order,\n";
                query += "Tran.num_order invoice_num_order,\n";
                query += "nvl(SSB.price,0) total_bill_price,\n";
                //query += "br.HEIN_PROVINCE_CODE BRANCH_HEIN_PROVINCE_CODE,\n";
                query += "SR.TDL_PATIENT_GENDER_NAME,\n";
                query += "SR.TDL_PATIENT_WORK_PLACE_NAME WORK_PLACE_NAME,\n";
                query += "SR.TDL_PATIENT_PHONE,\n";
                query += "SR.TDL_PATIENT_ADDRESS ADDRESS\n";
                query += "FROM HIS_SERE_SERV SS\n";
                query += "join HIS_SERVICE_REQ SR on sr.id=ss.service_req_id\n";
                query += "LEFT join HIS_TEST_SAMPLE_TYPE TST ON TST.ID = SR.TEST_SAMPLE_TYPE_ID\n";
                //query += "join HIS_SERVICE SV on SV.id=ss.service_id\n";
                query += @"join lateral (select 
trea.id,
TREA.TDL_TREATMENT_TYPE_ID,
TREA.TDL_PATIENT_TYPE_ID,
TREA.IN_TIME,
TREA.OUT_TIME,
TREA.FEE_LOCK_TIME,
trea.tdl_hein_card_number,
trea.TDL_PATIENT_ADDRESS,
trea.TDL_PATIENT_CAREER_NAME,
trea.icd_code trea_icd_code,
trea.icd_name trea_icd_name,
dp.department_code last_department_code,
dp.department_name last_department_name,
tet.treatment_end_type_code,
tet.treatment_end_type_name,
tr.treatment_result_code,
tr.treatment_result_name,
trea.treatment_day_count,
trea.branch_id 
from HIS_TREATMENT TREA 
join his_department dp on dp.id=trea.last_department_id 
left join his_treatment_end_type tet on tet.id=trea.treatment_end_type_id
left join his_treatment_result tr on tr.id=trea.treatment_result_id
where trea.id=sr.treatment_id) trea on trea.id=sr.treatment_id
left join lateral
(
select
min(fdp.department_code) keep(DENSE_RANK first order by dpt.id) first_department_code,
min(fdp.department_name) keep(DENSE_RANK first order by dpt.id) first_department_name
from his_department_tran dpt
join his_department fdp on fdp.id=dpt.department_id
where dpt.treatment_id=ss.tdl_treatment_id) fdp on 1=1
";
                //query += "left join his_branch br on br.id=trea.branch_id\n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID and ssb.is_cancel is null \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID AND TRAN.IS_CANCEL IS NULL \n";
                query += "WHERE SS.IS_DELETE = 0\n";
                query += "AND SR.IS_DELETE = 0\n";
                query += "AND SS.IS_NO_EXECUTE IS NULL\n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL\n";

                //hao phí
                if (filter.INPUT_DATA_ID_EXPEND_TYPE == 1)
                {
                    query += string.Format("AND SS.IS_EXPEND = {0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if(filter.INPUT_DATA_ID_EXPEND_TYPE == 2)
                {
                    query += "AND SS.IS_EXPEND IS NULL\n";
                }
                //phòng thực hiện
                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0}\n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID in ({0})\n", string.Join(",",filter.EXECUTE_ROOM_IDs));
                }
                //khoa thực hiện
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0}\n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                //phòng chỉ định
                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID = {0}\n", filter.REQUEST_ROOM_ID);
                }
                if (filter.REQUEST_ROOM_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID in ({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = 1 AND SR.EXECUTE_ROOM_ID in ({0}) or SS.TDL_SERVICE_TYPE_ID <> 1 AND SR.REQUEST_ROOM_ID in ({0}))\n", string.Join(",", filter.EXAM_ROOM_IDs));
                }
                //khoa chỉ định
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }

                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0}\n", filter.REQUEST_DEPARTMENT_ID);
                }
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0}\n", filter.DEPARTMENT_ID);
                }

                //đối tượng thanh toán
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0}\n", filter.PATIENT_TYPE_ID);
                }

                //đối tượng bệnh nhân
                if (filter.TDL_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0}\n", filter.TDL_PATIENT_TYPE_ID);
                }

                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} and tran.is_cancel is null\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);

                }
                //loại dịch vụ
                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0}\n", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0})\n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }
                //dịch vụ
                if (filter.SERVICE_ID != null)
                {
                    query += string.Format("AND SS.SERVICE_ID = {0}\n", filter.SERVICE_ID);
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n", string.Join(",", filter.SERVICE_IDs));
                }
                if (!string.IsNullOrEmpty(filter.SERVICE_NAME))
                {
                    query += string.Format("AND lower(SS.TDL_SERVICE_NAME) like '%{0}%'\n", filter.SERVICE_NAME.ToLower());
                }
                //nhóm dịch vụ
                if (filter.EXACT_PARENT_SERVICE_ID != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN (SELECT ID FROM HIS_SERVICE WHERE PARENT_ID= {0})\n", filter.EXACT_PARENT_SERVICE_ID);
                }
                if (filter.EXACT_PARENT_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN (SELECT ID FROM HIS_SERVICE WHERE PARENT_ID IN ({0}))\n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
                }

                //diện điều trị
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0}\n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0})", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                //phòng thu ngân
                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID = {0}\n", filter.EXACT_CASHIER_ROOM_ID);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID in ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }

                //nhân viên thu ngân
                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME = '{0}'\n", filter.CASHIER_LOGINNAME);
                }
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME in ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }


                //trạng thái y lệnh
                if (filter.SERVICE_REQ_STT_ID != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", filter.SERVICE_REQ_STT_ID);
                }
                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }
                //if (filter.EXECUTE_LOGINNAME_DOCTORs != null)
                //{
                //    query += string.Format("AND SR.EXECUTE_LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAME_DOCTORs));
                //}

                //đối tượng bệnh nhân
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }

                //đối tượng thanh toán
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }

                //đối tượng chi tiết
                if (filter.PATIENT_CLASSIFY_ID != null)
                {
                    query += string.Format("AND SR.TDL_PATIENT_CLASSIFY_ID = {0}\n",  filter.PATIENT_CLASSIFY_ID);
                }
                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("AND SR.TDL_PATIENT_CLASSIFY_ID IN ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }
                //khoa điều trị
                if (filter.LAST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND trea.LAST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.LAST_DEPARTMENT_IDs));
                }
                //cơ sở
                if (filter.BRANCH_IDs != null)
                {
                    query += string.Format("AND trea.branch_id IN ({0})\n", string.Join(",", filter.BRANCH_IDs));
                }
                //nhóm báo cáo
                if (filter.REPORT_TYPE_CAT_IDs != null)
                {
                    query += string.Format("AND ss.service_id IN (select service_id from his_rs.his_service_rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00290RDO>(query);
               
                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
        public List<ACS_USER> GetAcsUser(Mrs00290Filter filter)
        {
            List<ACS_USER> result = new List<ACS_USER>();
            try
            {

                string query = " --danh sach nguoi dung from QCS\n";
                query += "SELECT\n";
                query += "AU.*\n";
              
                query += "FROM ACS_RS.ACS_USER AU\n";
               
                query += "JOIN HIS_RS.HIS_EMPLOYEE EMP ON EMP.LOGINNAME = AU.LOGINNAME\n";
                query += "WHERE 1=1 and EMP.IS_DOCTOR=1\n";
               
                //if (filter.EXECUTE_LOGINNAME_DOCTORs != null)
                //{
                //    query += string.Format("AND AU.LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAME_DOCTORs));
                //}
                //if (filter.REQUEST_LOGINNAME_DOCTORs != null)
                //{
                //    query += string.Format("AND AU.LOGINNAME IN ('{0}')\n", string.Join("','", filter.REQUEST_LOGINNAME_DOCTORs));
                //}

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new  MOS.DAO.Sql.MyAppContext().GetSql<ACS_USER>(query);

                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_ICD> GetIcd(Mrs00290Filter filter)
        {
            List<HIS_ICD> result = null;
            try
            {
                string query = "select * from his_icd \n";
                if (filter.ICD_IDs != null)
                {
                    query += string.Format("where id in ({0}) \n", string.Join(",", filter.ICD_IDs));
                }
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_ICD>(query);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                return null;
            }
            return result;
        }
    }
}
