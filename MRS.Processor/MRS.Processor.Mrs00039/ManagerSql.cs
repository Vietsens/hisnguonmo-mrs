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

namespace MRS.Processor.Mrs00039
{
    public class ManagerSql
    {
        public List<Mrs00039RDO> GetSereServ(Mrs00039Filter filter)
        {
            try
            {
                List<Mrs00039RDO> result = new List<Mrs00039RDO>();
                string query = "--dich vu phau thuat\n";
                query += "SELECT \n";
                query += "SS.*\n, ";
                query += "sr.TDL_PATIENT_NAME PATIENT_NAME, \n";
                query += "sr.TDL_PATIENT_CODE PATIENT_CODE, \n";
                query += "sr.TDL_PATIENT_ADDRESS VIR_ADDRESS, \n";
                query += "SR.START_TIME, \n";
                query += "SR.FINISH_TIME TDL_FINISH_TIME, \n";
                query += "sr.TDL_PATIENT_DOB, \n";
                query += "SR.TDL_PATIENT_GENDER_ID, \n";
                query += "sr.ICD_NAME, \n";
                query += "sr.ICD_CODE, \n";
                query += "TREA.TDL_PATIENT_TYPE_ID, \n";
                query += "TREA.IN_TIME, \n";
                query += "TREA.OUT_TIME, \n";
                query += "SR.EXECUTE_LOGINNAME,\n ";
                query += "SR.EXECUTE_USERNAME \n";

                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID=SR.ID  \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON sr.TREATMENT_ID=TREA.ID  \n";
                query += "JOIN HIS_RS.HIS_SERVICE SV ON SS.SERVICE_ID=SV.ID  \n";
                query += "LEFT JOIN HIS_RS.HIS_PTTT_GROUP PG ON SV.PTTT_GROUP_ID=PG.ID \n";
                if (filter.REPORT_TYPE_CODE_CATEGORY_CODE != null)
                {
                    query += "JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON SS.SERVICE_ID=SRC.SERVICE_ID AND SRC.REPORT_TYPE_CODE = 'MRS00439' AND SRC.CATEGORY_CODE = '439_SURG' \n";
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {

                    query += "JOIN HIS_RS.HIS_SERE_SERV_BILL SSB ON SSB.SERE_SERV_ID=SS.ID  \n";
                    query += "JOIN HIS_RS.HIS_TRANSACTION TRAN ON TRAN.ID=SSB.BILL_ID  \n";
                }
                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {

                    query += "JOIN HIS_RS.HIS_SERE_SERV_EXT SSE ON SSE.SERE_SERV_ID=SS.ID  \n";
                }
                query += "WHERE 1=1 \n";
                if (filter.HAS_IS_EXPEND!=true)
                {
                    query += "AND SS.IS_EXPEND IS NULL\n";
                }
                
                query += "AND SS.IS_NO_EXECUTE IS NULL\n";
               
                query += "and sr.is_delete =0 \n";
                query += "and ss.is_delete =0 \n";
                
                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("and (sv.service_type_id in ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }
                else
                {
                    query += string.Format("AND (SV.SERVICE_TYPE_ID = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT);
                }
                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("and sv.id in ({0})\n", string.Join(",", filter.SERVICE_IDs));
                }
                if (!string.IsNullOrWhiteSpace(filter.PTTT_GROUP_CODE__PTs))
                {
                    query += string.Format("OR PG.PTTT_GROUP_CODE in ('{0}')\n", filter.PTTT_GROUP_CODE__PTs.Replace(",", "','"));
                }
                query += string.Format(") \n");

                if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SSE.BEGIN_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 8)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 7)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} and {1} AND TREA.IS_ACTIVE={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO, IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND TREA.OUT_TIME BETWEEN {0} and {1} AND TREA.IS_PAUSE ={2}\n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO,IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM ?? filter.FINISH_TIME_FROM, filter.TIME_TO ?? filter.FINISH_TIME_TO,IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
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
                if (filter.REQUEST_ROOM_IDs != null)
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
                    query += string.Format("and sr.execute_department_id in ({0})\n", string.Join(",",filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_ROOM_ID != null)
                {
                    query += string.Format("and sr.execute_room_id = {0}\n", filter.EXECUTE_ROOM_ID);
                }
                if (filter.TREATMENT_TYPE_ID != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0} \n", filter.TREATMENT_TYPE_ID);
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

                //lọc theo đối tượng thanh toán
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and ss.PATIENT_TYPE_ID in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }

                //lọc theo đối tượng bệnh nhân
                if (filter.TDL_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and TREA.TDL_PATIENT_TYPE_ID in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
                }

                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00039RDO>(paramGet, query);


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
