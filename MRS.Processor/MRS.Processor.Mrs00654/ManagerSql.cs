using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00654
{
    class ManagerSql
    {
        internal static List<SAR.EFMODEL.DataModels.SAR_PRINT_LOG> GetPrintLog(string printTypeCode)
        {
            List<SAR.EFMODEL.DataModels.SAR_PRINT_LOG> result = new List<SAR.EFMODEL.DataModels.SAR_PRINT_LOG>();
            try
            {
                string query = string.Format("SELECT * FROM SAR_PRINT_LOG WHERE PRINT_TYPE_CODE = '{0}'", printTypeCode);

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new SAR.DAO.Sql.SqlDAO().GetSql<SAR.EFMODEL.DataModels.SAR_PRINT_LOG>(query);
            }
            catch (Exception ex)
            {
                result = null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        
    }
}
