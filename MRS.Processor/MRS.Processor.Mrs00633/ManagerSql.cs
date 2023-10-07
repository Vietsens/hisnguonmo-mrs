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

namespace MRS.Processor.Mrs00633
{
    public partial class ManagerSql : BusinessBase
    {
        public List<DepartmentInOut> GetDepartmentTran(Mrs00633Filter filter)
        {
            List<DepartmentInOut> result = new List<DepartmentInOut>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "DT.ID, ";
                query += "DT.PREVIOUS_ID, ";
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
                query += "TREA.IN_DATE, ";
                query += "TREA.IN_TIME, ";
                query += "TREA.TRANSFER_IN_MEDI_ORG_CODE, ";
                query += "TREA.DEATH_DOCUMENT_DATE, ";
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
    }
}
