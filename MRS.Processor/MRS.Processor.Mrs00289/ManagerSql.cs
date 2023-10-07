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
using MRS.Processor.Mrs00289;

namespace MRS.Processor.Mrs00296
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00296RDO> GetSereServDO(Mrs00289Filter filter,List<V_HIS_ROOM> listRoom)
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
                string query = "";
                query += "SELECT \n";
                query += "BI.IS_CANCEL,\n";
                query += "SS.ID AS SERE_SERV_ID, \n";
                query += string.Format("(CASE WHEN SS.PATIENT_TYPE_ID = {0} AND NVL(SS.VIR_TOTAL_HEIN_PRICE,0) = 0 THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0) WHEN SS.PATIENT_TYPE_ID = {0} WHEN SSD.ID IS NULL AND SS.PATIENT_TYPE_ID = {0} THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0)-nvl(SS.VIR_TOTAL_PATIENT_PRICE_BHYT,0) ELSE 0 END) AS CHENHLECH, \n", HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += "BI.ID AS TRANSACTION_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_TREATMENT_CODE, \n";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, \n";
                query += "NVL(SS.TDL_SERVICE_CODE,'NONE') AS PARENT_SERVICE_CODE, \n";
                query += "NVL(SS.TDL_SERVICE_NAME,'NONE') AS PARENT_SERVICE_NAME, \n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID, \n";
                query += "SRC.CATEGORY_CODE SERVICE_TYPE_CODE, \n";
                query += "SRC.CATEGORY_NAME SERVICE_TYPE_NAME, \n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID, \n";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, \n";
                query += "SS.TDL_EXECUTE_ROOM_ID, \n";
                query += "SS.PATIENT_TYPE_ID SS_PATIENT_TYPE_ID, \n";
                query += "BI.CASHIER_LOGINNAME, \n";
                query += "BI.CASHIER_USERNAME, \n";
                query += "BI.NUM_ORDER, \n";
                query += "BI.PAY_FORM_ID, \n";
                query += "NVL(PF.PAY_FORM_NAME,'NONE') AS PAY_FORM_NAME, \n";
                query += "NVL(PF.PAY_FORM_CODE,'NONE') AS PAY_FORM_CODE, \n";
                //query += "SS.TDL_REQUEST_ROOM_ID, \n";
                //query += "SS.PRICE, \n";

                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += " (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) TDL_REQUEST_ROOM_ID,\n";
                }
                else
                {
                    query += "SS.TDL_REQUEST_ROOM_ID,\n";
                }
                //query += "(CASE WHEN SSD.ID IS NULL OR SSD.AMOUNT>SSB.PRICE THEN SS.AMOUNT ELSE 0 END) AS AMOUNT_DEPOSIT_BILL, \n";
                query += "(CASE WHEN SSD.ID IS NULL THEN SSB.PRICE ELSE (CASE WHEN SSD.AMOUNT>=SSB.PRICE THEN 0 ELSE SSB.PRICE-SSD.AMOUNT END) END) AS TOTAL_DEPOSIT_BILL_PRICE, \n";
                //query += "(CASE WHEN SSD.ID IS NULL THEN SS.VIR_TOTAL_PATIENT_PRICE_BHYT ELSE 0 END) AS TOTAL_PATIENT_BHYT_PRICE, \n";
                //query += "0 AS AMOUNT_REPAY, \n";
                //query += "BI.CASHIER_ROOM_ID, \n";
                query += "0 AS TOTAL_REPAY_PRICE \n";

                query += "FROM HIS_SERE_SERV SS \n";
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID \n";
                query += "LEFT JOIN HIS_SERVICE PR ON SV.PARENT_ID = PR.ID \n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID\n";
                    query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON SR.REQUEST_ROOM_ID = ER.ROOM_ID AND (ER.IS_EXAM IS NULL OR ER.IS_EXAM <>1) AND (ER.IS_SURGERY IS NULL OR ER.IS_SURGERY <>1)\n";
                    query += "LEFT JOIN HIS_SERVICE_REQ PRSR ON PRSR.ID = SR.PARENT_ID\n";
                }
                query += "JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID \n";
                query += "JOIN HIS_TRANSACTION BI ON BI.ID = SSB.BILL_ID \n";
                query += "LEFT JOIN HIS_PAY_FORM PF ON BI.PAY_FORM_ID = PF.ID \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
                query += "LEFT JOIN HIS_ROOM RO ON RO.ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN HIS_ROOM EXAMRO ON EXAMRO.ID = TREA.TDL_FIRST_EXAM_ROOM_ID \n";
                query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON ER.ROOM_ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SSD.SERE_SERV_ID = SSB.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL AND NVL(BI.KC_AMOUNT,0)>0) \n";
                query += "LEFT JOIN V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00296') \n";
                query += "WHERE 1=1 \n";
                if (filter.IS_ADD_BILL_CANCEL != true)
                {
                    query += "AND SS.IS_DELETE=0 \n";
                    query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                    query += "AND BI.IS_CANCEL IS NULL \n";
                }

                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE==true)
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} ", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} ", filter.SS_PATIENT_TYPE_ID);
                    }
                }
                else
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }
               

                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("AND BI.PAY_FORM_ID ={0} ", filter.PAY_FORM_ID);
                }

                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("AND BI.PAY_FORM_ID in({0}) ", string.Join(",", filter.PAY_FORM_IDs));
                }

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND BI.CASHIER_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.LOGINNAME != null)
                {
                    query += string.Format("AND BI.CASHIER_LOGINNAME ='{0}' ", filter.LOGINNAME);//
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }

                if (filter.AREA_IDs != null || filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += "AND (1=0 \n";
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
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00296RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }
             
               
                query = "";
                query += "SELECT \n";
                query += "DE.IS_CANCEL,\n";
                query += "SS.ID AS SERE_SERV_ID, \n";
                query += string.Format("(CASE WHEN SS.PATIENT_TYPE_ID = {0} AND NVL(SS.VIR_TOTAL_HEIN_PRICE,0) = 0 THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0) WHEN SS.PATIENT_TYPE_ID = {0} WHEN SS.PATIENT_TYPE_ID = {0} THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0)-nvl(SS.VIR_TOTAL_PATIENT_PRICE_BHYT,0) ELSE 0 END) AS CHENHLECH, \n", HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += "DE.ID AS TRANSACTION_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_TREATMENT_CODE, \n";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, \n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID, \n";
                query += "SRC.CATEGORY_CODE SERVICE_TYPE_CODE, \n";
                query += "SRC.CATEGORY_NAME SERVICE_TYPE_NAME, \n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID, \n";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, \n";
                query += "SS.TDL_EXECUTE_ROOM_ID, \n";
                query += "SS.PATIENT_TYPE_ID SS_PATIENT_TYPE_ID, \n";
                query += "DE.CASHIER_LOGINNAME, \n";
                query += "DE.CASHIER_USERNAME, \n";
                query += "DE.NUM_ORDER, \n";
                query += "DE.PAY_FORM_ID, \n";
                query += "NVL(PF.PAY_FORM_NAME,'NONE') AS PAY_FORM_NAME, \n";
                query += "NVL(PF.PAY_FORM_CODE,'NONE') AS PAY_FORM_CODE, \n";
                //query += "SS.TDL_REQUEST_ROOM_ID, \n";
                //query += "SS.PRICE, \n";

                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += " (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) TDL_REQUEST_ROOM_ID,\n";
                }
                else
                {
                    query += "SS.TDL_REQUEST_ROOM_ID,\n";
                }
                //query += "SS.AMOUNT AS AMOUNT_DEPOSIT_BILL, \n";
                query += "SSD.AMOUNT AS TOTAL_DEPOSIT_BILL_PRICE, \n";
                //query += "SS.VIR_TOTAL_PATIENT_PRICE_BHYT AS TOTAL_PATIENT_BHYT_PRICE, \n";
                //query += "0 AS AMOUNT_REPAY, \n";
                //query += "DE.CASHIER_ROOM_ID, \n";
                query += "0 AS TOTAL_REPAY_PRICE \n";

                query += "FROM HIS_SERE_SERV SS \n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID\n";
                    query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON SR.REQUEST_ROOM_ID = ER.ROOM_ID AND (ER.IS_EXAM IS NULL OR ER.IS_EXAM <>1) AND (ER.IS_SURGERY IS NULL OR ER.IS_SURGERY <>1)\n";
                    query += "LEFT JOIN HIS_SERVICE_REQ PRSR ON PRSR.ID = SR.PARENT_ID\n";
                }
                query += "JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID \n";
                query += "JOIN HIS_TRANSACTION DE ON DE.ID = SSD.DEPOSIT_ID \n";
                query += "LEFT JOIN HIS_PAY_FORM PF ON DE.PAY_FORM_ID = PF.ID \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
                query += "LEFT JOIN HIS_ROOM RO ON RO.ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN HIS_ROOM EXAMRO ON EXAMRO.ID = TREA.TDL_FIRST_EXAM_ROOM_ID \n";
                query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON ER.ROOM_ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00296') \n";
                query += "WHERE 1=1 \n";

                if (filter.IS_ADD_DEPO_CANCEL != true)
                {
                    query += "AND SS.IS_DELETE = 0 \n";
                    query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                    query += "AND DE.IS_CANCEL IS NULL \n";
                }



                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE == true)
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }
                else
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }

                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("AND DE.PAY_FORM_ID ={0} ", filter.PAY_FORM_ID);
                }

                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("AND DE.PAY_FORM_ID in({0}) ", string.Join(",", filter.PAY_FORM_IDs));
                }


                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND DE.CASHIER_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }
                if (filter.LOGINNAME != null)
                {
                    query += string.Format("AND DE.CASHIER_LOGINNAME ='{0}' ", filter.LOGINNAME);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND DE.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }

                if (filter.AREA_IDs != null || filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += "AND (1=0 \n";
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
                    query += string.Format("AND DE.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00296RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }

                query = "";
                query += "SELECT \n";
                query += "RE.IS_CANCEL,\n";
                query += "SS.ID AS SERE_SERV_ID, \n";
                query += "0 AS CHENHLECH, \n";
                query += "RE.ID AS TRANSACTION_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_TREATMENT_CODE, \n";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, \n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID, \n";
                query += "SRC.CATEGORY_CODE SERVICE_TYPE_CODE, \n";
                query += "SRC.CATEGORY_NAME SERVICE_TYPE_NAME, \n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID, \n";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, \n";
                query += "SS.TDL_EXECUTE_ROOM_ID, \n";
                query += "SS.PATIENT_TYPE_ID SS_PATIENT_TYPE_ID, \n";
                //query += "SS.TDL_REQUEST_ROOM_ID, \n";
                //query += "SS.PRICE, \n";

                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += " (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) TDL_REQUEST_ROOM_ID,\n";
                }
                else
                {
                    query += "SS.TDL_REQUEST_ROOM_ID,\n";
                }
                //query += "0 AS AMOUNT_DEPOSIT_BILL, \n";
                query += "0 AS TOTAL_DEPOSIT_BILL_PRICE, \n";
                //query += "SS.AMOUNT AS AMOUNT_REPAY, \n";
                //query += "RE.CASHIER_ROOM_ID, \n";
                query += "SDR.AMOUNT AS TOTAL_REPAY_PRICE \n";

                query += "FROM HIS_SERE_SERV SS \n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID\n";
                    query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON SR.REQUEST_ROOM_ID = ER.ROOM_ID AND (ER.IS_EXAM IS NULL OR ER.IS_EXAM <>1) AND (ER.IS_SURGERY IS NULL OR ER.IS_SURGERY <>1)\n";
                    query += "LEFT JOIN HIS_SERVICE_REQ PRSR ON PRSR.ID = SR.PARENT_ID\n";
                }
                query += "JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SS.ID = SSD.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL) \n";
                query += "JOIN HIS_SESE_DEPO_REPAY SDR ON (SSD.ID = SDR.SERE_SERV_DEPOSIT_ID AND SDR.IS_CANCEL IS NULL) \n";
                query += "JOIN HIS_TRANSACTION RE ON RE.ID = SDR.REPAY_ID \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
                query += "LEFT JOIN HIS_ROOM RO ON RO.ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN HIS_ROOM EXAMRO ON EXAMRO.ID = TREA.TDL_FIRST_EXAM_ROOM_ID \n";
                query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON ER.ROOM_ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00296') \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND RE.IS_CANCEL IS NULL \n";


                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE == true)
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }
                else
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }

                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("AND RE.PAY_FORM_ID ={0} ", filter.PAY_FORM_ID);
                }


                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("AND RE.PAY_FORM_ID in({0}) ", string.Join(",", filter.PAY_FORM_IDs));
                }


                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND RE.TRANSACTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND RE.TRANSACTION_TIME < {0} ", filter.TIME_TO);
                }


                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND RE.CASHIER_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.LOGINNAME != null)
                {
                    query += string.Format("AND RE.CASHIER_LOGINNAME ='{0}' ", filter.LOGINNAME);
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND RE.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }

                if (filter.AREA_IDs != null || filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += "AND (1=0 \n";
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
                    query += string.Format("AND RE.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
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

        public List<Mrs00296RDO> GetBillCancel(Mrs00289Filter filter, List<V_HIS_ROOM> listRoom)
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
                string query = "";
                query += "SELECT \n";
                query += "BI.IS_CANCEL,\n";
                query += "SS.ID AS SERE_SERV_ID, \n";
                query += string.Format("(CASE WHEN SSD.ID IS NULL AND SS.PATIENT_TYPE_ID = {0} AND NVL(SS.VIR_TOTAL_HEIN_PRICE,0) = 0 THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0) WHEN SS.PATIENT_TYPE_ID = {0} WHEN SSD.ID IS NULL AND SS.PATIENT_TYPE_ID = {0} THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0)-nvl(SS.VIR_TOTAL_PATIENT_PRICE_BHYT,0) ELSE 0 END) AS CHENHLECH, \n", HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += "BI.ID AS TRANSACTION_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_TREATMENT_CODE, \n";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, \n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID, \n";
                query += "SRC.CATEGORY_CODE SERVICE_TYPE_CODE, \n";
                query += "SRC.CATEGORY_NAME SERVICE_TYPE_NAME, \n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID, \n";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, \n";
                query += "SS.TDL_EXECUTE_ROOM_ID, \n";
                query += "SS.PATIENT_TYPE_ID SS_PATIENT_TYPE_ID, \n";
                //query += "SS.TDL_REQUEST_ROOM_ID, \n";
                //query += "SS.PRICE, \n";

                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += " (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) TDL_REQUEST_ROOM_ID,\n";
                }
                else
                {
                    query += "SS.TDL_REQUEST_ROOM_ID,\n";
                }
                //query += "(CASE WHEN SSD.ID IS NULL OR SSD.AMOUNT>SSB.PRICE THEN SS.AMOUNT ELSE 0 END) AS AMOUNT_DEPOSIT_BILL, \n";
                query += "0 AS TOTAL_DEPOSIT_BILL_PRICE, \n";
                query += "0 AS TOTAL_PATIENT_BHYT_PRICE, \n";
                //query += "0 AS AMOUNT_REPAY, \n";
                //query += "BI.CASHIER_ROOM_ID, \n";
                query += "(CASE WHEN SSD.ID IS NULL THEN SSB.PRICE ELSE (CASE WHEN SSD.AMOUNT>=SSB.PRICE THEN 0 ELSE SSB.PRICE-SSD.AMOUNT END) END) AS TOTAL_REPAY_PRICE \n";

                query += "FROM HIS_SERE_SERV SS \n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID\n";
                    query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON SR.REQUEST_ROOM_ID = ER.ROOM_ID AND (ER.IS_EXAM IS NULL OR ER.IS_EXAM <>1) AND (ER.IS_SURGERY IS NULL OR ER.IS_SURGERY <>1)\n";
                    query += "LEFT JOIN HIS_SERVICE_REQ PRSR ON PRSR.ID = SR.PARENT_ID\n";
                }
                query += "JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID \n";
                query += "JOIN HIS_TRANSACTION BI ON BI.ID = SSB.BILL_ID \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
                query += "LEFT JOIN HIS_ROOM RO ON RO.ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN HIS_ROOM EXAMRO ON EXAMRO.ID = TREA.TDL_FIRST_EXAM_ROOM_ID \n";
                query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON ER.ROOM_ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SSD.SERE_SERV_ID = SSB.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL AND NVL(BI.KC_AMOUNT,0)>0) \n";
                query += "LEFT JOIN V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00296') \n";
                query += "WHERE 1=1\n";
                query += "AND BI.IS_CANCEL = 1 \n";


                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE == true)
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }
                else
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }

                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("AND BI.PAY_FORM_ID ={0} ", filter.PAY_FORM_ID);
                }

                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("AND BI.PAY_FORM_ID in({0}) ", string.Join(",", filter.PAY_FORM_IDs));
                }

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND BI.CANCEL_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND BI.CANCEL_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND BI.CANCEL_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.LOGINNAME != null)
                {
                    query += string.Format("AND BI.CANCEL_LOGINNAME ='{0}' ", filter.LOGINNAME);//
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }

                if (filter.AREA_IDs != null || filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += "AND (1=0 \n";
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
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00296RDO>(query);
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

        public List<Mrs00296RDO> GetDepositCancel(Mrs00289Filter filter, List<V_HIS_ROOM> listRoom)
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
                string query = "";

                query += "SELECT \n";
                query += "DE.IS_CANCEL,\n";
                query += "SS.ID AS SERE_SERV_ID, \n";
                query += "SS.PATIENT_TYPE_ID SS_PATIENT_TYPE_ID, \n";
                query += string.Format("(CASE WHEN SSD.ID IS NULL AND SS.PATIENT_TYPE_ID = {0} AND NVL(SS.VIR_TOTAL_HEIN_PRICE,0) = 0 THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0) WHEN SSD.ID IS NULL SS.PATIENT_TYPE_ID = {0} THEN nvl(SS.VIR_TOTAL_PATIENT_PRICE,0)-nvl(SS.VIR_TOTAL_PATIENT_PRICE_BHYT,0) ELSE 0 END) AS CHENHLECH, \n", HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                query += "DE.ID AS TRANSACTION_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_TREATMENT_CODE, \n";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, \n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID, \n";
                query += "SRC.CATEGORY_CODE SERVICE_TYPE_CODE, \n";
                query += "SRC.CATEGORY_NAME SERVICE_TYPE_NAME, \n";
                query += "SS.TDL_EXECUTE_DEPARTMENT_ID, \n";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, \n";
                query += "SS.TDL_EXECUTE_ROOM_ID, \n";
                //query += "SS.TDL_REQUEST_ROOM_ID, \n";
                //query += "SS.PRICE, \n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += " (CASE WHEN PRSR.REQUEST_ROOM_ID IS NOT NULL AND ER.ID IS NOT NULL THEN PRSR.REQUEST_ROOM_ID ELSE SS.TDL_REQUEST_ROOM_ID END) TDL_REQUEST_ROOM_ID,\n";
                }
                else
                {
                    query += "SS.TDL_REQUEST_ROOM_ID,\n";
                }
                query += "SS.AMOUNT AS AMOUNT_DEPOSIT_BILL, \n";
                query += "SSD.AMOUNT AS TOTAL_DEPOSIT_BILL_PRICE, \n";
                //query += "SS.VIR_TOTAL_PATIENT_PRICE_BHYT AS TOTAL_PATIENT_BHYT_PRICE, \n";

                query += "0 AS AMOUNT_REPAY, \n";
                query += "DE.CASHIER_ROOM_ID, \n";
                query += "0 AS TOTAL_REPAY_PRICE \n";

                query += "FROM HIS_SERE_SERV SS \n";
                if (filter.IS_MOV_CLS_TO_PARENT == true)
                {
                    query += "LEFT JOIN HIS_SERVICE_REQ SR ON SR.ID = SS.SERVICE_REQ_ID\n";
                    query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON SR.REQUEST_ROOM_ID = ER.ROOM_ID AND (ER.IS_EXAM IS NULL OR ER.IS_EXAM <>1) AND (ER.IS_SURGERY IS NULL OR ER.IS_SURGERY <>1)\n";
                    query += "LEFT JOIN HIS_SERVICE_REQ PRSR ON PRSR.ID = SR.PARENT_ID\n";
                }
                query += "JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID \n";
                query += "JOIN HIS_TRANSACTION DE ON DE.ID = SSD.DEPOSIT_ID \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = SS.TDL_TREATMENT_ID \n";
                query += "LEFT JOIN HIS_ROOM RO ON RO.ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN HIS_ROOM EXAMRO ON EXAMRO.ID = TREA.TDL_FIRST_EXAM_ROOM_ID \n";
                query += "LEFT JOIN HIS_EXECUTE_ROOM ER ON ER.ROOM_ID = SS.TDL_REQUEST_ROOM_ID \n";
                query += "LEFT JOIN V_HIS_SERVICE_RETY_CAT SRC ON (SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00296') \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND DE.IS_CANCEL =1 \n";


                if (filter.IS_PATIENT_TYPE.HasValue && filter.IS_PATIENT_TYPE == true)
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }
                else
                {
                    if (filter.PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID ={0} \n", filter.PATIENT_TYPE_ID);
                    }
                    if (filter.SS_PATIENT_TYPE_ID != null)
                    {
                        query += string.Format("AND SS.PATIENT_TYPE_ID ={0} \n", filter.SS_PATIENT_TYPE_ID);
                    }
                }
                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("AND DE.PAY_FORM_ID ={0} ", filter.PAY_FORM_ID);
                }

                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("AND DE.PAY_FORM_ID in({0}) ", string.Join(",", filter.PAY_FORM_IDs));
                }

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND DE.CANCEL_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND DE.CANCEL_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND DE.CANCEL_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.LOGINNAME != null)
                {
                    query += string.Format("AND DE.CANCEL_LOGINNAME ='{0}' ", filter.LOGINNAME);//
                }
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("AND DE.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs));
                }

                if (filter.AREA_IDs != null || filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += "AND (1=0 \n";
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
                    query += string.Format("AND DE.CANCEL_CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00296RDO>(query);
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

        public List<LoginnameDepa> GetLoginnameDepa()
        {
            try
            {
                string query = "select loginname,department_code,department_name from v_his_employee where department_code is not null";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var result = new MOS.DAO.Sql.SqlDAO().GetSql<LoginnameDepa>(query);
                foreach (var item in result)
                {
                    item.DEPARTMENT_SC = string.Join("", (item.DEPARTMENT_NAME ?? " ").Split(' ').Select(o=>o.Length>0?o.Substring(0,1):"").ToList());
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
    public class LoginnameDepa
    {
        public string LOGINNAME { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string DEPARTMENT_SC { get; set; }
    }

}
