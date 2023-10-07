
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

namespace MRS.Processor.Mrs00040
{
    public class ManagerSql
    {
        public List<Mrs00040RDO> GetSereServ(Mrs00040Filter filter)
        {
            try
            {
                List<Mrs00040RDO> result = new List<Mrs00040RDO>();
                string query = "--dich vu thu thuat\n";
                query += "SELECT \n";
                query += "trea.tdl_hein_card_number, \n";
                query += "SS.*\n, ";
                query += "sr.TDL_PATIENT_NAME PATIENT_NAME, \n";
                query += "sr.TDL_PATIENT_FIRST_NAME, \n";
                query += "sr.TDL_PATIENT_CODE PATIENT_CODE, \n";
                query += "sr.TDL_PATIENT_ADDRESS VIR_ADDRESS, \n";
                query += "SR.START_TIME, \n";
                query += "SR.FINISH_TIME TDL_FINISH_TIME, \n";
                query += "sr.TDL_PATIENT_DOB, \n";
                query += "PG.PTTT_GROUP_NAME, \n";
                query += "SR.TDL_PATIENT_GENDER_ID, \n";
                query += "trea.IN_TIME, \n";
                query += "trea.OUT_TIME, \n";
                query += "trea.TDL_PATIENT_TYPE_ID, \n";
                query += "pt.patient_type_name PATIENT_TYPE_NAME_TT, \n";
                query += "sr.ICD_NAME SERVICE_REQ_ICD_NAME, \n";
                query += "sr.ICD_CODE SERVICE_REQ_ICD_CODE, \n";
                query += "sr.ICD_TEXT ICD_SUB_NAME, \n";
                query += "sr.ICD_SUB_CODE, \n";
                query += "trea.ICD_NAME TREA_ICD_NAME, \n";
                query += "trea.ICD_CODE TREA_ICD_CODE, \n";
                query += "trea.ICD_TEXT TREA_SUB_NAME, \n";
                query += "trea.ICD_SUB_CODE TREA_SUB_CODE, \n";
                //query += "TREA.ICD_TEXT, ";
                query += "SR.EXECUTE_LOGINNAME,\n ";
                query += "SR.EXECUTE_USERNAME, \n";
                query += "SSE.BEGIN_TIME \n";
                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {

                    query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                    query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {

                    query += "JOIN HIS_RS.HIS_SERE_SERV_EXT SSE ON SSE.SERE_SERV_ID=SS.ID  \n";
                }
                else
                {
                    query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_EXT SSE ON SSE.SERE_SERV_ID=SS.ID  \n";
                }
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "JOIN lateral (select trea.id from his_treatment trea where trea.id=sr.treatment_id) TREA ON sr.TREATMENT_ID=TREA.ID  \n";
                query += "JOIN HIS_RS.HIS_SERVICE SV ON SS.SERVICE_ID=SV.ID  \n";
                query += "JOIN HIS_RS.HIS_PATIENT_TYPE PT ON SS.PATIENT_TYPE_ID = PT.ID \n";
                query += "left JOIN HIS_RS.HIS_PTTT_GROUP PG ON SV.PTTT_GROUP_ID=PG.ID \n";
               
                query += "WHERE 1=1 \n";

                query += "AND SS.IS_NO_EXECUTE IS NULL\n";
                query += "AND SS.IS_EXPEND IS NULL\n";
                query += "and sr.is_delete =0 \n";
                query += "and ss.is_delete =0 \n";
                //query += string.Format("AND (SV.SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);

               if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND (SV.SERVICE_TYPE_ID in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR sv.id in (select service_id from his_rs.his_service_rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }

                    query += string.Format(") \n");
                }
                else
                {
                    query += string.Format("AND (SV.SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT);
                    if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__TTs))
                    {
                        query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__TTs.Replace(",", "','"));
                    }
                    if (filter.REPORT_TYPE_CAT_IDs != null)
                    {
                        query += string.Format("OR sv.id in (select service_id from his_rs.his_service_rety_cat where report_type_cat_id in ({0}))\n", string.Join(",", filter.REPORT_TYPE_CAT_IDs));
                    }
                    query += string.Format(") \n");
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("and sv.id in ({0})\n", string.Join(",", filter.SERVICE_IDs));
                }

                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SSE.BEGIN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND TREA.IN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else
                {
                    if (filter.TIME_FROM != null)
                    {
                        query += string.Format("AND SR.INTRUCTION_TIME >={0} \n", filter.TIME_FROM);
                    }
                    if (filter.TIME_TO != null)
                    {
                        query += string.Format("AND SR.INTRUCTION_TIME <{0} \n", filter.TIME_TO);
                    }
                    if (filter.FINISH_TIME_FROM != null)
                    {
                        query += string.Format("AND SR.FINISH_TIME >={0} \n", filter.FINISH_TIME_FROM);
                    }
                    if (filter.FINISH_TIME_TO != null)
                    {
                        query += string.Format("AND SR.FINISH_TIME <{0}\n ", filter.FINISH_TIME_TO);
                    }
                }

                if ((filter.EXECUTE_ROOM_IDs ?? filter.MY_SURG_ROOM_IDs) != null)
                {
                    query += string.Format("and sr.execute_room_id in ({0})\n", string.Join(",", filter.EXECUTE_ROOM_IDs ?? filter.MY_SURG_ROOM_IDs));
                }
                if (filter.REQUEST_ROOM_IDs != null)//
                {
                    query += string.Format("and sr.request_room_id in ({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
                if (filter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("and sr.request_department_id = {0}\n", filter.REQUEST_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("and sr.execute_department_id = {0}\n", filter.EXECUTE_DEPARTMENT_ID);
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("and sr.execute_room_id = {0}\n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
                }
                if (filter.FINAL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",",filter.FINAL_PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }

                if (filter.IS_NT_NGT_0.HasValue)
                {
                    if (filter.IS_NT_NGT_0.Value == 0)
                    {
                        query += string.Format("AND (SR.TREATMENT_TYPE_ID <> {0} or SR.TREATMENT_TYPE_ID is null) \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                    else if (filter.IS_NT_NGT_0.Value == 1)
                    {
                        query += string.Format("AND SR.TREATMENT_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
                    }
                }
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00040RDO>(paramGet, query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }


        public List<SESE_PTTT_METHOD> GetSesePtttMethod(List<long> listSSIDs)
        {
            try
            {
                List<SESE_PTTT_METHOD> result = new List<SESE_PTTT_METHOD>();
                string query = "--Phuong phap pttt thuc te\n";
                query += "SELECT \n";
                query += "SPM.PTTT_METHOD_ID,\n ";
                query += "SPM.SERE_SERV_PTTT_ID,\n ";
                query += "SPM.TDL_SERE_SERV_ID,\n ";
                query += "SPM.TDL_SERVICE_REQ_ID,\n ";
                query += "SPM.PTTT_GROUP_ID,\n ";
                query += "SPM.AMOUNT,\n ";
                query += "SPM.EKIP_ID\n ";

                query += "FROM HIS_RS.HIS_SESE_PTTT_METHOD SPM WHERE 1=1\n";

                if (listSSIDs != null)
                {
                    query += string.Format("AND SPM.TDL_SERE_SERV_ID IN ({0}) \n", string.Join(",", listSSIDs));
                }

               
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SESE_PTTT_METHOD>(paramGet, query);


                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }
    }
}
