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

namespace MRS.Processor.Mrs00636
{
    public partial class ManagerSql : BusinessBase
    {
        public List<DepartmentCountRDO> GetCountExamDO(Mrs00636Filter filter)
        {
            List<DepartmentCountRDO> result = new List<DepartmentCountRDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SR1.EXECUTE_DEPARTMENT_ID AS DEPARTMENT_ID, ";
                query += "COUNT(SR1.ID) AS COUNT ";

                query += "FROM HIS_SERVICE_REQ SR1, ";
                query += "HIS_TREATMENT TREA, ";
                query += "HIS_SERVICE_REQ SR2 ";
                query += "WHERE 1=1 ";
                query += "AND SR1.TREATMENT_ID = SR2.TREATMENT_ID AND SR2.IS_DELETE =0 AND SR2.IS_NO_EXECUTE IS NULL AND SR2.SERVICE_REQ_TYPE_ID = 1 ";
                query += "AND SR1.IS_DELETE =0 AND SR1.IS_NO_EXECUTE IS NULL AND SR1.SERVICE_REQ_TYPE_ID = 1 ";
                query += "AND TREA.ID = SR1.TREATMENT_ID ";
                query += "AND NOT EXISTS (SELECT 1 FROM DUAL WHERE SR2.ID > SR1.ID AND NVL(SR2.FINISH_TIME,99999999999999)>NVL(SR1.FINISH_TIME,99999999999999)) ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.IN_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.TIME_TO);
                }

                query += "GROUP BY ";
                query += "SR1.EXECUTE_DEPARTMENT_ID ";
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

        public List<DepartmentCountRDO> GetCountTreatDO(Mrs00636Filter filter)
        {
            List<DepartmentCountRDO> result = new List<DepartmentCountRDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "DPT.DEPARTMENT_ID, ";
                query += "PTA1.TREATMENT_TYPE_ID, ";
                query += "(CEIL(SUM(TO_DATE(TO_CHAR(NVL(TREA.OUT_TIME, ";

                if (filter.TIME_TO != null)
                {
                    query += string.Format("{0} ", filter.TIME_TO);
                }
                query += ")),'YYYYMMDDHH24MISS')-TO_DATE(TO_CHAR(TREA.IN_TIME),'YYYYMMDDHH24MISS')))) AS TOTAL_DATE, ";
                query += "COUNT(PTA1.ID) AS COUNT ";

                query += "FROM HIS_PATIENT_TYPE_ALTER PTA1, ";
                query += "HIS_TREATMENT TREA, ";
                query += "HIS_DEPARTMENT_TRAN DPT, ";
                query += "HIS_PATIENT_TYPE_ALTER PTA2 ";
                query += "WHERE 1=1 ";
                query += "AND PTA2.TREATMENT_ID = PTA1.TREATMENT_ID AND PTA1.TREATMENT_TYPE_ID > 1 AND PTA2.TREATMENT_TYPE_ID > 1 ";
                query += "AND DPT.ID = PTA1.DEPARTMENT_TRAN_ID ";
                query += "AND TREA.ID = PTA1.TREATMENT_ID ";
                query += "AND NOT EXISTS (SELECT 1 FROM DUAL WHERE PTA2.ID > PTA1.ID AND PTA2.LOG_TIME > PTA1.LOG_TIME) ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.IN_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.TIME_TO);
                }

                query += "GROUP BY ";
                query += "PTA1.TREATMENT_TYPE_ID, ";
                query += "DPT.DEPARTMENT_ID ";
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

        public List<DepartmentCountRDO> GetCountCategoryDO(Mrs00636Filter filter)
        {
            List<DepartmentCountRDO> result = new List<DepartmentCountRDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID AS DEPARTMENT_ID, ";
                query += "RTC.CATEGORY_CODE, ";
                query += "SUM(SS.AMOUNT) AS COUNT ";

                query += "FROM HIS_SERVICE_RETY_CAT SRC, ";
                query += "HIS_SERE_SERV SS, ";
                query += "HIS_REPORT_TYPE_CAT RTC ";
                query += "WHERE 1=1 ";
                query += "AND SS.SERVICE_ID = SRC.SERVICE_ID AND SRC.REPORT_TYPE_CAT_ID = RTC.ID ";
                query += "AND RTC.REPORT_TYPE_CODE = 'MRS00636' AND SS.IS_DELETE = 0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
             
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} ", filter.TIME_TO);
                }

                query += "GROUP BY ";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, ";
                query += "RTC.CATEGORY_CODE ";
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

        public List<DepartmentCountRDO> GetTotalPriceDO(Mrs00636Filter filter)
        {
            List<DepartmentCountRDO> result = new List<DepartmentCountRDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID AS DEPARTMENT_ID, ";
                query += "SUM(SS.VIR_TOTAL_PRICE) AS COUNT ";

                query += "FROM HIS_SERE_SERV SS, ";
                query += "HIS_TREATMENT TREA ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND TREA.ID = SS.TDL_TREATMENT_ID ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TIME_TO);
                }

                query += "GROUP BY ";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID ";
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
