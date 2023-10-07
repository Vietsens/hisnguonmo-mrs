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

namespace MRS.Processor.Mrs00642
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00642RDO> Get(Mrs00642Filter filter)
        {
            List<Mrs00642RDO> result = new List<Mrs00642RDO>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.ID AS TREATMENT_ID, ";
                query += "ICD.ICD_CODE AS ICD_CODE, ";
                query += "PATI.DOB AS DOB, ";
                query += "TREA.OUT_TIME AS OUT_TIME, ";
                query += "PATI.CAREER_CODE AS CAREER_CODE, ";
                query += "PATI.GENDER_ID AS GENDER_ID ";


                query += "FROM HIS_TREATMENT TREA, ";
                query += "HIS_PATIENT PATI, ";
                query += "HIS_ICD ICD ";
                query += "WHERE 1=1 ";
                query += "AND TREA.PATIENT_ID = PATI.ID AND ((TREA.ICD_SUB_CODE LIKE '%'||ICD.ICD_CODE ||'%' OR TREA.ICD_CODE = ICD.ICD_CODE)) ";
                query += "AND TREA.IS_PAUSE =1 ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TREA.OUT_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TREA.OUT_TIME < {0} ", filter.TIME_TO);
                }
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND TREA.END_DEPARTMENT_ID = {0} ", filter.DEPARTMENT_ID);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00642RDO>(query);
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
