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

namespace MRS.Processor.Mrs00112
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00112RDO> GetTreatment(Mrs00112Filter filter)
        {
            List<Mrs00112RDO> result = new List<Mrs00112RDO>();
            try
            {
                string query = "";
                query += "SELECT \n";
                query += "SS.*, \n";
                query += "TREA.TDL_KSK_CONTRACT_ID KSK_CONTRACT_ID, \n";
                query += "TREA.TDL_PATIENT_CODE PATIENT_CODE, \n";
                query += "WP.WORK_PLACE_NAME CUSTOMER_NAME, \n";
                query += "TREA.TREATMENT_CODE, \n";
                query += "TREA.TDL_PATIENT_NAME VIR_PATIENT_NAME, \n";
                query += "TREA.TDL_PATIENT_GENDER_NAME GENDER_NAME, \n";
                query += "TREA.TDL_PATIENT_CODE, \n";
                query += "TREA.TDL_PATIENT_NAME, \n";
                query += "TREA.TDL_PATIENT_ADDRESS, \n";
                query += "TREA.TDL_PATIENT_DOB, \n";
                query += "TREA.TDL_PATIENT_GENDER_NAME, \n";
                query += "TREA.ICD_NAME, \n";
                query += "TREA.ICD_TEXT \n";

                query += "FROM HIS_RS.HIS_SERE_SERV SS \n";
                query += "JOIN HIS_RS.HIS_TREATMENT TREA ON TREA.ID =SS.TDL_TREATMENT_ID \n";
                query += "LEFT JOIN HIS_RS.HIS_KSK_CONTRACT KC ON KC.ID =TREA.TDL_KSK_CONTRACT_ID \n";
                query += "LEFT JOIN HIS_RS.HIS_WORK_PLACE WP ON WP.ID =KC.WORK_PLACE_ID \n";
                query += "JOIN HIS_RS.HIS_SERVICE_REQ SR ON SR.ID =SS.SERVICE_REQ_ID \n";
                query += "WHERE 1=1 \n";
                query += "AND SS.IS_DELETE =0 AND SS.IS_NO_EXECUTE IS NULL AND SS.IS_EXPEND IS NULL and sr.is_delete =0 \n";
                query += string.Format("AND SS.PATIENT_TYPE_ID = {0} \n", HisPatientTypeCFG.PATIENT_TYPE_ID__KSKHD);
               
                if (filter.KSK_CONTRACT_ID != null)
                {
                    query += string.Format("AND TREA.TDL_KSK_CONTRACT_ID = {0} \n", filter.KSK_CONTRACT_ID);
                }

                if (filter.KSK_CONTRACT_IDs != null)
                {
                    query += string.Format("AND TREA.TDL_KSK_CONTRACT_ID IN ({0}) \n", string.Join(",", filter.KSK_CONTRACT_IDs));
                }

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND SR.FINISH_TIME >= {0} \n", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND SR.FINISH_TIME < {0} \n", filter.TIME_TO);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00112RDO>(query);
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
