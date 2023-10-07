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

namespace MRS.Processor.Mrs00295
{
    public partial class Mrs00295RDOManager : BusinessBase
    {
        public List<Mrs00295RDO> GetBill(Mrs00295Filter castFilter)
        {
            try
            {
                List<Mrs00295RDO> result = new List<Mrs00295RDO>();
                string query = "";
                query += "SELECT ";

                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, ";
                query += "SS.TDL_TREATMENT_ID AS TREATMENT_ID, ";
                query += "TREA.TDL_TREATMENT_TYPE_ID, ";

                query += "SUM(SSB.PRICE) AS AMOUNT_DEPOSIT, ";
                query += "SUM(SS.AMOUNT) AS TOTAL_DEPOSIT_PRICE, ";
                query += "MAX(SS.VIR_PRICE) AS PRICE ";

                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID ";
                query += "JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID ";
                query += "JOIN HIS_TRANSACTION BI ON BI.ID = SSB.BILL_ID ";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON (SS.ID = SSD.SERE_SERV_ID AND SSD.IS_CANCEL IS NULL) ";
                query += "WHERE SS.IS_DELETE = 0 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND BI.IS_CANCEL IS NULL AND BI.SALE_TYPE_ID IS NULL ";
                query += "AND SSD.ID IS NULL ";
                query += "AND TREA.TDL_TREATMENT_TYPE_ID IN (1,2) ";

                if (castFilter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} ", castFilter.SERVICE_TYPE_ID);
                }

                if (castFilter.SERVICE_ID != null)
                {
                    query += string.Format("AND SV.PARENT_ID = {0} ", castFilter.SERVICE_ID);
                }
                if (castFilter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0} ", castFilter.REQUEST_DEPARTMENT_ID);
                }
                if (castFilter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SS.REQUEST_ROOM_ID = {0} ", castFilter.REQUEST_ROOM_ID);
                }
                if (castFilter.TIME_FROM != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME >= {0} ", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO != null)
                {
                    query += string.Format("AND BI.TRANSACTION_TIME < {0} ", castFilter.TIME_TO);
                }
                if (castFilter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = castFilter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND BI.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }

                query += "GROUP BY ";
                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME, ";
                query += "SS.TDL_TREATMENT_ID, ";
                query += "TREA.TDL_TREATMENT_TYPE_ID ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = DAOWorker.SqlDAO.GetSql<Mrs00295RDO>(query);
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
        public List<Mrs00295RDO> GetRepay(Mrs00295Filter castFilter)
        {
            try
            {
                List<Mrs00295RDO> result = new List<Mrs00295RDO>();
                string query = "";
                
                query += "SELECT ";

                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, ";
                query += "SS.TDL_TREATMENT_ID AS TREATMENT_ID, ";
                query += "TREA.TDL_TREATMENT_TYPE_ID, ";

                query += "SUM(SDR.AMOUNT) AS AMOUNT_REPAY, ";
                query += "SUM(SS.AMOUNT) AS TOTAL_REPAY_PRICE, ";
                query += "MAX(SS.VIR_PRICE) AS PRICE ";


                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID ";
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID ";
                query += "JOIN HIS_SESE_DEPO_REPAY SDR ON SSD.ID = SDR.SERE_SERV_DEPOSIT_ID ";
                query += "JOIN HIS_TRANSACTION RE ON RE.ID = SDR.REPAY_ID ";
                query += "WHERE SS.IS_DELETE = 0 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND SSD.IS_CANCEL IS NULL ";
                query += "AND SDR.IS_CANCEL IS NULL ";
                query += "AND RE.IS_CANCEL IS NULL ";
                if (castFilter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} ", castFilter.SERVICE_TYPE_ID);
                }

                if (castFilter.SERVICE_ID != null)
                {
                    query += string.Format("AND SV.PARENT_ID = {0} ", castFilter.SERVICE_ID);
                }

                if (castFilter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0} ", castFilter.REQUEST_DEPARTMENT_ID);
                }
                if (castFilter.TIME_FROM != null)
                {
                    query += string.Format("AND RE.TRANSACTION_TIME >= {0} ", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO != null)
                {
                    query += string.Format("AND RE.TRANSACTION_TIME < {0} ", castFilter.TIME_TO);
                }
                if (castFilter.REQUEST_ROOM_ID !=null)
                {
                    query += string.Format("AND SS.REQUEST_ROOM_ID = {0} ", castFilter.REQUEST_ROOM_ID);
                }
                if (castFilter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = castFilter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND RE.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }

                query += "GROUP BY ";

                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME, ";
                query += "SS.TDL_TREATMENT_ID, ";
                query += "TREA.TDL_TREATMENT_TYPE_ID ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = DAOWorker.SqlDAO.GetSql<Mrs00295RDO>(query);
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
        public List<Mrs00295RDO> GetDeposit(Mrs00295Filter castFilter)
        {
            try
            {
                List<Mrs00295RDO> result = new List<Mrs00295RDO>();
                string query = "";
                query += "SELECT ";

                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME, ";
                query += "SS.TDL_TREATMENT_ID AS TREATMENT_ID, ";
                query += "TREA.TDL_TREATMENT_TYPE_ID, ";

                query += "SUM(CASE WHEN SSB.ID IS NOT NULL THEN SSB.PRICE ELSE SSD.AMOUNT END) AS AMOUNT_DEPOSIT, ";
                query += "SUM(SS.AMOUNT) AS TOTAL_DEPOSIT_PRICE, ";
                query += "MAX(SS.VIR_PRICE) AS PRICE ";


                query += "FROM HIS_SERE_SERV SS ";
                query += "JOIN HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "JOIN HIS_SERVICE SV ON SS.SERVICE_ID = SV.ID ";
                query += "JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID ";
                query += "JOIN HIS_TRANSACTION DE ON DE.ID = SSD.DEPOSIT_ID ";
                query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON (SS.ID = SSB.SERE_SERV_ID AND SSB.IS_CANCEL IS NULL) ";
                query += "WHERE SS.IS_DELETE = 0 ";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND DE.IS_CANCEL IS NULL AND DE.SALE_TYPE_ID IS NULL ";

                if (castFilter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0} ", castFilter.SERVICE_TYPE_ID);
                }

                if (castFilter.SERVICE_ID != null)
                {
                    query += string.Format("AND SV.PARENT_ID = {0} ", castFilter.SERVICE_ID);
                }

                if (castFilter.REQUEST_DEPARTMENT_ID != null)
                {
                    query += string.Format("AND SS.TDL_REQUEST_DEPARTMENT_ID = {0} ", castFilter.REQUEST_DEPARTMENT_ID);
                }
                if (castFilter.TIME_FROM != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME >= {0} ", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO != null)
                {
                    query += string.Format("AND DE.TRANSACTION_TIME < {0} ", castFilter.TIME_TO);
                }
                if (castFilter.REQUEST_ROOM_ID != null)
                {
                    query += string.Format("AND SS.REQUEST_ROOM_ID = {0} ", castFilter.REQUEST_ROOM_ID);
                }
                if (castFilter.BRANCH_ID != null)
                {
                    HisCashierRoomViewFilterQuery HisCashierRoomfilter = new HisCashierRoomViewFilterQuery();
                    HisCashierRoomfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    HisCashierRoomfilter.BRANCH_ID = castFilter.BRANCH_ID;
                    var cashierRooms = new HisCashierRoomManager().GetView(HisCashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                    query += string.Format("AND DE.CASHIER_ROOM_ID IN ({0}) ", string.Join(",", cashierRooms.Select(o => o.ID).ToList()));
                }

                query += "GROUP BY ";

                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_CODE, ";
                query += "SS.TDL_SERVICE_NAME, ";
                query += "SS.TDL_TREATMENT_ID, ";
                query += "TREA.TDL_TREATMENT_TYPE_ID ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = DAOWorker.SqlDAO.GetSql<Mrs00295RDO>(query);
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
