using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00255
{
    class ManagerSql
    {
        enum TimeChoose
        {
            Intruction,
            Begin,
            End
        }
        TimeChoose chooseTime;

        internal static List<EKIP_USER_CFG> GetEkipUserCfg()
        {
            List<EKIP_USER_CFG> result = new List<EKIP_USER_CFG>();
            try
            {
                string queryBase = "SELECT SRDE.*, SURE.SERVICE_TYPE_ID, SURE.PTTT_GROUP_ID, SURE.EMOTIONLESS_METHOD_ID  FROM HIS_SURG_REMU_DETAIL SRDE JOIN HIS_SURG_REMUNERATION SURE ON SRDE.SURG_REMUNERATION_ID = SURE.ID \n";

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

        internal  List<Mrs00255RDO> GetRdo(Mrs00255Filter filter)
        {
            List<Mrs00255RDO> result = new List<Mrs00255RDO>();
            try
            {
                string query = "";
                query += "select \n";
                query += "ss.tdl_treatment_id, \n";
                query += "ss.id, \n";
                query += "ss.ekip_id, \n";
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
                query += "sv.service_type_id, \n";
                query += "svt.service_type_name, \n";
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
                query += "ssp.pttt_group_id, \n";
                query += "ptgr.pttt_group_name, \n";
                query += "ssp.death_within_id, \n";
                query += "dea.death_within_name, \n";
                query += "ssp.emotionless_method_id, \n";
                query += "ssp.emotionless_method_second_id, \n";
                query += "ssp.pttt_catastrophe_id, \n";
                query += "cata.pttt_catastrophe_name, \n";
                query += "ssp.pttt_condition_id, \n";
                query += "ssp.real_pttt_method_id, \n";
                query += "sr.start_time l_start_time, \n";
                query += "sr.finish_time l_finish_time, \n";
                query += "ssp.PTTT_PRIORITY_ID, \n";
                query += "pri.PTTT_PRIORITY_NAME, \n";
                query += "pt.patient_code, \n";
                query += "ss.primary_price, \n";
                query += "ss.original_price, \n";
                query += "ss.hein_limit_price, \n";
                query += "ss.price, \n";
                query += "ss.vat_ratio, \n";
                query += "pt.dob, \n";
                query += "trea.in_code, \n";
                query += "trea.tdl_treatment_type_id, \n";
                query += "trea.tdl_patient_type_id, \n";
                query += "sse.begin_time l_begin_time, \n";
                query += "sse.end_time l_end_time, \n";
                query += "trea.in_time l_in, \n";
                query += "trea.out_time l_out, \n";
                query += "sr.intruction_time l_intruction \n";
                query += "from his_sere_serv ss \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += "JOIN HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                    query += "JOIN HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
                }
                query += "left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id \n";
                query += "left join his_sere_serv_pttt ssp on ssp.sere_serv_id=ss.id \n";
                query += "join his_service_req sr on sr.id = ss.service_req_id \n";
                query += "join his_service sv on sv.id = ss.service_id \n";
                query += "join his_treatment trea on ss.tdl_treatment_id=trea.id \n";
                query += "join his_patient pt on pt.id=ss.tdl_patient_id \n";
                query += "left join his_pttt_group ptgr on ptgr.ID = ssp.pttt_group_id \n";
                query += "left join his_death_within dea on dea.ID = ssp.death_within_id \n";
                query += "left join HIS_PTTT_CATASTROPHE cata on cata.ID = ssp.PTTT_CATASTROPHE_ID \n";
                query += "left join HIS_PTTT_PRIORITY pri on pri.ID = ssp.PTTT_PRIORITY_ID \n";
                query += "join his_service_type svt on svt.id = ss.tdl_service_type_id \n";
                query += "where 1=1 \n";
                query += "and ss.is_delete =0 \n";
                query += "and ss.is_no_execute is null \n";

                //chon thoi gian
                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SSE.BEGIN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM , filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM , filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM , filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM , filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM , filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM , filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM , filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM , filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM , filter.TIME_TO);
                }
                else
                {
                    chooseTime = TimeChoose.Intruction;
                    if (filter.IS_END_TIME.HasValue)
                    {
                        if (filter.IS_END_TIME.Value)
                        {
                            chooseTime = TimeChoose.End;
                        }
                        else
                        {
                            chooseTime = TimeChoose.Begin;
                        }
                    }
                    if (chooseTime == TimeChoose.Intruction)
                    {
                        query += string.Format("and sr.intruction_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO); //thời gian chỉ định
                    }
                    if (chooseTime == TimeChoose.Begin)
                    {
                        query += string.Format("and sse.begin_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                    }
                    if (chooseTime == TimeChoose.End)
                    {
                        query += string.Format("and sse.end_time between {0} and {1} \n", filter.TIME_FROM, filter.TIME_TO);
                    }
                }
                //chon loai dich vu
                List<long> serviceTypeIds = new List<long>();

                if (filter.IS_PT_TT.HasValue)
                {
                    if (filter.IS_PT_TT.Value == 0)
                    {
                        serviceTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                    }
                    else if (filter.IS_PT_TT.Value == 1)
                    {
                        serviceTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                    }
                }
                else
                {
                    serviceTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                    serviceTypeIds.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                }

                query += string.Format("and ss.tdl_service_type_id in ({0}) \n", string.Join(",", serviceTypeIds));

                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }

                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.request_department_id in ({0}) \n", string.Join(",", filter.DEPARTMENT_IDs));
                }

                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("and sr.request_department_id={0} \n", filter.DEPARTMENT_ID);
                }
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("and trea.tdl_treatment_type_id={0} \n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and trea.tdl_patient_type_id={0} \n", filter.PATIENT_TYPE_ID);
                }
                if (filter.TDL_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and ss.patient_type_id = {0} \n", filter.TDL_PATIENT_TYPE_ID);
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and ss.patient_type_id in ({0}) \n", string.Join(",",filter.TDL_PATIENT_TYPE_IDs));
                }
                if (filter.IS_PAUSE.HasValue && filter.IS_PAUSE.Value)
                {
                    query += string.Format("and trea.is_pause ={0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                if (filter.IS_LOCK_FEE.HasValue && filter.IS_LOCK_FEE.Value)
                {
                    query += string.Format("and trea.is_active ={0} \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }

                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("and sr.service_req_stt_id in ({0}) \n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }
                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("and sv.service_type_id={0} \n", filter.SERVICE_TYPE_ID);
                }
                if (filter.SERVICE_ID != null)
                {
                    query += string.Format("and sv.id={0} \n", filter.SERVICE_ID);
                }

                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("and sv.id in ({0}) \n", string.Join(",", filter.SERVICE_IDs));
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("and sv.service_type_id in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00255RDO>(query);
                if (rs != null && rs.Count > 0)
                {
                    result.AddRange(rs);
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
