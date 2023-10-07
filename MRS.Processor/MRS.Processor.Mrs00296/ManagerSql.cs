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

namespace MRS.Processor.Mrs00296
{
    public partial class ManagerSql : BusinessBase
    {
        internal List<Mrs00296RDO> GetSereServDO(Mrs00296Filter filter,List<V_HIS_ROOM> listRoom)
        {
            List<Mrs00296RDO> result = new List<Mrs00296RDO>();
            try
            {
                var roomIds = listRoom.Select(p => p.ID).ToList();
                //if (filter.IS_ADD_INFO_AREA == true)
                {
                    var rooms = listRoom;
                    if (filter.AREA_IDs != null)
                    {
                        rooms = rooms.Where(o => filter.AREA_IDs.Contains(o.AREA_ID ?? 0)).ToList();
                    }
                    if (filter.REQUEST_DEPARTMENT_IDs != null)
                    {
                        rooms = rooms.Where(o => filter.REQUEST_DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)).ToList();
                    }
                    roomIds = rooms.Select(p => p.ID).ToList();
                }
                LogSystem.Info("COUNT ROOM: " + roomIds.Count);
                string query = "";
                query += "SELECT\n";
                query += "SS.ID AS SERE_SERV_ID,\n";
                query += "SS.PATIENT_TYPE_ID AS SS_PATIENT_TYPE_ID,\n";
                query += "SS.PRIMARY_PATIENT_TYPE_ID,\n";
                query += "BI.ID TRANSACTION_ID,\n";
                query += "SS.SERVICE_ID,\n";
                query += "SS.TDL_TREATMENT_CODE,\n";
                query += "SR.START_TIME EXECUTE_TIME,\n";
                query += "TREA.TDL_PATIENT_CLASSIFY_ID,\n";
                query += "SS.HEIN_RATIO,\n";
                //query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE,\n";
                //query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME,\n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID,\n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID,\n";
                query += "SR.REQUEST_USERNAME,\n";
                query += "SR.REQUEST_LOGINNAME,\n";
                query += "SR.EXECUTE_USERNAME,\n";
                query += "SR.EXECUTE_LOGINNAME,\n";
                query += "SR.INTRUCTION_TIME, \n";
                //query += "SS.TDL_REQUEST_DEPARTMENT_ID,\n";
                query += "SS.TDL_EXECUTE_ROOM_ID,\n";
                if (filter.IS_ADD_TREA_INFO == true)
                {
                    query += "TREA.TDL_PATIENT_CODE,\n";
                    query += "TREA.TDL_PATIENT_NAME,\n";
                    query += "TREA.TDL_PATIENT_DOB,\n";
                    query += "TREA.TDL_PATIENT_GENDER_ID,\n";
                    query += "TREA.ICD_CODE, \n";
                    query += "TREA.ICD_NAME, \n";
                    query += "TREA.TDL_PATIENT_TYPE_ID,\n";
                    query += "ACC.ACCOUNT_BOOK_CODE,\n";
                    query += "ACC.ACCOUNT_BOOK_NAME,\n";
                    query += "nvl(BI.NUM_ORDER,0) TRANSACTION_NUM_ORDER,\n";
                }
                else
                {
                    query += "TREA.TDL_PATIENT_CODE,\n";
                    query += "TREA.TDL_PATIENT_NAME,\n";
                    query += "TREA.TDL_PATIENT_DOB,\n";
                    query += "TREA.TDL_PATIENT_GENDER_ID,\n";
                    query += "TREA.TDL_PATIENT_TYPE_ID,\n";
                    query += "TREA.ICD_CODE, \n";
                    query += "TREA.ICD_NAME, \n";
                }
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += " (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) TDL_REQUEST_ROOM_ID,\n";
                }
                else
                {
                    query += "SS.TDL_REQUEST_ROOM_ID,\n";
                }
                query += "SS.PRICE,\n";
                query += "(CASE WHEN SSD.ID IS NULL OR SSD.AMOUNT>SSB.PRICE THEN SS.AMOUNT ELSE 0 END) AS AMOUNT_DEPOSIT_BILL,\n";
                query += "(CASE WHEN SSD.ID IS NULL THEN SSB.PRICE ELSE (CASE WHEN SSD.AMOUNT>=SSB.PRICE THEN 0 ELSE SSB.PRICE-SSD.AMOUNT END) END) AS TOTAL_DEPOSIT_BILL_PRICE,\n";
                query += "(CASE WHEN SSD.ID IS NULL AND SS.VIR_TOTAL_PATIENT_PRICE_BHYT > 0 THEN SS.VIR_TOTAL_PATIENT_PRICE_BHYT ELSE 0 END) AS VIR_TOTAL_PATIENT_PRICE_BHYT,\n";
                query += "0 AS AMOUNT_REPAY,\n";
                query += "BI.CASHIER_ROOM_ID,\n";
                query += "BI.PAY_FORM_ID,\n";
                query += "BI.CASHIER_LOGINNAME,\n";
                query += "BI.CASHIER_USERNAME,\n";
                query += "BI.TRANSACTION_DATE,\n";
                query += "BI.TRANSACTION_TIME,\n";
                if (filter.EXAM_IS_CONSULT == true)
                {
                    query += "(case when sr.service_req_type_id=1 and sr.consultant_loginname is null then sr.execute_loginname else sr.consultant_loginname end) CONSULTANT_LOGINNAME,\n";
                    query += "(case when sr.service_req_type_id=1 and sr.consultant_username is null then sr.execute_username else sr.consultant_username end) CONSULTANT_USERNAME,\n";
                }
                else
                {
                    query += "SR.CONSULTANT_LOGINNAME,\n";
                    query += "SR.CONSULTANT_USERNAME,\n";
                
                }
                query += "0 AS TOTAL_REPAY_PRICE\n";

                query += "FROM HIS_SERE_SERV SS\n";
                query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID\n";
                query += "LEFT JOIN HIS_TRANSACTION BI ON BI.ID = SSB.BILL_ID\n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID\n";
                query += "JOIN HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID\n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    //query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID\n";
                    query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON SR.REQUEST_ROOM_ID = ER.ROOM_ID AND (ER.IS_EXAM IS NULL OR ER.IS_EXAM <>1) AND (ER.IS_SURGERY IS NULL OR ER.IS_SURGERY <>1)\n";
                    query += "LEFT JOIN HIS_SERVICE_REQ PRSR ON PRSR.ID = SR.PARENT_ID\n";
                }
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID\n";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SSD.SERE_SERV_ID = SSB.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL AND NVL(BI.KC_AMOUNT,0)>0)\n";
                query += "JOIN HIS_SERVICE_TYPE ST ON SS.TDL_SERVICE_TYPE_ID = ST.ID \n";
                query += "LEFT JOIN HIS_ACCOUNT_BOOK ACC ON ACC.ID=BI.ACCOUNT_BOOK_ID \n";
                query += "WHERE SS.IS_DELETE = 0\n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL\n";
                query += "AND BI.IS_CANCEL IS NULL\n";


                if (filter.TAKE_ZERO_PRICE == true)
                { }
                else
                {
                    query += string.Format("AND BI.AMOUNT > 0 \n");
                }

                if (filter.IS_EXECUTED == 1)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.IS_EXECUTED == 2)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (filter.IS_POLICE_OFFICER == true)
                {
                    query += string.Format("AND TREA.TDL_HEIN_CARD_NUMBER LIKE 'CA%' \n");
                }
                if (!string.IsNullOrWhiteSpace(filter.HEIN_RATIO))
                {
                    query += string.Format("AND SS.HEIN_RATIO = {0}\n", filter.HEIN_RATIO);
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }
                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }
                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE==true)
                {
                    if (filter.SS_PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs));
                    }
                    if (filter.PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                    }
                }
                else
                {
                    if (filter.SS_PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND SS.PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs));
                    }
                    if (filter.PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                    }
                }
                
                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_CLASSIFY_ID IN ({0}) \n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }
                if (filter.WORK_PLACE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_WORK_PLACE_ID IN ({0}) \n", string.Join(",", filter.WORK_PLACE_IDs));
                }

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                //if (filter.INPUT_DATA_ID_TIME_TYPE != null)
                //{
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.RESULTING_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME >= {0} AND BI.TRANSACTION_TIME < {1}\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                //}
                else
                {
                    query += string.Format("AND BI.TRANSACTION_TIME >= {0} AND BI.TRANSACTION_TIME < {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                if (filter.INPUT_DATA_ID_IS_BILL == 1)
                {
                    query += string.Format("AND BI.ID IS NOT NULL\n");
                }
                else if (filter.INPUT_DATA_ID_IS_BILL == 2)
                {
                    query += string.Format("AND BI.ID IS  NULL\n");
                }

                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }

                if (filter.REQUEST_LOGINNAMEs != null)
                {
                    query += string.Format("AND SR.REQUEST_LOGINNAME IN ('{0}')\n", string.Join("','", filter.REQUEST_LOGINNAMEs));
                }

                if (filter.EXECUTE_LOGINNAMEs != null)
                {
                    query += string.Format("AND SR.EXECUTE_LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAMEs));
                }


                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }


                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID in ({0}) \n", string.Join(",", filter.SERVICE_IDs));
                }


                if (filter.EXACT_PARENT_SERVICE_IDs != null)
                {
                    query += string.Format("AND SV.PARENT_ID in ({0}) \n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
                }
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("AND BI.CASHIER_LOGINNAME IN ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }
                if (filter.TRANSACTION_TYPE_IDs != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TYPE_ID IN ({0})\n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
                }
                if (filter.AREA_IDs != null || filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += "AND (1=0 ";

                    int skip = 0;
                    while (roomIds.Count - skip > 0)
                    {
                        var listId = roomIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        if (filter.IS_MOV_CLS_TO_PARENT == true)
                        {
                            query += string.Format("or (SS.TDL_EXECUTE_ROOM_ID IN ({0}) or (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) IN ({0}))\n", string.Join(",", listId));
                        }
                        else
                        {
                            query += string.Format("or (SS.TDL_EXECUTE_ROOM_ID IN ({0}) or SS.TDL_REQUEST_ROOM_ID IN ({0}))\n", string.Join(",", listId));
                        }
                    }

                    query += ")\n";
                }
               
                if (filter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = filter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0})\n", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00296RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }
                query = "";
                query += "SELECT\n";

                query += "SS.ID AS SERE_SERV_ID,\n";
                query += "SS.PRIMARY_PATIENT_TYPE_ID,\n";
                query += "SS.PATIENT_TYPE_ID AS SS_PATIENT_TYPE_ID,\n";
                query += "DE.ID TRANSACTION_ID,\n";
                query += "SS.SERVICE_ID,\n";
                query += "SS.TDL_TREATMENT_CODE,\n";
                query += "SR.START_TIME EXECUTE_TIME,\n";
                query += "TREA.TDL_PATIENT_CLASSIFY_ID,\n";
                //query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE,\n";
                //query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME,\n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID,\n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID,\n";
                query += "SR.REQUEST_USERNAME,\n";
                query += "SR.REQUEST_LOGINNAME,\n";
                query += "SR.EXECUTE_USERNAME,\n";
                query += "SR.EXECUTE_LOGINNAME,\n";
                query += "SR.INTRUCTION_TIME, \n";
                //query += "SS.TDL_REQUEST_DEPARTMENT_ID,\n";
                if (filter.IS_ADD_TREA_INFO == true)
                {
                    query += "TREA.TDL_PATIENT_CODE,\n";
                    query += "TREA.TDL_PATIENT_NAME,\n";
                    query += "TREA.TDL_PATIENT_DOB,\n";
                    query += "TREA.TDL_PATIENT_GENDER_ID,\n";
                    query += "TREA.ICD_CODE, \n";
                    query += "TREA.ICD_NAME, \n";
                    query += "TREA.TDL_PATIENT_TYPE_ID,\n";
                    query += "ACC.ACCOUNT_BOOK_CODE,\n";
                    query += "ACC.ACCOUNT_BOOK_NAME,\n";
                    query += "nvl(DE.NUM_ORDER,0) TRANSACTION_NUM_ORDER,\n";
                }
                else
                {
                    query += "TREA.TDL_PATIENT_CODE,\n";
                    query += "TREA.TDL_PATIENT_NAME,\n";
                    query += "TREA.TDL_PATIENT_DOB,\n";
                    query += "TREA.TDL_PATIENT_GENDER_ID,\n";
                    query += "TREA.TDL_PATIENT_TYPE_ID,\n";
                    query += "TREA.ICD_CODE, \n";
                    query += "TREA.ICD_NAME, \n";
                }
                query += "SS.TDL_EXECUTE_ROOM_ID,\n";

                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += " (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) TDL_REQUEST_ROOM_ID,\n";
                }
                else
                {
                    query += "SS.TDL_REQUEST_ROOM_ID,\n";
                }
                query += "SS.PRICE,\n";
                
                if (filter.CLEAR_AMOUNT_WHEN_NO_PRICE == true)
                {
                    query += "(case when SSD.AMOUNT>0 then SS.AMOUNT else 0 end) AS AMOUNT_DEPOSIT_BILL,\n";
                }
                else
                {
                    query += "SS.AMOUNT AS AMOUNT_DEPOSIT_BILL,\n";
                }
                query += "SSD.AMOUNT AS TOTAL_DEPOSIT_BILL_PRICE,\n";
                query += "SS.VIR_TOTAL_PATIENT_PRICE_BHYT AS VIR_TOTAL_PATIENT_PRICE_BHYT,\n";
                query += "0 AS AMOUNT_REPAY,\n";
                query += "DE.CASHIER_ROOM_ID,\n";
                query += "DE.PAY_FORM_ID,\n";
                query += "DE.CASHIER_LOGINNAME,\n";
                query += "DE.CASHIER_USERNAME,\n";
                query += "DE.TRANSACTION_DATE,\n";
                query += "DE.TRANSACTION_TIME,\n";
                if (filter.EXAM_IS_CONSULT == true)
                {
                    query += "(case when sr.service_req_type_id=1 and sr.consultant_loginname is null then sr.execute_loginname else sr.consultant_loginname end) CONSULTANT_LOGINNAME,\n";
                    query += "(case when sr.service_req_type_id=1 and sr.consultant_username is null then sr.execute_username else sr.consultant_username end) CONSULTANT_USERNAME,\n";
                }
                else
                {
                    query += "SR.CONSULTANT_LOGINNAME,\n";
                    query += "SR.CONSULTANT_USERNAME,\n";

                }
                query += "0 AS TOTAL_REPAY_PRICE\n";

                query += "FROM HIS_SERE_SERV SS\n";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID\n";
                query += "LEFT JOIN HIS_TRANSACTION DE ON DE.ID = SSD.DEPOSIT_ID\n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID\n";
                query += "JOIN HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID\n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    //query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID\n";
                    query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON SR.REQUEST_ROOM_ID = ER.ROOM_ID AND (ER.IS_EXAM IS NULL OR ER.IS_EXAM <>1) AND (ER.IS_SURGERY IS NULL OR ER.IS_SURGERY <>1)\n";
                    query += "LEFT JOIN HIS_SERVICE_REQ PRSR ON PRSR.ID = SR.PARENT_ID\n";
                }
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID\n";
                query += "JOIN HIS_SERVICE_TYPE ST ON SS.TDL_SERVICE_TYPE_ID = ST.ID \n";
                query += "LEFT JOIN HIS_ACCOUNT_BOOK ACC ON ACC.ID=DE.ACCOUNT_BOOK_ID \n";
                query += "WHERE SS.IS_DELETE = 0\n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL\n";
                query += "AND DE.IS_CANCEL IS NULL\n";


                if (filter.TAKE_ZERO_PRICE == true)
                { }
                else
                {
                    query += string.Format("AND DE.AMOUNT > 0 \n");
                }

                if (filter.IS_EXECUTED == 1)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.IS_EXECUTED == 2)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (filter.IS_POLICE_OFFICER == true)
                {
                    query += string.Format("AND TREA.TDL_HEIN_CARD_NUMBER LIKE 'CA%' \n");
                }
                if (!string.IsNullOrWhiteSpace(filter.HEIN_RATIO))
                {
                    query += string.Format("AND SS.HEIN_RATIO = {0}\n", filter.HEIN_RATIO);
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }
                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }
                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE == true)
                {
                    if (filter.SS_PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs));
                    }
                    if (filter.PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                    }
                }
                else
                {
                    if (filter.SS_PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND SS.PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs));
                    }
                    if (filter.PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                    }
                }
                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_CLASSIFY_ID IN ({0}) \n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }
                if (filter.WORK_PLACE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_WORK_PLACE_ID IN ({0}) \n", string.Join(",", filter.WORK_PLACE_IDs));
                }

                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                //if (filter.INPUT_DATA_ID_TIME_TYPE != null)
                //{
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.RESULTING_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME >= {0} AND DE.TRANSACTION_TIME < {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                //}
                else
                {
                    query += string.Format("AND DE.TRANSACTION_TIME >= {0} AND DE.TRANSACTION_TIME < {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }

                if (filter.INPUT_DATA_ID_IS_BILL == 1)
                {
                    query += string.Format("AND DE.ID IS NOT NULL\n");
                }
                else if (filter.INPUT_DATA_ID_IS_BILL == 2)
                {
                    query += string.Format("AND DE.ID IS  NULL\n");
                }

                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }

                if (filter.REQUEST_LOGINNAMEs != null)
                {
                    query += string.Format("AND SR.REQUEST_LOGINNAME IN ('{0}')\n", string.Join("','", filter.REQUEST_LOGINNAMEs));
                }

                if (filter.EXECUTE_LOGINNAMEs != null)
                {
                    query += string.Format("AND SR.EXECUTE_LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAMEs));
                }

                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }


                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID in ({0}) \n", string.Join(",", filter.SERVICE_IDs));
                }


                if (filter.EXACT_PARENT_SERVICE_IDs != null)
                {
                    query += string.Format("AND SV.PARENT_ID in ({0}) \n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
                }


                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("AND DE.CASHIER_LOGINNAME IN ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND DE.CASHIER_ROOM_ID IN ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }
                if (filter.TRANSACTION_TYPE_IDs != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TYPE_ID IN ({0})\n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
                }
                if (filter.AREA_IDs != null || filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += "AND (1=0 ";
                    int skip = 0;
                    while (roomIds.Count - skip > 0)
                    {
                        var listId = roomIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        if (filter.IS_MOV_CLS_TO_PARENT == true)
                        {
                            query += string.Format("or (SS.TDL_EXECUTE_ROOM_ID IN ({0}) or (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) IN ({0}))\n", string.Join(",", listId));
                        }
                        else
                        {
                            query += string.Format("or (SS.TDL_EXECUTE_ROOM_ID IN ({0}) or SS.TDL_REQUEST_ROOM_ID IN ({0}))\n", string.Join(",", listId));
                        }
                    }

                    query += ")\n";
                }
                if (filter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = filter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND DE.CASHIER_ROOM_ID IN ({0})\n", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00296RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }

                query = "";
                query += "SELECT\n";

                query += "SS.ID AS SERE_SERV_ID,\n";
                query += "SS.PRIMARY_PATIENT_TYPE_ID,\n";
                query += "SS.PATIENT_TYPE_ID AS SS_PATIENT_TYPE_ID,\n";
                query += "RE.ID TRANSACTION_ID,\n";
                query += "SS.SERVICE_ID,\n";
                query += "SS.TDL_TREATMENT_CODE,\n";
                query += "SR.START_TIME EXECUTE_TIME,\n";
                query += "TREA.TDL_PATIENT_CLASSIFY_ID,\n";
                query += "SR.REQUEST_USERNAME,\n";
                query += "SR.REQUEST_LOGINNAME,\n";
                query += "SR.EXECUTE_USERNAME,\n";
                query += "SR.EXECUTE_LOGINNAME,\n";
                query += "SR.INTRUCTION_TIME, \n";
                //query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE,\n";
                //query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME,\n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID,\n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID,\n";
                query += "SS.TDL_EXECUTE_ROOM_ID,\n";
                if (filter.IS_ADD_TREA_INFO == true)
                {
                    query += "TREA.TDL_PATIENT_CODE,\n";
                    query += "TREA.TDL_PATIENT_NAME,\n";
                    query += "TREA.TDL_PATIENT_DOB,\n";
                    query += "TREA.TDL_PATIENT_GENDER_ID,\n";
                    query += "TREA.TDL_PATIENT_TYPE_ID,\n";
                    query += "TREA.ICD_CODE, \n";
                    query += "TREA.ICD_NAME, \n";
                    query += "ACC.ACCOUNT_BOOK_CODE,\n";
                    query += "ACC.ACCOUNT_BOOK_NAME,\n";
                    query += "nvl(RE.NUM_ORDER,0) TRANSACTION_NUM_ORDER,\n";
                }
                else
                {
                    query += "TREA.TDL_PATIENT_CODE,\n";
                    query += "TREA.TDL_PATIENT_NAME,\n";
                    query += "TREA.TDL_PATIENT_DOB,\n";
                    query += "TREA.TDL_PATIENT_GENDER_ID,\n";
                    query += "TREA.TDL_PATIENT_TYPE_ID,\n";
                    query += "TREA.ICD_CODE, \n";
                    query += "TREA.ICD_NAME, \n";
                }

                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += " (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) TDL_REQUEST_ROOM_ID,\n";
                }
                else
                {
                    query += "SS.TDL_REQUEST_ROOM_ID,\n";
                }
                query += "SS.PRICE,\n";
                query += "0 AS AMOUNT_DEPOSIT_BILL,\n";
                query += "0 AS TOTAL_DEPOSIT_BILL_PRICE,\n";
                if (filter.CLEAR_AMOUNT_WHEN_NO_PRICE == true)
                {
                    query += "(case when SDR.AMOUNT>0 then SS.AMOUNT else 0 end) AS AMOUNT_REPAY,\n";
                }
                else
                {
                    query += "SS.AMOUNT AS AMOUNT_REPAY,\n";
                }
                query += "RE.CASHIER_ROOM_ID,\n";
                query += "RE.PAY_FORM_ID,\n";
                query += "RE.CASHIER_LOGINNAME,\n";
                query += "RE.CASHIER_USERNAME,\n";
                query += "RE.TRANSACTION_DATE,\n";
                query += "RE.TRANSACTION_TIME,\n";
                if (filter.EXAM_IS_CONSULT == true)
                {
                    query += "(case when sr.service_req_type_id=1 and sr.consultant_loginname is null then sr.execute_loginname else sr.consultant_loginname end) CONSULTANT_LOGINNAME,\n";
                    query += "(case when sr.service_req_type_id=1 and sr.consultant_username is null then sr.execute_username else sr.consultant_username end) CONSULTANT_USERNAME,\n";
                }
                else
                {
                    query += "SR.CONSULTANT_LOGINNAME,\n";
                    query += "SR.CONSULTANT_USERNAME,\n";

                }
                query += "-SS.VIR_TOTAL_PATIENT_PRICE_BHYT AS VIR_TOTAL_PATIENT_PRICE_BHYT,\n";
                query += "SDR.AMOUNT AS TOTAL_REPAY_PRICE\n";

                query += "FROM HIS_SERE_SERV SS\n";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SS.ID = SSD.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL)\n";
                query += "LEFT JOIN HIS_SESE_DEPO_REPAY SDR ON (SSD.ID = SDR.SERE_SERV_DEPOSIT_ID AND SDR.IS_CANCEL IS NULL)\n";
                query += "LEFT JOIN HIS_TRANSACTION RE ON RE.ID = SDR.REPAY_ID\n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID\n";
                query += "JOIN HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID\n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    //query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID\n";
                    query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON SR.REQUEST_ROOM_ID = ER.ROOM_ID AND (ER.IS_EXAM IS NULL OR ER.IS_EXAM <>1) AND (ER.IS_SURGERY IS NULL OR ER.IS_SURGERY <>1)\n";
                    query += "LEFT JOIN HIS_SERVICE_REQ PRSR ON PRSR.ID = SR.PARENT_ID\n";
                }
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID\n";
                //query += "LEFT JOIN HIS_ROOM RO ON RO.ID = SS.TDL_REQUEST_ROOM_ID\n";
                //query += "LEFT JOIN HIS_ROOM EXAMRO ON EXAMRO.ID = TREA.TDL_FIRST_EXAM_ROOM_ID\n";
                //query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON ER.ROOM_ID = SS.TDL_REQUEST_ROOM_ID\n";
                query += "JOIN HIS_SERVICE_TYPE ST ON SS.TDL_SERVICE_TYPE_ID = ST.ID \n";
                query += "LEFT JOIN HIS_ACCOUNT_BOOK ACC ON ACC.ID=RE.ACCOUNT_BOOK_ID \n";
                query += "WHERE SS.IS_DELETE = 0\n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL\n";
                query += "AND RE.IS_CANCEL IS NULL\n";


                if (filter.TAKE_ZERO_PRICE == true)
                { }
                else
                {
                    query += string.Format("AND RE.AMOUNT > 0 \n");
                }

                if (filter.IS_EXECUTED == 1)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else if (filter.IS_EXECUTED == 2)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
                if (filter.IS_POLICE_OFFICER == true)
                {
                    query += string.Format("AND TREA.TDL_HEIN_CARD_NUMBER LIKE 'CA%' \n");
                }
                if (!string.IsNullOrWhiteSpace(filter.HEIN_RATIO))
                {
                    query += string.Format("AND SS.HEIN_RATIO = {0}\n", filter.HEIN_RATIO);
                }
                if (filter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
                }
                if (filter.EXECUTE_ROOM_IDs != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_ROOM_ID IN ({0}) \n", string.Join(",", filter.EXECUTE_ROOM_IDs));
                }
                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }
                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE == true)
                {
                    if (filter.SS_PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs));
                    }
                    if (filter.PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                    }
                }
                else
                {
                    if (filter.SS_PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND SS.PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs));
                    }
                    if (filter.PATIENT_TYPE_IDs != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                    }
                }
                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_CLASSIFY_ID IN ({0}) \n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }

                if (filter.WORK_PLACE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_PATIENT_WORK_PLACE_ID IN ({0}) \n", string.Join(",", filter.WORK_PLACE_IDs));
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID IN ({0})\n ", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }

                //if (filter.INPUT_DATA_ID_TIME_TYPE != null)
                //{
                if (filter.INPUT_DATA_ID_TIME_TYPE == 1)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 2)
                {
                    query += string.Format("AND SR.RESULTING_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 3)
                {
                    query += string.Format("AND RE.TRANSACTION_TIME >= {0} AND RE.TRANSACTION_TIME< {1}\n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 4)
                {
                    query += string.Format("AND  SR.START_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 5)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else if (filter.INPUT_DATA_ID_TIME_TYPE == 6)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} AND {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }
                //}
                else
                {
                    query += string.Format("AND RE.TRANSACTION_TIME >= {0} AND RE.TRANSACTION_TIME< {1} \n", filter.TIME_FROM, filter.TIME_TO);
                }

                if (filter.INPUT_DATA_ID_IS_BILL == 1)
                {
                    query += string.Format("AND RE.ID IS NOT NULL\n");
                }
                else if (filter.INPUT_DATA_ID_IS_BILL == 2)
                {
                    query += string.Format("AND RE.ID IS  NULL\n");
                }

                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }

                if (filter.REQUEST_LOGINNAMEs != null)
                {
                    query += string.Format("AND SR.REQUEST_LOGINNAME IN ('{0}')\n", string.Join("','", filter.REQUEST_LOGINNAMEs));
                }

                if (filter.EXECUTE_LOGINNAMEs != null)
                {
                    query += string.Format("AND SR.EXECUTE_LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAMEs));
                }

                if (filter.EXECUTE_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_EXECUTE_DEPARTMENT_ID = {0} \n", filter.EXECUTE_DEPARTMENT_ID);
                }


                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_IDs != null)
                {
                    query += string.Format("AND SS.SERVICE_ID in ({0}) \n", string.Join(",", filter.SERVICE_IDs));
                }


                if (filter.EXACT_PARENT_SERVICE_IDs != null)
                {
                    query += string.Format("AND SV.PARENT_ID in ({0}) \n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
                }


                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("AND RE.CASHIER_LOGINNAME IN ('{0}')\n", string.Join("','", filter.CASHIER_LOGINNAMEs));
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND RE.CASHIER_ROOM_ID IN ({0})\n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }

                if (filter.TRANSACTION_TYPE_IDs != null)
                {
                    query += string.Format("AND RE.TRANSACTION_TYPE_ID IN ({0})\n", string.Join(",", filter.TRANSACTION_TYPE_IDs));
                }
                if (filter.AREA_IDs != null || filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += "AND (1=0 ";
                    int skip = 0;
                    while (roomIds.Count - skip > 0)
                    {
                        var listId = roomIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        if (filter.IS_MOV_CLS_TO_PARENT == true)
                        {
                            query += string.Format("or (SS.TDL_EXECUTE_ROOM_ID IN ({0}) or (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) IN ({0}))\n", string.Join(",", listId));
                        }
                        else
                        {
                            query += string.Format("or (SS.TDL_EXECUTE_ROOM_ID IN ({0}) or SS.TDL_REQUEST_ROOM_ID IN ({0}))\n", string.Join(",", listId));
                        }
                    }

                    query += ")\n";
                }
                if (filter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = filter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND RE.CASHIER_ROOM_ID IN ({0})\n", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00296RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
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

        public List<HIS_AREA> GetArea()
        {
            return new MOS.DAO.Sql.SqlDAO().GetSql<HIS_AREA>("select * from his_area where 1=1");
        }

        internal List<HIS_PATIENT_CLASSIFY> GetPatientClassify()
        {
            return new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>("select * from HIS_PATIENT_CLASSIFY where 1=1");
        }
    }
}
