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

namespace MRS.Processor.Mrs00627
{
    public partial class ManagerSql : BusinessBase
    {
        public List<DepartmentInOut> GetDepartmentTran(Mrs00627Filter filter)
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
                query += "FROM HIS_DEPARTMENT_TRAN DT ";
                query += "LEFT JOIN HIS_DEPARTMENT_TRAN DT1 ";
                query += "ON DT1.PREVIOUS_ID = DT.ID ";
                query += "LEFT JOIN HIS_TREATMENT TREA ";
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

        public List<PatientTypeAlter> GetPatientTypeAlter(Mrs00627Filter filter)
        {
            List<PatientTypeAlter> result = new List<PatientTypeAlter>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "PTA.ID, ";
                query += "PTA.DEPARTMENT_TRAN_ID, ";
                query += "PTA.TREATMENT_ID, ";
                query += "PTA.LOG_TIME, ";
                query += "PTA.TREATMENT_TYPE_ID, ";
                query += "PTA.PATIENT_TYPE_ID ";
                query += "FROM HIS_PATIENT_TYPE_ALTER PTA ";
                query += "JOIN HIS_DEPARTMENT_TRAN DT ";
                query += "ON PTA.TREATMENT_ID = DT.TREATMENT_ID ";
                query += "LEFT JOIN HIS_DEPARTMENT_TRAN DT1 ";
                query += "ON DT1.PREVIOUS_ID = DT.ID ";
                query += "LEFT JOIN HIS_TREATMENT TREA ";
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

                query += "GROUP BY ";
                query += "PTA.ID, ";
                query += "PTA.DEPARTMENT_TRAN_ID, ";
                query += "PTA.TREATMENT_ID, ";
                query += "PTA.LOG_TIME, ";
                query += "PTA.TREATMENT_TYPE_ID, ";
                query += "PTA.PATIENT_TYPE_ID ";
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

        public List<Client> GetTreatment(Mrs00627Filter filter)
        {
            List<Client> result = new List<Client>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.*, ";

                query += "CASE WHEN 0<(SELECT COUNT(1) FROM HIS_TREATMENT WHERE ID<>TREA.ID AND PATIENT_ID=TREA.PATIENT_ID) THEN 1 ELSE NULL END AS IS_USED_TO_EXAM, ";
                query += "CASE WHEN 0<(SELECT COUNT(1) FROM HIS_SERVICE_REQ WHERE TREATMENT_ID=TREA.ID and is_delete=0 AND SERVICE_REQ_TYPE_ID IN (2,3,8,9) AND REQUEST_ROOM_ID IN (SELECT ID FROM HIS_ROOM WHERE ROOM_TYPE_ID = 3)) THEN 1 ELSE NULL END AS IS_SUBCLINICAL_EXTRA ";
                query += "FROM HIS_TREATMENT TREA ";
                query += "WHERE 1=1 ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.IN_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.TIME_TO);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Client>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<D_HIS_SERE_SERV> GetSS(Mrs00627Filter filter)
        {
            List<D_HIS_SERE_SERV> result = new List<D_HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SS.SERVICE_ID, ";
                query += "SS.TDL_SERVICE_TYPE_ID, ";
                query += "SSP.EMOTIONLESS_METHOD_ID, ";
                query += "SS.TDL_INTRUCTION_DATE, ";
                query += "SS.TDL_REQUEST_DEPARTMENT_ID, ";
                query += "SS.TDL_EXECUTE_ROOM_ID, ";
                query += "SS.AMOUNT, ";
                query += "TREA.IS_EMERGENCY, ";
                query += "TREA.TDL_TREATMENT_TYPE_ID, ";
                query += "SR.SERVICE_REQ_STT_ID, ";
                query += "SS.TDL_SERVICE_CODE ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SS.SERVICE_REQ_ID = SR.ID ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "LEFT JOIN HIS_RS.HIS_SERE_SERV_PTTT SSP ON SSP.SERE_SERV_ID = SS.ID ";
                query += "LEFT JOIN HIS_RS.HIS_TRANSACTION TRAN ON (SS.TDL_SERVICE_TYPE_ID =1 AND TRAN.TREATMENT_ID = SS.TDL_TREATMENT_ID AND TRAN.IS_CANCEL IS NULL and tran.transaction_type_id in (1,3) AND not exists(select 1 from his_rs.his_transaction where is_cancel is null and tran.treatment_id = treatment_id and (transaction_time>tran.transaction_time or (transaction_time=tran.transaction_time and id>tran.id) and transaction_type_id in (1,3) and id<>tran.id) ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TIME_TO);
                }
                query += ")) ";

                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL and sr.is_delete=0 ";
                query += "AND ((SS.TDL_SERVICE_TYPE_ID <>1 ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SS.TDL_INTRUCTION_TIME < {0} ", filter.TIME_TO);
                }
                query += ") OR (SS.TDL_SERVICE_TYPE_ID =1 AND (TRAN.ID IS NOT NULL OR (TREA.IS_ACTIVE=0 ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.FEE_LOCK_TIME < {0} ", filter.TIME_TO);
                }
                query += "))) ) ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<D_HIS_SERE_SERV>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<KskAmount> GetKsk(Mrs00627Filter filter)
        {
            List<KskAmount> result = new List<KskAmount>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "KSK.KSK_CODE AS CODE, ";
                query += "TREA.IN_DATE, ";
                query += "(COUNT(DISTINCT(TREA.ID))) AS COUNT ";

                query += "FROM HIS_RS.HIS_SERE_SERV SS ";
                query += "JOIN HIS_RS.HIS_KSK_SERVICE KSV ON KSV.SERVICE_ID = SS.SERVICE_ID ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON SS.TDL_TREATMENT_ID = TREA.ID ";
                query += "JOIN HIS_RS.HIS_KSK KSK ON KSK.ID = KSV.KSK_ID ";
               
                query += "WHERE 1=1 ";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL AND SS.SERVICE_REQ_ID IS NOT NULL ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.IN_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.TIME_TO);
                }
                query += "GROUP BY ";
                query += "KSK.KSK_CODE, ";
                query += "TREA.IN_DATE ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<KskAmount>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<AppointmentAmount> GetAppointment(Mrs00627Filter filter)
        {
            List<AppointmentAmount> result = new List<AppointmentAmount>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.IN_DATE, ";
                query += "(COUNT(1)) AS COUNT ";

                query += "FROM HIS_RS.HIS_TREATMENT TREA ";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA1 ON (TREA1.TREATMENT_CODE = TREA.APPOINTMENT_CODE AND TREA1.APPOINTMENT_TIME IS NOT NULL) ";

                query += "WHERE 1=1 ";
                query += "AND  (TREA1.APPOINTMENT_TIME-MOD(TREA1.APPOINTMENT_TIME,1000000))= TREA.IN_DATE ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.IN_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.TIME_TO);
                }
                query += "GROUP BY ";
                query += "TREA.IN_DATE ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<AppointmentAmount>(query);


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
