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

namespace MRS.Processor.Mrs00641
{
    public partial class ManagerSql : BusinessBase
    {
        public List<DepartmentCountRDO> GetAmountInDO(Mrs00641Filter filter)
        {
            List<DepartmentCountRDO> result = new List<DepartmentCountRDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.END_DEPARTMENT_ID AS DEPARTMENT_ID, ";
                query += "SS.SERVICE_ID AS SERVICE_ID, ";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID, ";
                query += "SS.PATIENT_TYPE_ID AS PATIENT_TYPE_ID, ";
                query += "SUM(NVL(SSD.AMOUNT,0)+NVL(SSB.PRICE,0)) AS AMOUNT ";

                query += "FROM HIS_SERE_SERV SS ";
                query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID ";
                query += "LEFT JOIN HIS_TRANSACTION BI ON (BI.ID = SSB.BILL_ID AND BI.IS_CANCEL IS NULL) ";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID ";
                query += "LEFT JOIN HIS_TRANSACTION DE ON (DE.ID = SSD.DEPOSIT_ID AND DE.IS_CANCEL IS NULL), ";
                query += "HIS_TREATMENT TREA ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND (DE.ID is not null or bi.ID is not null) ";
                query += "AND TREA.ID = SS.TDL_TREATMENT_ID ";
                query += "AND EXISTS(SELECT 1 FROM HIS_PATIENT_TYPE_ALTER WHERE TREATMENT_ID=TREA.ID AND TREATMENT_TYPE_ID=3 AND (LOG_TIME<=DE.TRANSACTION_TIME OR LOG_TIME<=BI.TRANSACTION_TIME) FETCH FIRST ROWS ONLY) ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.OUT_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.OUT_TIME < {0} ", filter.TIME_TO);
                }

                query += "GROUP BY ";
                query += "TREA.END_DEPARTMENT_ID, ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SS.PATIENT_TYPE_ID ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<DepartmentCountRDO>(query);
                if (rs != null)
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

        public List<DepartmentCountRDO> GetAmountOutDO(Mrs00641Filter filter)
        {
            List<DepartmentCountRDO> result = new List<DepartmentCountRDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.SERVICE_ID AS SERVICE_ID, ";
                query += "SS.PATIENT_TYPE_ID AS PATIENT_TYPE_ID, ";
                query += "SUM(NVL(SSD.AMOUNT,0)+NVL(SSB.PRICE,0)) AS AMOUNT ";

                query += "FROM HIS_SERE_SERV SS ";
                query += "LEFT JOIN HIS_SERE_SERV_BILL SSB ON SS.ID = SSB.SERE_SERV_ID ";
                query += "LEFT JOIN HIS_TRANSACTION BI ON (BI.ID = SSB.BILL_ID AND BI.IS_CANCEL IS NULL) ";
                query += "LEFT JOIN HIS_SERE_SERV_DEPOSIT SSD ON SS.ID = SSD.SERE_SERV_ID ";
                query += "LEFT JOIN HIS_TRANSACTION DE ON (DE.ID = SSD.DEPOSIT_ID AND DE.IS_CANCEL IS NULL), ";
                query += "HIS_TREATMENT TREA ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND (DE.ID is not null or bi.ID is not null) ";
                query += "AND TREA.ID = SS.TDL_TREATMENT_ID ";
                query += "AND NOT EXISTS(SELECT 1 FROM HIS_PATIENT_TYPE_ALTER WHERE TREATMENT_ID=TREA.ID AND TREATMENT_TYPE_ID=3 AND (LOG_TIME<=DE.TRANSACTION_TIME OR LOG_TIME<=BI.TRANSACTION_TIME) FETCH FIRST ROWS ONLY) ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.OUT_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.OUT_TIME < {0} ", filter.TIME_TO);
                }

                query += "GROUP BY ";
                query += "SS.SERVICE_ID, ";
                query += "SS.PATIENT_TYPE_ID ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<DepartmentCountRDO>(query);
                if (rs != null)
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
}
