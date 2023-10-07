using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00714
{
    class ManagerSql
    {
        //    enum TimeChoose
        //    {
        //        Intruction,
        //        Begin,
        //        End
        //    }
        //    TimeChoose chooseTime;

        internal static List<EKIP_USER_CFG> GetEkipUserCfg()
        {
            List<EKIP_USER_CFG> result = new List<EKIP_USER_CFG>();
            try
            {
                string queryBase = "SELECT\n SRDE.*,\n SURE.SERVICE_TYPE_ID,\n SURE.PTTT_GROUP_ID,\n SURE.EMOTIONLESS_METHOD_ID\n  FROM HIS_SURG_REMU_DETAIL SRDE\n JOIN HIS_SURG_REMUNERATION SURE ON SRDE.SURG_REMUNERATION_ID = SURE.ID \n";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + queryBase);
                var sample = new MOS.DAO.Sql.SqlDAO().GetSql<EKIP_USER_CFG>(queryBase);
                if (sample != null && sample.Count > 0)
                {
                    result.AddRange(sample);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        internal List<Mrs00714RDO> GetRdo(Mrs00714Filter filter)
        {
            List<Mrs00714RDO> result = new List<Mrs00714RDO>();
            try
            {
                string query = "";
                query += "select \n";
                query += "ss.tdl_treatment_id, \n";
                query += "ss.id, \n";
                query += "ss.ekip_id, \n";
                query += "ss.tdl_intruction_time, \n";
                query += "sr.request_department_id, \n";
                query += "sr.request_room_id, \n";
                query += "sr.execute_department_id, \n";
                query += "sr.execute_room_id, \n";
                query += "trea.treatment_code, \n";
                query += "ss.service_req_id, \n";
                query += "sr.service_req_code, \n";
                query += "pt.vir_patient_name, \n";
                query += "trea.tdl_patient_gender_name gender_name, \n";
                query += "pt.vir_address address, \n";
                query += "ss.patient_type_id, \n";
                query += "ss.hein_card_number, \n";
                query += "sv.service_type_id, \n";//loại dịch vụ
                query += "sv.service_type_code, \n";//loại dịch vụ
                query += "sv.service_type_name, \n";//loại dịch vụ
                query += "sv.pttt_group_id sv_pttt_group_id, \n";
                query += "sv.service_code, \n";
                query += "sv.id service_id, \n";
                query += "sv.service_name, \n";
                query += "ss.parent_id, \n";
                query += "ss.amount, \n";
                query += "nvl(ss.vir_price,0) vir_price, \n";
                query += "nvl(ss.vir_total_price,0) vir_total_price, \n";
                query += "ssp.before_pttt_icd_name, \n";
                query += "ssp.after_pttt_icd_name, \n";
                query += "ssp.pttt_method_id, \n";
                query += "sv.pttt_group_id, \n";
                query += "ssp.death_within_id, \n";
                query += "ssp.emotionless_method_id, \n";
                query += "ssp.emotionless_method_second_id, \n";
                query += "ssp.pttt_catastrophe_id, \n";
                query += "ssp.pttt_condition_id, \n";
                query += "ssp.real_pttt_method_id, \n";
                query += "sr.start_time l_start_time, \n";
                query += "pt.patient_code, \n";
                query += "ss.original_price, \n";
                query += "ss.hein_limit_price, \n";
                query += "ss.price, \n";
                query += "ss.vat_ratio, \n";
                query += "pt.dob, \n";
                query += "trea.in_code, \n";
                query += "trea.icd_name, \n";
                query += "trea.tdl_treatment_type_id, \n";
                query += "trea.tdl_patient_type_id, \n";
                query += "sse.begin_time l_begin_time, \n";
                query += "sse.end_time l_end_time, \n";
                query += "sr.execute_loginname, \n";
                query += "sr.execute_username, \n";
                query += "ssp.manner, \n";
                query += "ss.tdl_intruction_time - mod(ss.tdl_intruction_time, 100000000) intruction_month \n";
                query += "from his_sere_serv ss \n";
                query += "left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id \n";
                query += "left join his_sere_serv_pttt ssp on ssp.sere_serv_id=ss.id \n";
                query += "join his_service_req sr on sr.id = ss.service_req_id \n";
                query += "join v_his_service sv on sv.id = ss.service_id \n";
                query += "join his_treatment trea on ss.tdl_treatment_id=trea.id \n";
                query += "join his_patient pt on pt.id=ss.tdl_patient_id \n";
                query += "left join his_pttt_group ptgr on ptgr.id=sv.pttt_group_id \n";
                query += "where 1=1 \n";
                query += "and ss.is_delete =0 \n";
                query += "and ss.is_no_execute is null \n";

                //chon thoi gian
                //chooseTime = TimeChoose.Intruction;
                if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("and trea.out_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("and sse.begin_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("and sse.end_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("and sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 1)
                {
                    query += "AND EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 2)
                {
                    query += "AND NOT EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }

                if (filter.INPUT_DATA_ID_STT_PAUSE == 1)
                {
                    query += "AND trea.IS_PAUSE=1 \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAUSE == 2)
                {
                    query += "AND trea.IS_PAUSE IS NULL \n";
                }
                //bỏ phẫu thuật thẩm mỹ
                if (!string.IsNullOrWhiteSpace(filter.PATIENT_CLASSIFY_CODE__TMs))
                {
                    query += string.Format("and (trea.TDL_PATIENT_CLASSIFY_ID is null or trea.TDL_PATIENT_CLASSIFY_ID not in (select id from his_patient_classify where patient_classify_code in ('{0}') ) ) \n", (filter.PATIENT_CLASSIFY_CODE__TMs ?? "").Replace(",", "','"));
                }
                //chon loai dich vu

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                else if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                        {
                            query += string.Format("OR ptgr.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                        {
                            query += string.Format("OR ptgr.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }

                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.requset_department_id in ({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n ", string.Join(",", filter.SERVICE_IDs));
                }

                if (filter.EXCLUDE_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID  NOT IN ({0})\n ", string.Join(",", filter.EXCLUDE_SERVICE_IDs));
                }

                if (filter.IS_GATHER_DATA==true)
                {
                    query += string.Format("AND SSE.IS_GATHER_DATA = {0}\n ", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00714RDO>(query);
                if (rs != null && rs.Count > 0)
                {
                    result.AddRange(rs);
                    result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        internal List<USER_COUNT_PTTT> GetUserPttt(Mrs00714Filter filter)
        {
            //lay du lieu tuong tu listRdo, tach ra theo nguoi thuc hien, vai tro va khoa cua nguoi thuc hien

            List<USER_COUNT_PTTT> result = new List<USER_COUNT_PTTT>();
            try
            {
                string query = "";
                query += "select \n";
                query += "eu.EXECUTE_ROLE_CODE, \n";//mã vai trò
                query += "eu.EXECUTE_ROLE_NAME, \n";//tên vai trò
                query += "eu.LOGINNAME, \n";//mã user
                query += "eu.username USER_NAME, \n";//tên user
                query += "ss.id SERE_SERV_ID, \n";
                query += "sr.request_department_id, \n";
                query += "sr.request_room_id, \n";
                query += "sr.execute_department_id, \n";
                query += "sr.execute_room_id, \n";
                query += "sv.service_type_id, \n";//loại dịch vụ
                query += "sv.service_type_code, \n";//loại dịch vụ
                query += "sv.service_type_name, \n";//loại dịch vụ
                query += "sv.pttt_group_id sv_pttt_group_id, \n";
                query += "sv.pttt_group_id, \n";
                query += "ptgr.pttt_group_code, \n";
                query += "ptgr.pttt_group_name, \n";
                query += "ss.tdl_intruction_time - mod(ss.tdl_intruction_time, 100000000) intruction_month \n";
                query += "from his_sere_serv ss \n";
                query += "left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id \n";
                query += "left join his_sere_serv_pttt ssp on ssp.sere_serv_id=ss.id \n";
                query += "join his_service_req sr on sr.id = ss.service_req_id \n";
                query += "join v_his_service sv on sv.id = ss.service_id \n";
                query += "join his_treatment trea on ss.tdl_treatment_id=trea.id \n";
                query += "join v_his_ekip_user eu on eu.ekip_id=ss.ekip_id \n";
                query += "join his_pttt_group ptgr on ptgr.id=sv.pttt_group_id \n";
                query += "where 1=1 \n";
                query += "and ss.is_delete =0 \n";
                query += "and ss.is_no_execute is null \n";
                //chon thoi gian
                //chooseTime = TimeChoose.Intruction;
                if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("and trea.out_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("and sse.begin_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("and sse.end_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("and sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 1)
                {
                    query += "AND EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 2)
                {
                    query += "AND NOT EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }

                if (filter.INPUT_DATA_ID_STT_PAUSE == 1)
                {
                    query += "AND trea.IS_PAUSE=1 \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAUSE == 2)
                {
                    query += "AND trea.IS_PAUSE IS NULL \n";
                }
                //bỏ phẫu thuật thẩm mỹ
                if (!string.IsNullOrWhiteSpace(filter.PATIENT_CLASSIFY_CODE__TMs))
                {
                    query += string.Format("and (trea.TDL_PATIENT_CLASSIFY_ID is null or trea.TDL_PATIENT_CLASSIFY_ID not in (select id from his_patient_classify where patient_classify_code in ('{0}') ) ) \n", (filter.PATIENT_CLASSIFY_CODE__TMs ?? "").Replace(",", "','"));
                }
                //chon loai dich vu

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                else if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                        {
                            query += string.Format("OR ptgr.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                        {
                            query += string.Format("OR ptgr.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }

                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("and eu.department_id = {0}\n", filter.DEPARTMENT_ID);
                }

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and eu.department_id in ({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }

                if (filter.LOGINNAMEs != null)
                {
                    query += string.Format("and eu.LOGINNAME in ('{0}') \n", string.Join("','", filter.LOGINNAMEs));
                }
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.requset_department_id in ({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n ", string.Join(",", filter.SERVICE_IDs));
                }

                if (filter.EXCLUDE_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID  NOT IN ({0})\n ", string.Join(",", filter.EXCLUDE_SERVICE_IDs));
                }

                if (filter.IS_GATHER_DATA == true)
                {
                    query += string.Format("AND SSE.IS_GATHER_DATA = {0}\n ", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var r = new MOS.DAO.Sql.SqlDAO().GetSql<USER_COUNT_PTTT>(query);
                if (r != null && r.Count > 0)
                {
                    result.AddRange(r);
                    result = result.GroupBy(o => string.Format("{0}_{1}_{2}", o.SERE_SERV_ID, o.EXECUTE_ROLE_CODE, o.LOGINNAME)).Select(p => p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
        internal List<SS_USER_REMUNERATION> GetRemuneration(Mrs00714Filter filter)
        {
            //lay du lieu tuong tu listRdo, tach ra theo nguoi thuc hien, vai tro va khoa cua nguoi thuc hien

            List<SS_USER_REMUNERATION> result = new List<SS_USER_REMUNERATION>();
            try
            {
                string query = "";
                query += "select \n";
                query += "eu.execute_role_code, \n";//mã vai trò
                query += "eu.execute_role_name, \n";//tên vai trò
                query += "eu.loginname, \n";//mã user
                query += "eu.username user_name, \n";//tên user

                query += "nvl(srd.price,0) remuneration_price, \n";//tiền phụ cấp
                query += "ss.id SERE_SERV_ID, \n";
                query += "ss.amount, \n";
                query += "sr.request_department_id, \n";
                query += "sr.request_room_id, \n";
                query += "sr.execute_department_id, \n";
                query += "sr.execute_room_id, \n";
                query += "sv.service_type_id, \n";//loại dịch vụ
                query += "sv.service_type_code, \n";//loại dịch vụ
                query += "sv.service_type_name, \n";//loại dịch vụ
                query += "sv.pttt_group_id sv_pttt_group_id, \n";
                query += "sv.pttt_group_id, \n";
                query += "ss.tdl_intruction_time - mod(ss.tdl_intruction_time, 100000000) intruction_month \n";
                query += "from his_sere_serv ss \n";
                query += "left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id \n";
                query += "left join his_sere_serv_pttt ssp on ssp.sere_serv_id=ss.id \n";
                query += "join his_service_req sr on sr.id = ss.service_req_id \n";
                query += "join v_his_service sv on sv.id = ss.service_id \n";
                query += "join his_treatment trea on ss.tdl_treatment_id=trea.id \n";
                query += "join his_surg_remuneration srm on (srm.pttt_group_id=sv.pttt_group_id and srm.service_type_id = ss.tdl_service_type_id and (srm.emotionless_method_id is null or srm.emotionless_method_id = ssp.emotionless_method_id or srm.emotionless_method_id = ssp.emotionless_method_second_id)) \n";
                query += "join v_his_ekip_user eu on eu.ekip_id=ss.ekip_id \n";
                query += "join his_surg_remu_detail srd ON srd.execute_role_id=eu.execute_role_id and srd.surg_remuneration_id=srm.id \n";
                query += "left join his_pttt_group ptgr on ptgr.id=sv.pttt_group_id \n";
                query += "where 1=1 \n";
                query += "and ss.is_delete =0 \n";
                query += "and ss.is_no_execute is null \n";

                //chon thoi gian
                //chooseTime = TimeChoose.Intruction;
                if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("and trea.out_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("and sse.begin_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("and sse.end_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("and sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }

                if (filter.INPUT_DATA_ID_STT_PAUSE == 1)
                {
                    query += "AND trea.IS_PAUSE=1 \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAUSE == 2)
                {
                    query += "AND trea.IS_PAUSE IS NULL \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 1)
                {
                    query += "AND EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                if (filter.INPUT_DATA_ID_STT_PAY == 2)
                {
                    query += "AND NOT EXISTS (select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null) \n";
                }
                //bỏ phẫu thuật thẩm mỹ
                if (!string.IsNullOrWhiteSpace(filter.PATIENT_CLASSIFY_CODE__TMs))
                {
                    query += string.Format("and (trea.TDL_PATIENT_CLASSIFY_ID is null or trea.TDL_PATIENT_CLASSIFY_ID not in (select id from his_patient_classify where patient_classify_code in ('{0}') ) ) \n", (filter.PATIENT_CLASSIFY_CODE__TMs ?? "").Replace(",", "','"));
                }
                //chon loai dich vu

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                else if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 1)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                        {
                            query += string.Format("OR ptgr.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                    else if (filter.IS_PT_TT.Value == 0)
                    {
                        query += string.Format("AND (SS.TDL_SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                        if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                        {
                            query += string.Format("OR ptgr.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));

                        }
                        if (filter.REPORT_TYPE_CAT_IDs != null)
                        {
                            query += string.Format("OR ss.service_id in (select service_id from his_rs.his_service_Rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                        }
                        query += string.Format(") \n");
                    }
                }

                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }

                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("and eu.department_id = {0}\n", filter.DEPARTMENT_ID);
                }

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and eu.department_id in ({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }

                if (filter.LOGINNAMEs != null)
                {
                    query += string.Format("and eu.LOGINNAME in ('{0}') \n", string.Join("','", filter.LOGINNAMEs));
                }
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and ss.requset_department_id in ({0}) \n", string.Join(",", filter.REQ_DEPARTMENT_IDs));
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID IN ({0})\n ", string.Join(",", filter.SERVICE_IDs));
                }

                if (filter.EXCLUDE_SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID  NOT IN ({0})\n ", string.Join(",", filter.EXCLUDE_SERVICE_IDs));
                }

                if (filter.IS_GATHER_DATA == true)
                {
                    query += string.Format("AND SSE.IS_GATHER_DATA = {0}\n ", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (filter.IS_REMU_PRICE_WITH_EKIP == true)
                {
                    query = query.Replace("nvl(srd.price,0) remuneration_price, \n", "nvl(eu.remuneration_price,0) remuneration_price, \n")//tiền phụ cấp
                        .Replace("\njoin his_surg_remuneration", "\n--join his_surg_remuneration").Replace("\njoin his_surg_remu_detail", "\n--join his_surg_remu_detail");
                }
                if (filter.ADD_EKIP_NO_REMU == true)
                {

                    query = query.Replace("\njoin his_surg_remuneration", "\nleft join his_surg_remuneration").Replace("\njoin his_surg_remu_detail", "\nleft join his_surg_remu_detail");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<SS_USER_REMUNERATION>(query);
                if (rs != null && rs.Count > 0)
                {
                    result.AddRange(rs);
                    result = result.GroupBy(o => string.Format("{0}_{1}_{2}", o.SERE_SERV_ID, o.EXECUTE_ROLE_CODE, o.LOGINNAME)).Select(p => p.First()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }

    public class EKIP_USER_CFG : HIS_SURG_REMU_DETAIL
    {
        public long SERVICE_TYPE_ID { get; set; }
        public long PTTT_GROUP_ID { get; set; }
        public long? EMOTIONLESS_METHOD_ID { get; set; }
    }
}
