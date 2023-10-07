using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisCashierRoom;

namespace MRS.Processor.Mrs00293
{
    public partial class Mrs00293RDOManager : BusinessBase
    {
        public List<Mrs00293RDO> GetMrs00293RDO(Mrs00293Filter castFilter)
        {
            try
            {
                List<Mrs00293RDO> result = new List<Mrs00293RDO>();
                string query = "\n";
                query += "SELECT \n";
                query += "SS.AMOUNT, \n";
                query += "SS.HEIN_CARD_NUMBER, \n";
                query += "SS.ID, \n";
                query += "SSB.PRICE AS AMOUNT_DEPOSIT, \n";
                query += "SS.TDL_REQUEST_LOGINNAME, \n";
                query += "SS.TDL_REQUEST_USERNAME, \n";
                query += "SS.TDL_SERVICE_TYPE_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME, \n";
                query += "SS.TDL_PATIENT_ID AS PATIENT_ID, \n";
                query += "SS.TDL_TREATMENT_CODE AS TREATMENT_CODE, \n";
                query += "SS.TDL_TREATMENT_ID AS TreatmentId, \n";
                query += "SS.VIR_PRICE AS PRICE, \n";
                query += "SS.TDL_INTRUCTION_TIME, \n";
                query += "SS.TDL_REQUEST_ROOM_ID, \n";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, \n";
                query += "SS.TDL_REQUEST_USERNAME, \n";
                query += "SS.TDL_INTRUCTION_DATE AS INSTRUCTION_DATE, \n";
                query += "BI.TRANSACTION_TIME, \n";
                query += "BI.TRANSACTION_DATE AS TRANSACTION_DATE, \n";
                
                query += "SS.VIR_HEIN_PRICE AS HEIN_PRICE, \n";
                query += "SS.VIR_TOTAL_PATIENT_PRICE AS TDL_VIR_TOTAL_PATIENT_PRICE, \n";
                query += "SS.VIR_TOTAL_HEIN_PRICE AS TDL_VIR_TOTAL_HEIN_PRICE, \n";

                query += "TREA.TDL_TREATMENT_TYPE_ID, \n";

                query += "TREA.TDL_PATIENT_DOB AS DOB, \n";

                query += "TREA.TDL_PATIENT_CODE AS PATIENT_CODE, \n";

                query += "TREA.TDL_PATIENT_NAME AS VIR_PATIENT_NAME, \n";

                query += "TREA.TDL_PATIENT_GENDER_NAME AS GENDER_NAME, \n";

                query += "TREA.TDL_PATIENT_GENDER_ID AS GENDER_ID, \n";

                query += "SR.ICD_CODE AS ICD_CODE, \n";

                query += "SR.ICD_SUB_CODE AS ICD_SUB_CODE, \n";

                query += "SR.ICD_NAME AS ICD_NAME, \n";

                query += "SR.ICD_TEXT AS ICD_TEXT \n";

                query += "FROM HIS_SERE_SERV SS \n";
                query += "JOIN  HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID \n";
                query += "JOIN HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID \n";
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID \n";
                query += "JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID \n";
                query += "JOIN HIS_TRANSACTION BI ON BI.ID = SSB.BILL_ID \n";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SS.ID = SSD.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL) \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND BI.IS_CANCEL IS NULL  AND BI.SALE_TYPE_ID IS NULL  \n";
                query += "AND SSD.ID IS NULL \n";

                if (castFilter.IS_ADD_TREATIN == true)
                {
                    query += "AND TREA.TDL_TREATMENT_TYPE_ID IN (1,2,3,4) \n";
                }
                else
                {
                    query += "AND TREA.TDL_TREATMENT_TYPE_ID IN (1,2) \n";
                }

                if (castFilter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", castFilter.SERVICE_TYPE_ID);
                }
                if (castFilter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", castFilter.SERVICE_TYPE_IDs));
                }

                if (castFilter.SERVICE_ID != null)
                {
                    query += string.Format("AND SV.PARENT_ID = {0} \n", castFilter.SERVICE_ID);
                }
                if (castFilter.SERVICE_IDs != null)
                {
                    query += string.Format("AND ss.service_id in ({0}) \n", string.Join(",", castFilter.SERVICE_IDs));
                }
                if (castFilter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0} \n", castFilter.REQUEST_DEPARTMENT_ID);
                }
                if (castFilter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_ROOM_ID = {0} \n", castFilter.REQUEST_ROOM_ID);
                }
                if (castFilter.TIME_FROM != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME >= {0} \n", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME < {0} \n", castFilter.TIME_TO);
                }
                if (castFilter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = castFilter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                query += "ORDER BY SS.ID \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = DAOWorker.SqlDAO.GetSql<Mrs00293RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
                }
                query = "\n";
                query += "SELECT \n";
                query += "SS.AMOUNT, \n";
                query += "SS.HEIN_CARD_NUMBER, \n";
                query += "SS.ID, \n";
                query += "(CASE WHEN SSB.ID IS NOT NULL THEN SSB.PRICE ELSE SSD.AMOUNT END) AS AMOUNT_DEPOSIT, \n";

                query += "SS.TDL_SERVICE_TYPE_ID, \n";
                query += "SS.SERVICE_ID, \n";
                query += "SS.TDL_SERVICE_CODE, \n";
                query += "SS.TDL_SERVICE_NAME, \n";
                query += "SS.TDL_PATIENT_ID AS PATIENT_ID, \n";
                query += "SS.TDL_TREATMENT_CODE AS TREATMENT_CODE, \n";
                query += "SS.TDL_TREATMENT_ID AS TreatmentId, \n";
                query += "SS.VIR_PRICE AS PRICE, \n";
                query += "SS.TDL_INTRUCTION_TIME, \n";
                query += "SS.TDL_REQUEST_ROOM_ID, \n";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, \n";
                query += "SS.TDL_REQUEST_USERNAME, \n";
                query += "DE.TRANSACTION_TIME, \n";

                query += "DE.TRANSACTION_DATE AS TRANSACTION_DATE, \n";

                query += "SS.TDL_INTRUCTION_DATE AS INSTRUCTION_DATE, \n";
                query += "SS.VIR_HEIN_PRICE AS HEIN_PRICE, \n";
                query += "SS.VIR_TOTAL_PATIENT_PRICE AS TDL_VIR_TOTAL_PATIENT_PRICE, \n";
                query += "SS.VIR_TOTAL_HEIN_PRICE AS TDL_VIR_TOTAL_HEIN_PRICE, \n";

                query += "TREA.TDL_TREATMENT_TYPE_ID, \n";

                query += "TREA.TDL_PATIENT_DOB AS DOB, \n";

                query += "TREA.TDL_PATIENT_CODE AS PATIENT_CODE, \n";

                query += "TREA.TDL_PATIENT_NAME AS VIR_PATIENT_NAME, \n";

                query += "TREA.TDL_PATIENT_GENDER_NAME AS GENDER_NAME, \n";

                query += "TREA.TDL_PATIENT_GENDER_ID AS GENDER_ID, \n";

                query += "SR.ICD_CODE AS ICD_CODE, \n";

                query += "SR.ICD_SUB_CODE AS ICD_SUB_CODE \n";

                query += "FROM HIS_SERE_SERV SS \n";
                query += "JOIN HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID \n";
                query += "JOIN HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID \n";
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID \n";
                query += "JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID \n";
                query += "JOIN HIS_TRANSACTION DE ON DE.ID = SSD.DEPOSIT_ID \n";
                query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON (SS.ID = SSB.SERE_SERV_ID AND SSB.IS_CANCEL IS NULL) \n";
                query += "WHERE SS.IS_DELETE = 0 \n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL \n";
                query += "AND DE.IS_CANCEL IS NULL  AND DE.SALE_TYPE_ID IS NULL  \n";

                if (castFilter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} \n", castFilter.SERVICE_TYPE_ID);
                }
                if (castFilter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID in ({0}) \n", string.Join(",", castFilter.SERVICE_TYPE_IDs));
                }
                if (castFilter.SERVICE_IDs != null)
                {
                    query += string.Format("AND ss.service_id in ({0}) \n", string.Join(",", castFilter.SERVICE_IDs));
                }
                if (castFilter.SERVICE_ID != null)
                {
                    query += string.Format("AND SV.PARENT_ID = {0} \n", castFilter.SERVICE_ID);
                }

                if (castFilter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0} \n", castFilter.REQUEST_DEPARTMENT_ID);
                }
                if (castFilter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_ROOM_ID = {0} \n", castFilter.REQUEST_ROOM_ID);
                }
                if (castFilter.TIME_FROM != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME >= {0} \n", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME < {0} \n", castFilter.TIME_TO);
                }
                if (castFilter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = castFilter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND DE.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }
                query += "ORDER BY SS.ID \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                rs = DAOWorker.SqlDAO.GetSql<Mrs00293RDO>(query);
                if (rs != null)
                {
                    result.AddRange(rs);
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
}
