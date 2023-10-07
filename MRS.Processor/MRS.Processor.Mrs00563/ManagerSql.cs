using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.DAO.Sql;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00563
{
    class ManagerSql
    {

        public List<LastDepartment> Get(long min, long max)
        {
            List<LastDepartment> result = new List<LastDepartment>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "DPT.TREATMENT_ID, ";
                query += "max(department_id) keep(dense_rank last order by dpt.id) DEPARTMENT_ID ";
                query += "FROM HIS_DEPARTMENT_TRAN DPT ";
                query += "WHERE DPT.TREATMENT_ID BETWEEN {0} AND {1} ";

                query += "GROUP BY ";
                query += "DPT.TREATMENT_ID ";
                query = string.Format(query,min,max);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SqlDAO().GetSql<LastDepartment>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }


            return result;
        }
    }
}
