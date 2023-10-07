using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00099
{
    class ManagerSql
    {
        enum TimeChoose
        {
            Intruction,
            Finish,
            End
        }
        TimeChoose chooseTime;

        internal List<Mrs00099RDO> GetSereServ(Mrs00099Filter filter)
        {
            List<Mrs00099RDO> result = null;
            try
            {

                string query = "";
                query += "--iss39136 \n";
                query += "-- Bao cao ket qua cls \n";
                query += "select \n";
                query += "sr.service_req_code, \n";
                query += "trea.tdl_patient_name patient_name, \n";
                query += "sr.request_room_id, \n";
                query += "sr.execute_room_id, \n";
                query += "sr.execute_username, \n";
                query += "ss.tdl_service_code service_code, \n";
                query += "ss.tdl_service_name service_name, \n";
                query += "trea.tdl_patient_code patient_code, \n";
                query += "trea.patient_id, \n";
                query += "sr.intruction_time, \n";
                query += "sr.tdl_patient_gender_name gender_name, \n";
                query += "ss.hein_card_number, \n";
                query += "sr.tdl_patient_address vir_address, \n";
                query += "sr.tdl_patient_dob, \n";
                query += "sr.icd_name icd_diim, \n";
                query += "sr.icd_text, \n";
                query += "sv.number_of_film IS_SIZE_FILM_20_25, \n";
                query += "sse.conclude diim_result, \n";
                query += "sse.begin_time, \n";
                query += "sse.description, \n";
                query += "sse.end_time, \n";
                query += "sse.instruction_note, \n";
                query += "sse.number_of_film, \n";
                query += "sse.FILM_SIZE_ID, \n";
                query += "trea.ksk_order, \n";
                query += "trea.treatment_code, \n";
                query += "kc.ksk_contract_code, \n";
                query += "wp.work_place_name, \n";
                query += "ss.tdl_service_type_id, \n";
                query += "ss.amount, \n";
                query += "ss.vir_price price, \n";
                query += "svt.service_type_code, \n";
                query += "svt.service_type_name \n";
                query += "from his_treatment trea \n";
                query += "join his_service_req sr on sr.treatment_id=trea.id \n";
                query += "left join his_ksk_contract kc on kc.id=trea.tdl_ksk_contract_id \n";
                query += "left join his_work_place wp on wp.id=kc.work_place_id \n";
                query += "join his_sere_serv ss on ss.service_req_id=sr.id \n";
                query += "left join his_sere_serv_ext sse on sse.sere_serv_id=ss.id \n";
                query += "join his_service sv on ss.service_id=sv.id \n";
                query += "join his_service_type svt on ss.tdl_service_type_id=svt.id \n";
                query += "where 1=1 \n";
                query += "and ss.is_delete=0 \n";
                query += "and ss.is_no_execute is null \n";
                query += "and ss.is_expend is null \n";
                //them chon thoi gian
                chooseTime = TimeChoose.Intruction;
                if (filter.IS_END_TIME.HasValue)
                {
                    if (filter.IS_END_TIME.Value)
                    {
                        chooseTime = TimeChoose.End;
                    }
                    else
                    {
                        chooseTime = TimeChoose.Finish;
                    }
                }
                if (chooseTime == TimeChoose.Intruction)
                {
                    query += string.Format("and sr.intruction_time between {0} and {1} \n", filter.TIME_FROM??filter.FINISH_TIME_FROM??0, filter.TIME_TO??filter.FINISH_TIME_TO??0);
                }
                if (chooseTime == TimeChoose.Finish)
                {
                    query += string.Format("and (sr.finish_time between {0} and {1} and sr.service_req_stt_id ={2}) \n", filter.TIME_FROM??filter.FINISH_TIME_FROM??0, filter.TIME_TO??filter.FINISH_TIME_TO??0,IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (chooseTime == TimeChoose.End)
                {
                    query += string.Format("and sse.end_time between {0} and {1} \n", filter.TIME_FROM??filter.FINISH_TIME_FROM??0, filter.TIME_TO??filter.FINISH_TIME_TO??0);
                }
                //them chon loai cls
                if (filter.SERVICE_REQ_TYPE_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_REQ_TYPE_IDs));
                }
                else
                {
                    query += string.Format("AND SR.SERVICE_REQ_TYPE_ID in ({0}) \n", string.Join(",", new List<long>() { 
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA, 
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA
                    }));
                }

                if (filter.EXE_ROOM_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_ROOM_ID = {0} \n", filter.EXE_ROOM_ID);
                }

                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }

                if (filter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_ROOM_ID = {0} \n", filter.REQUEST_ROOM_ID);
                }

                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0} \n", filter.REQUEST_DEPARTMENT_ID);
                }
                if (filter.TREATMENT_TYPE_ID.HasValue)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID.Value);
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                //them doi tuong benh nhan
                if (filter.PATIENT_TYPE_ID.HasValue)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} \n", filter.PATIENT_TYPE_ID.Value);
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                //them hop dong kham suc khoe
                if (filter.KSK_CONTRACT_IDs != null)
                {
                    query += string.Format("AND KC.ID in ({0}) \n", string.Join(",", filter.KSK_CONTRACT_IDs));
                }
                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID in ({0}) \n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }
               

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00099RDO>(query);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
