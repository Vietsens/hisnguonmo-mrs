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
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00386
{
    internal partial class ManagerSql : BusinessBase
    {
        internal List<PatientTypeAlter> GetPatientTypeAlter(Mrs00386Filter filter)
        {
            List<PatientTypeAlter> result = new List<PatientTypeAlter>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.*, ";
                query += "PTA.TREATMENT_ID, ";
                query += "PTA.RIGHT_ROUTE_CODE, ";
                query += "PTA.HEIN_MEDI_ORG_CODE, ";
                query += "PTA.TREATMENT_TYPE_ID, ";
                query += "PTA.LOG_TIME, ";
                query += "PTA.PATIENT_TYPE_ID ";

                query += "FROM HIS_RS.HIS_PATIENT_TYPE_ALTER PTA, ";
                query += "HIS_RS.HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND PTA.TREATMENT_ID = TREA.ID ";
                query += string.Format("AND ((TREA.IN_TIME BETWEEN {0} AND {1}) OR (TREA.CLINICAL_IN_TIME BETWEEN {0} AND {1}) OR (TREA.OUT_TIME BETWEEN {0} AND {1} AND TREA.TREATMENT_END_TYPE_ID = {2})) ", filter.TIME_FROM, filter.TIME_TO, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<PatientTypeAlter>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<long> GetTreatmentIdKsk(Mrs00386Filter filter, List<string> listServiceCodeKsk)
        {
            List<long> result = new List<long>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "DISTINCT(SS.TDL_TREATMENT_ID) ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";

                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE = 0 ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND SS.IS_EXPEND IS NULL ";
                query += string.Format("AND SS.TDL_INTRUCTION_TIME BETWEEN {0} AND {1} ", filter.TIME_FROM, filter.TIME_TO);
                if (listServiceCodeKsk != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_CODE IN ('{0}') ", string.Join("','", listServiceCodeKsk));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<long>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<SereServCat> GetSereServCat(Mrs00386Filter filter)
        {
            List<SereServCat> result = new List<SereServCat>();
            try
            {
                string query = "";
                query += "SELECT ";
                if (filter.IS_PARENT_FINISH_TIME == true)
                {
                    query += "SR.FINISH_TIME AS TDL_INTRUCTION_TIME, ";
                }
                else
                {
                    query += "SS.TDL_INTRUCTION_TIME, ";
                }
                query += "SR.SERVICE_REQ_STT_ID, \n";
                query += "NVL(SS.TDL_TREATMENT_ID,0) TDL_TREATMENT_ID, \n";
                query += "SS.AMOUNT, ";
                query += "SRC.REPORT_TYPE_CAT_ID ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID \n";
                query += "JOIN HIS_RS.V_HIS_SERVICE_RETY_CAT SRC ON SS.SERVICE_ID = SRC.SERVICE_ID ";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE = 0 and sr.is_delete=0 ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND SS.IS_EXPEND IS NULL ";
                query += "AND SRC.REPORT_TYPE_CODE = 'MRS00386' ";
                if (filter.IS_PARENT_FINISH_TIME == true)
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} AND {1} ", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME BETWEEN {0} AND {1} ", filter.TIME_FROM, filter.TIME_TO);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SereServCat>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<SereServType> GetSereServType(Mrs00386Filter filter)
        {
            List<SereServType> result = new List<SereServType>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.TDL_INTRUCTION_TIME, ";
                query += "SS.AMOUNT, ";
                query += "SS.TDL_SERVICE_TYPE_ID ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE = 0 ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND SS.IS_EXPEND IS NULL ";
                query += string.Format("AND SS.TDL_INTRUCTION_TIME BETWEEN {0} AND {1} ", filter.TIME_FROM, filter.TIME_TO);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SereServType>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<SereServPrice> GetSereServKsk(Mrs00386Filter filter,List<long> KskIds)
        {
            List<SereServPrice> result = new List<SereServPrice>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.IN_DATE AS TIME, ";
                query += "COUNT(DISTINCT(TREA.ID)) AS COUNT_TREATMENT, ";
                query += "SUM(SS.VIR_TOTAL_PRICE) AS VIR_TOTAL_PRICE ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS, ";
                query += "HIS_RS.HIS_TREATMENT TREA ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE = 0 ";
                query += "AND SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND SS.IS_EXPEND IS NULL ";
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} AND {1} ", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0}) ", string.Join(",", KskIds));
                query += "GROUP BY TREA.IN_DATE ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SereServPrice>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<SereServPrice> GetSereServKsk(Mrs00386Filter filter)
        {
            List<SereServPrice> result = new List<SereServPrice>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.IN_DATE AS TIME, ";
                query += "COUNT(DISTINCT(TREA.ID)) AS COUNT_TREATMENT, ";
                query += "SUM(SS.VIR_TOTAL_PRICE) AS VIR_TOTAL_PRICE ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS, ";
                query += "HIS_RS.HIS_TREATMENT TREA ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE = 0 ";
                query += "AND SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND SS.IS_EXPEND IS NULL ";
                query += string.Format("AND TREA.IN_TIME BETWEEN {0} AND {1} ", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", HisPatientTypeCFG.PATIENT_TYPE_ID__KSK);
                query += "GROUP BY TREA.IN_DATE ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SereServPrice>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<SereServPrice> GetSereServVp(Mrs00386Filter filter)
        {
            List<SereServPrice> result = new List<SereServPrice>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "(TREA.FEE_LOCK_TIME - MOD(TREA.FEE_LOCK_TIME,1000000)) AS TIME, ";
                query += "COUNT(DISTINCT(TREA.ID)) AS COUNT_TREATMENT, ";
                query += "SUM(SS.VIR_TOTAL_PRICE) AS VIR_TOTAL_PRICE ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS, ";
                query += "HIS_RS.HIS_TREATMENT TREA ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE = 0 ";
                query += "AND SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND SS.IS_EXPEND IS NULL ";
                query += "AND TREA.IS_ACTIVE=0 ";
                query += string.Format("AND TREA.FEE_LOCK_TIME BETWEEN {0} AND {1} ", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0} ", HisPatientTypeCFG.PATIENT_TYPE_ID__FEE);
                query += "GROUP BY TREA.FEE_LOCK_TIME - MOD(TREA.FEE_LOCK_TIME,1000000) ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SereServPrice>(query);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<SereServPrice> GetSereServBhyt(Mrs00386Filter filter)
        {
            List<SereServPrice> result = new List<SereServPrice>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "(HAP.EXECUTE_TIME - MOD(HAP.EXECUTE_TIME,1000000)) AS TIME, ";
                query += "COUNT(DISTINCT(SS.TDL_TREATMENT_ID)) AS COUNT_TREATMENT, ";
                query += "SUM(SS.VIR_TOTAL_PRICE) AS VIR_TOTAL_PRICE ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS, ";
                query += "HIS_RS.HIS_HEIN_APPROVAL HAP ";
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE = 0 ";
                query += "AND SS.HEIN_APPROVAL_ID = HAP.ID ";
                query += "AND SS.IS_NO_EXECUTE IS NULL ";
                query += "AND SS.IS_EXPEND IS NULL ";
                query += string.Format("AND HAP.EXECUTE_TIME BETWEEN {0} AND {1} ", filter.TIME_FROM, filter.TIME_TO);
                query += "GROUP BY HAP.EXECUTE_TIME - MOD(HAP.EXECUTE_TIME,1000000) ";

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SereServPrice>(query);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<DepartmentInOut> GetDepartmentTran(Mrs00386Filter filter)
        {
            List<DepartmentInOut> result = new List<DepartmentInOut>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "DT.ID, ";
                query += "DT.TREATMENT_ID, ";
                query += "DT.DEPARTMENT_ID, ";
                query += "DT.DEPARTMENT_IN_TIME, ";
                query += "DT1.ID AS NEXT_ID, ";
                query += "DT1.DEPARTMENT_ID AS NEXT_DEPARTMENT_ID, ";
                query += "DT1.DEPARTMENT_IN_TIME AS NEXT_DEPARTMENT_IN_TIME, ";
                query += "(CASE WHEN TREA.IS_PAUSE =1 THEN TREA.OUT_TIME ELSE NULL END) AS OUT_TIME, ";
                query += "(CASE WHEN TREA.IS_PAUSE =1 THEN TREA.OUT_DATE ELSE NULL END) AS OUT_DATE, ";
                query += "TREA.IS_PAUSE, ";
                query += "TREA.TDL_PATIENT_DOB, ";
                query += "TREA.TDL_TREATMENT_TYPE_ID, ";
                query += "(CASE WHEN TREA.TDL_TREATMENT_TYPE_ID >1 THEN TREA.CLINICAL_IN_TIME ELSE NULL END) AS CLINICAL_IN_TIME, ";
                query += "(CASE WHEN TREA.IS_PAUSE =1 THEN TREA.TREATMENT_RESULT_ID ELSE NULL END) AS TREATMENT_RESULT_ID, ";
                query += "(CASE WHEN TREA.IS_PAUSE =1 THEN TREA.TREATMENT_END_TYPE_ID ELSE NULL END) AS TREATMENT_END_TYPE_ID, ";
                query += "TREA.ICD_NAME AS TREATMENT_ICD_NAME, ";
                query += "TREA.ICD_CODE AS TREATMENT_ICD_CODE ";
                query += "FROM HIS_RS.HIS_DEPARTMENT_TRAN DT ";
                query += "LEFT JOIN HIS_RS.HIS_DEPARTMENT_TRAN DT1 ";
                query += "ON DT1.PREVIOUS_ID = DT.ID ";
                query += "LEFT JOIN HIS_RS.HIS_TREATMENT TREA ";
                query += "ON DT.TREATMENT_ID = TREA.ID ";

                query += "WHERE 1=1 ";
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND DT.DEPARTMENT_IN_TIME < {0} ", filter.TIME_TO);
                }
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND (DT1.DEPARTMENT_IN_TIME >={0} OR (DT1.ID IS NULL AND (TREA.IS_PAUSE IS NULL OR (TREA.IS_PAUSE =1 AND TREA.OUT_TIME >={0}))))  ", filter.TIME_FROM);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<DepartmentInOut>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        internal List<HIS_TREATMENT> getTreatment(Mrs00386Filter CastFilter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                string query = "";
                query += "select * from his_treatment\n";
                query += string.Format("where in_time between {0} and {1}\n", CastFilter.TIME_FROM, CastFilter.TIME_TO);
                
                Inventec.Common.Logging.LogSystem.Info(query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_TREATMENT>();
            }
            return result;

        }
    }
}
