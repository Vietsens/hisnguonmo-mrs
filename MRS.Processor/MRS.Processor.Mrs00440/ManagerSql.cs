using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisAccidentHurt;
using MRS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00440
{
    class ManagerSql 
    {
        public List<Mrs00440RDO> GetAccident(Mrs00440Filter filter)
        {
            try
            {
                List<Mrs00440RDO> result = new List<Mrs00440RDO>();
                string query = " --thong tin tai nan thuong tich\n";
                query += "SELECT\n";
                query += "TREA.ID TREATMENT_ID, \n";
                query += "TREA.TREATMENT_END_TYPE_ID, \n";
                query += "TREA.IN_TIME, \n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                query += "TREA.TDL_PATIENT_GENDER_ID, \n";
                query += "TREA.ICD_CODE, \n";
                query += "TREA.ICD_CAUSE_CODE, \n";
                query += "PT.CAREER_ID, \n";
                query += "AH.ACCIDENT_BODY_PART_ID, \n";
                query += "AH.ACCIDENT_HURT_TYPE_ID, \n";
                query += "AH.ACCIDENT_CARE_ID, \n";
                query += "AH.ACCIDENT_LOCATION_ID \n";

                query += "FROM HIS_ACCIDENT_HURT AH \n";
                query += "JOIN HIS_TREATMENT TREA ON TREA.ID = AH.TREATMENT_ID\n";
                query += "join his_patient pt on pt.id=trea.patient_id\n";

                query += "WHERE 1=1 \n";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND AH.ACCIDENT_TIME >={0} \n", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND AH.ACCIDENT_TIME <{0} \n", filter.TIME_TO);
                }
                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("AND TREA.DEPARTMENT_IDS ||',' like '{0},%' \n", filter.DEPARTMENT_ID);
                }
                

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00440RDO>(query);
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
        }
    }
}
