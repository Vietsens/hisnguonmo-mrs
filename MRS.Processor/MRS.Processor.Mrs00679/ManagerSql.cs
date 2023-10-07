using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00679
{
    public class ManagerSql
    {
        public List<Mrs00679RDO> GetSum(Mrs00679Filter filter)
        {
            List<Mrs00679RDO> result = null;
            try
            {
                string query = "--Thong ke benh nhan su dung dich vu\n";
                query += "select \n";
                query += "trea.TREATMENT_CODE, \n";
                query += "trea.TDL_PATIENT_CODE, \n";
                query += "trea.TDL_PATIENT_NAME, \n";
                query += "trea.TDL_PATIENT_DOB, \n";
                query += "trea.IN_TIME, \n";
                query += "trea.OUT_TIME, \n";
                query += "trea.ICD_NAME, \n";
                query += "trea.ICD_CODE, \n";
                query += "trea.FEE_LOCK_TIME, \n";
                query += "trea.MEDI_ORG_CODE, \n";
                query += "trea.MEDI_ORG_NAME, \n";
                query += "trea.TDL_PATIENT_ADDRESS, \n";
                query += "ss.HEIN_CARD_NUMBER, \n";
                query += "ss.TDL_SERVICE_NAME, \n";
                query += "ss.TDL_INTRUCTION_TIME, \n";
                query += "ss.TDL_REQUEST_USERNAME, \n";
                query += "ss.VIR_PRICE, \n";
                query += "dp.department_name,\n";
                query += "room.room_name,\n";
                query += "sr.SERVICE_REQ_CODE,\n";
                query += "sr.TDL_PATIENT_GENDER_NAME,\n";
                query += "sr.ICD_TEXT,\n";
                query += "sr.ICD_SUB_CODE,\n";
                query += "sr.EXECUTE_USERNAME,\n";
                query += "ST.SERVICE_TYPE_NAME,\n";
                query += "room1.room_name AS RQ_ROOM,\n";
                if(filter.IS_MERGE_TREA_SERV != true)
                {
                    query += "ss.id, \n";
                }

                query += "SUM(SS.VIR_TOTAL_PATIENT_PRICE_BHYT) BN_CUNG_CHI_TRA, \n";
                query += "sum(nvl(SS.OTHER_SOURCE_PRICE,0) * SS.AMOUNT) AS NGUON_KHAC, \n";
                query += "sum(SS.VIR_TOTAL_HEIN_PRICE) BH_CHITRA, \n";                            
                query += "sum(case when trea.tdl_treatment_type_id=3 then ss.AMOUNT else 0 end) as AMOUNT_NOITRU, \n";
                query += "sum(case when trea.tdl_treatment_type_id in(1,2) then ss.AMOUNT else 0 end) as AMOUNT_NGOAITRU, \n";
                query += "max(case when TRAN.ID IS NULL then '' else 'X' end) as SERVICE_STATUS, \n";
                query += "sum(ss.VIR_TOTAL_PRICE) VIR_TOTAL_PRICE, \n";
                query += "sum(case when ss.is_expend=1 then ss.AMOUNT else 0 end) as AMOUNT_EXPEND, \n";
                query += "sum(case when ss.is_expend=1 then ss.VIR_TOTAL_PRICE_NO_EXPEND else 0 end) as VIR_TOTAL_PRICE_EXPEND \n";
                query += "from \n";
                query += "his_rs.his_sere_serv ss \n";
                query += "join his_rs.his_treatment trea on (trea.id = ss.tdl_treatment_id ) \n";
                query += "join his_rs.his_service_req sr on sr.id = ss.service_req_id \n";
                query += "join his_rs.his_department dp on sr.request_department_id=dp.id\n ";
                query += "join his_rs.v_his_room room on sr.execute_room_id=room.id \n";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID AND ssb.IS_CANCEL IS NULL   \n";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID AND TRAN.IS_CANCEL IS NULL \n";
                query += "JOIN his_rs.v_his_room  room1 ON (room1.id = ss.TDL_REQUEST_ROOM_ID) \n";
                query += "JOIN HIS_RS.HIS_SERVICE_TYPE ST ON ST.ID = SS.TDL_SERVICE_TYPE_ID \n";
                query += "where ss.is_delete =0 and ss.is_no_execute is null \n";


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
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);

                }
                if (filter.BRANCH_ID != null)
                {
                    query += string.Format("and trea.branch_id = {0}", filter.BRANCH_ID);
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("and ss.SERVICE_ID in({0}) \n", string.Join(",", filter.SERVICE_IDs));
                }
                if (filter.END_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and trea.END_DEPARTMENT_ID in({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
                }
                if (filter.END_ROOM_IDs != null)
                {
                    query += string.Format("and trea.END_ROOM_ID in({0}) \n", string.Join(",", filter.END_ROOM_IDs));
                }
                
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.REQUEST_DEPARTMENT_ID in({0}) \n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("and ss.PATIENT_TYPE_ID = {0} \n", filter.PATIENT_TYPE_ID);
                }
                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("and sr.service_req_stt_id in({0}) \n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }
                if (filter.SERVICE_REQ_TYPE_IDs != null)
                {
                    query += string.Format("and sr.service_req_type_id in({0}) \n", string.Join(",", filter.SERVICE_REQ_TYPE_IDs));
                }
                query += "group by \n";
                query += "trea.TREATMENT_CODE, \n";
                query += "trea.TDL_PATIENT_CODE, \n";
                query += "trea.TDL_PATIENT_NAME, \n";
                query += "trea.TDL_PATIENT_DOB, \n";
                query += "trea.IN_TIME, \n";
                query += "trea.OUT_TIME, \n";
                query += "trea.ICD_NAME, \n";
                query += "trea.ICD_CODE, \n";
                query += "trea.FEE_LOCK_TIME, \n";
                query += "trea.MEDI_ORG_CODE, \n";
                query += "trea.MEDI_ORG_NAME, \n";
                query += "trea.TDL_PATIENT_ADDRESS, \n";
                query += "ss.HEIN_CARD_NUMBER, \n";
                query += "ss.TDL_SERVICE_NAME, \n";
                query += "ss.TDL_INTRUCTION_TIME, \n";
                query += "ss.TDL_REQUEST_USERNAME, \n";
                query += "ss.VIR_PRICE, \n";
                query += "dp.department_name,\n";

                query += "sr.SERVICE_REQ_CODE,\n";
                query += "sr.TDL_PATIENT_GENDER_NAME,\n";
                query += "sr.ICD_TEXT,\n";
                query += "sr.ICD_SUB_CODE,\n";
                query += "sr.EXECUTE_USERNAME,\n";
                query += "room1.room_name,\n";
                query += "SERVICE_TYPE_NAME,\n";
                if (filter.IS_MERGE_TREA_SERV != true)
                {
                    query += "ss.id, \n";
                }
                query += "room.room_name\n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00679RDO>(query);
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
