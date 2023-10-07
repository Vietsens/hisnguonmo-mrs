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

namespace MRS.Processor.Mrs00629
{
    public partial class ManagerSql
    {
        public List<CoTreatmentDepartment> GetCoTreatmentDepartment(Mrs00629Filter filter)
        {
            List<CoTreatmentDepartment> result = new List<CoTreatmentDepartment>();
            try
            {
                string query = " --danh sach khoa dieu tri ket hop\n";
                query += "SELECT \n";
                query += "CT.DEPARTMENT_ID, \n";
                query += "TREA.ID TREATMENT_ID \n";
                query += "FROM HIS_CO_TREATMENT CT \n";
                query += "JOIN HIS_TREATMENT TREA ON CT.TDL_TREATMENT_ID = TREA.ID \n";

                query += "WHERE 1=1 \n";
                query += string.Format("AND TREA.OUT_TIME BETWEEN {0} AND {1}\n", filter.OUT_TIME_FROM,filter.OUT_TIME_TO);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<CoTreatmentDepartment>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }

            return result;
        }
    }
}
