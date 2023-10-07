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

namespace MRS.Processor.Mrs00272
{
    public partial class ManagerSql : BusinessBase
    {
        public List<SereServSDO> GetSereServDO(Mrs00272Filter filter)
        {
            List<SereServSDO> result = new List<SereServSDO>();
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "SS.TDL_SERVICE_TYPE_ID ,\n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME, \n";
                query += "SS.VIR_PRICE ,\n";
                query += "SS.TDL_HEIN_SERVICE_TYPE_ID, \n";
                query += "SS.VAT_RATIO, \n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                if (filter.IS_NO_DETAIL != true)
                {
                 //   query += "TREA.TDL_PATIENT_DOB, \n";
                    query += "TREA.TREATMENT_CODE, \n";
                    query += "TREA.TDL_PATIENT_NAME, \n";
                    query += "SS.TDL_SERVICE_REQ_CODE SERVICE_REQ_CODE, \n";
                }
                query += "sum(SS.VIR_TOTAL_PRICE) vir_total_price, \n";
                query += "sum(SS.AMOUNT) amount \n";
                //query += "SUM(SS.VIR_TOTAL_PRICE) VIR_TOTAL_PRICE \n";

                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID \n";
                query += "JOIN HIS_TRANSACTION BI ON BI.ID = SSB.BILL_ID \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SSB.SERE_SERV_ID = SSD.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL) \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND BI.IS_CANCEL IS NULL \n";
                query += "AND SSD.ID IS NULL \n";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME >= {0} \n", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME < {0} \n", filter.TIME_TO);
                }
                if (filter.ROOM_IDs != null)
                {
                    query += string.Format("AND (ss.tdl_request_room_id in({0}) and ss.tdl_service_type_id<>{1} or ss.tdl_execute_room_id in ({0}) and ss.tdl_service_type_id = {1}) \n", string.Join(",", filter.ROOM_IDs), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND (ss.tdl_request_department_id in({0}) and ss.tdl_service_type_id<>{1} or ss.tdl_execute_department_id in ({0}) and ss.tdl_service_type_id = {1}) \n", string.Join(",", filter.DEPARTMENT_IDs), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                }
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND (ss.tdl_request_department_id ={0} and ss.tdl_service_type_id<>{1} or ss.tdl_execute_department_id ={0} and ss.tdl_service_type_id = {1}) \n", filter.DEPARTMENT_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                }
                if (filter.TREATMENT_TYPE_ID.HasValue)
                {
                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        query += string.Format("AND (trea.tdl_treatment_type_id = {0} or ss.tdl_service_type_id = {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                    }

                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    {
                        query += string.Format("AND (trea.tdl_treatment_type_id = {0} or ss.tdl_service_type_id = {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                    }

                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        query += string.Format("AND (trea.tdl_treatment_type_id = {0} and ss.tdl_service_type_id <> {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                    }
                }

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND trea.tdl_treatment_type_id in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = filter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0}) \n", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }

                query += "GROUP BY \n";
                query += "SS.TDL_SERVICE_TYPE_ID ,\n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME, \n";
                query += "SS.VIR_PRICE ,\n";
                query += "SS.TDL_HEIN_SERVICE_TYPE_ID, \n";
                query += "SS.VAT_RATIO, \n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                if (filter.IS_NO_DETAIL != true)
                {
                   // query += "TREA.TDL_PATIENT_DOB, \n";
                    query += "TREA.TREATMENT_CODE, \n";
                    query += "TREA.TDL_PATIENT_NAME, \n";
                    query += "SS.TDL_SERVICE_REQ_CODE, \n";
                }
                query += "1 \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<SereServSDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }
                query = "\n";
                query += "SELECT \n";
                query += "SS.TDL_SERVICE_TYPE_ID ,\n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME, \n";
                query += "SS.VIR_PRICE ,\n";
                query += "SS.TDL_HEIN_SERVICE_TYPE_ID, \n";
                query += "SS.VAT_RATIO, \n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                if (filter.IS_NO_DETAIL != true)
                {
                   // query += "TREA.TDL_PATIENT_DOB, \n";
                    query += "TREA.TREATMENT_CODE, \n";
                    query += "TREA.TDL_PATIENT_NAME, \n";
                    query += "SS.TDL_SERVICE_REQ_CODE SERVICE_REQ_CODE, \n";
                }
                query += "sum(SS.VIR_TOTAL_PRICE) vir_total_price, \n";
                query += "sum(SS.AMOUNT) amount \n";
                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID \n";
                query += "JOIN HIS_TRANSACTION DE ON DE.ID = SSD.DEPOSIT_ID \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND DE.IS_CANCEL IS NULL \n";


                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME >= {0} \n", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME < {0} \n", filter.TIME_TO);
                }
                if (filter.ROOM_IDs != null)
                {
                    query += string.Format("AND (ss.tdl_request_room_id in({0}) and ss.tdl_service_type_id<>{1} or ss.tdl_execute_room_id in ({0}) and ss.tdl_service_type_id = {1}) \n", string.Join(",", filter.ROOM_IDs), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND (ss.tdl_request_department_id in({0}) and ss.tdl_service_type_id<>{1} or ss.tdl_execute_department_id in ({0}) and ss.tdl_service_type_id = {1}) \n", string.Join(",", filter.DEPARTMENT_IDs), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                }
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND (ss.tdl_request_department_id ={0} and ss.tdl_service_type_id<>{1} or ss.tdl_execute_department_id ={0} and ss.tdl_service_type_id = {1}) \n", filter.DEPARTMENT_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                }
                if (filter.TREATMENT_TYPE_ID.HasValue)
                {
                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        query += string.Format("AND (trea.tdl_treatment_type_id = {0} or ss.tdl_service_type_id = {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                    }

                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    {
                        query += string.Format("AND (trea.tdl_treatment_type_id = {0} or ss.tdl_service_type_id = {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                    }

                    if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        query += string.Format("AND (trea.tdl_treatment_type_id = {0} and ss.tdl_service_type_id <> {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                    }
                }

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND trea.tdl_treatment_type_id in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = filter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND DE.CASHIER_ROOM_ID IN ({0}) \n", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }

                query += "GROUP BY \n";
                query += "SS.TDL_SERVICE_TYPE_ID ,\n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME, \n";
                query += "SS.VIR_PRICE ,\n";
                query += "SS.TDL_HEIN_SERVICE_TYPE_ID, \n";
                query += "SS.VAT_RATIO, \n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                if (filter.IS_NO_DETAIL != true)
                {
                  //  query += "TREA.TDL_PATIENT_DOB, \n";
                    query += "TREA.TREATMENT_CODE, \n";
                    query += "TREA.TDL_PATIENT_NAME, \n";
                    query += "SS.TDL_SERVICE_REQ_CODE, \n";
                }
                query += "1 \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                rs = new MOS.DAO.Sql.SqlDAO().GetSql<SereServSDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }
                if ((filter.HAS_TRANSACTION ?? false) == false)
                {
                    //Các dịch vụ có khoa viện phí trong thời gian báo cáo nhưng không phát sinh giao dịch thanh toán tạm thu dịch vụ
                    query = " --Các dịch vụ có khoa viện phí trong thời gian báo cáo nhưng không phát sinh giao dịch thanh toán tạm thu dịch vụ\n";
                    query += "SELECT \n";
                    query += "SS.TDL_SERVICE_TYPE_ID ,\n";
                    query += "SS.SERVICE_ID, \n";
                    query += "SS.TDL_SERVICE_CODE, \n";
                    query += "SS.TDL_SERVICE_NAME, \n";
                    query += "SS.VIR_PRICE ,\n";
                    query += "SS.TDL_HEIN_SERVICE_TYPE_ID, \n";
                    query += "SS.VAT_RATIO, \n";
                    query += "TREA.TDL_PATIENT_DOB, \n";
                    if (filter.IS_NO_DETAIL != true)
                    {
                      //  query += "TREA.TDL_PATIENT_DOB, \n";
                        query += "TREA.TREATMENT_CODE, \n";
                        query += "TREA.TDL_PATIENT_NAME, \n";
                        query += "SS.TDL_SERVICE_REQ_CODE SERVICE_REQ_CODE, \n";
                    }
                    query += "sum(SS.VIR_TOTAL_PRICE) vir_total_price, \n";
                    query += "sum(SS.AMOUNT) amount \n";
                    query += "FROM HIS_SERE_SERV SS ";
                    query += "JOIN HIS_TREATMENT TREA ON (SS.TDL_TREATMENT_ID = TREA.ID AND TREA.IS_ACTIVE=0 \n";


                    if (filter.TIME_FROM != null)
                    {
                        query += string.Format("AND TREA.FEE_LOCK_TIME >= {0}\n ", filter.TIME_FROM);
                    }
                    if (filter.TIME_TO != null)
                    {
                        query += string.Format("AND TREA.FEE_LOCK_TIME < {0} \n", filter.TIME_TO);
                    }
                    if (filter.ROOM_IDs != null)
                    {
                        query += string.Format("AND (ss.tdl_request_room_id in({0}) and ss.tdl_service_type_id<>{1} or ss.tdl_execute_room_id in ({0}) and ss.tdl_service_type_id = {1}) \n", string.Join(",",filter.ROOM_IDs), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                    }
                    if (filter.DEPARTMENT_IDs != null)
                    {
                        query += string.Format("AND (ss.tdl_request_department_id in({0}) and ss.tdl_service_type_id<>{1} or ss.tdl_execute_department_id in ({0}) and ss.tdl_service_type_id = {1}) \n", string.Join(",", filter.DEPARTMENT_IDs), IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                    }
                    if (filter.DEPARTMENT_ID != null)
                    {
                        query += string.Format("AND (ss.tdl_request_department_id ={0} and ss.tdl_service_type_id<>{1} or ss.tdl_execute_department_id ={0} and ss.tdl_service_type_id = {1}) \n", filter.DEPARTMENT_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH);
                    }
                    if (filter.TREATMENT_TYPE_ID.HasValue)
                    {
                        if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            query += string.Format("AND (trea.tdl_treatment_type_id = {0} or ss.tdl_service_type_id = {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                        }

                        if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                        {
                            query += string.Format("AND (trea.tdl_treatment_type_id = {0} or ss.tdl_service_type_id = {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                        }

                        if (filter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            query += string.Format("AND (trea.tdl_treatment_type_id = {0} and ss.tdl_service_type_id <> {1}) \n", filter.TREATMENT_TYPE_ID, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G);
                        }
                    }

                    if (filter.TREATMENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND trea.tdl_treatment_type_id in ({0}) \n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                    }
                    query += ") \n";
                    query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SS.ID = SSD.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL) \n";
                    query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON (SS.ID = SSB.SERE_SERV_ID AND SSB.IS_CANCEL IS NULL) \n";
                    query += "WHERE SS.IS_DELETE = 0 \n";
                    query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                    query += "AND SSB.ID IS NULL \n";
                    query += "AND SSD.ID IS NULL \n";

                    if (filter.BRANCH_ID != null)
                    {
                        query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID IN ({0})\n ", string.Join(",", HisDepartmentCFG.DEPARTMENTs.Where(p => p.BRANCH_ID == filter.BRANCH_ID).Select(o => o.ID).ToList()));
                    }
                   
                    query += "GROUP BY \n";
                    query += "SS.TDL_SERVICE_TYPE_ID ,\n";
                    query += "SS.SERVICE_ID, \n";
                    query += "SS.TDL_SERVICE_CODE, \n";
                    query += "SS.TDL_SERVICE_NAME, \n";
                    query += "SS.VIR_PRICE ,\n";
                    query += "SS.TDL_HEIN_SERVICE_TYPE_ID, \n";
                    query += "SS.VAT_RATIO, \n";
                    query += "TREA.TDL_PATIENT_DOB, \n";
                    
                    if (filter.IS_NO_DETAIL != true)
                    {
                        //query += "TREA.TDL_PATIENT_DOB, \n";
                        query += "TREA.TREATMENT_CODE, \n";
                        query += "TREA.TDL_PATIENT_NAME, \n";
                        query += "SS.TDL_SERVICE_REQ_CODE, \n";
                    }
                    query += "1 \n";
                    Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                    rs = new MOS.DAO.Sql.SqlDAO().GetSql<SereServSDO>(query);
                    if (rs != null)
                    {
                        result.AddRange(rs);
                    }
                }
                //result = result.GroupBy(o => o.ID).Select(p => p.First()).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
    }
}
