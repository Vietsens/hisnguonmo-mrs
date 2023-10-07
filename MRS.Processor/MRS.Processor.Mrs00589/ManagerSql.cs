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

namespace MRS.Processor.Mrs00589
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_TREATMENT_DEATH> GetTreatmentDeath(Mrs00589Filter filter)
        {
            List<HIS_TREATMENT_DEATH> result = new List<HIS_TREATMENT_DEATH>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.ID AS TREATMENT_ID, ";
                query += "DC.DEATH_CAUSE_CODE ";
                query += "FROM HIS_RS.HIS_TREATMENT TREA ";
                query += "JOIN HIS_RS.HIS_DEATH_CAUSE DC ON DC.ID = TREA.DEATH_CAUSE_ID ";
                query += "JOIN HIS_RS.HIS_ACCIDENT_HURT AH ON AH.TREATMENT_ID = TREA.ID ";

                query += "WHERE 1=1 AND TREA.TREATMENT_END_TYPE_ID = 1 ";
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND AH.CREATE_TIME < {0} ", filter.TIME_TO);
                }
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND AH.CREATE_TIME >{0} ", filter.TIME_FROM);
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT_DEATH>(query);


            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

//        public System.Data.DataTable GetSum(Mrs00589Filter filter, string query)
//        {
//            System.Data.DataTable result = null;
//            try
//            {
//                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
//, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"

//, (filter.ACCIDENT_HURT_TYPE_ID != null) ? filter.ACCIDENT_HURT_TYPE_ID.ToString() : "''"

//, (filter.DEPARTMENT_IDs != null) ? string.Join(",", filter.DEPARTMENT_IDs) : "''"

//, (filter.EXAM_ROOM_IDs != null) ? string.Join(",", filter.EXAM_ROOM_IDs) : "''"

//);
//                List<string> errors = new List<string>();
//                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
//                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
//            }
//            catch (Exception ex)
//            {
//                Inventec.Common.Logging.LogSystem.Error(ex);
//                result = null;
//            }
//            return result;
//        }
    }
}
