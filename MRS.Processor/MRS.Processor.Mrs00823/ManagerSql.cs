using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00823
{
    public partial class ManagerSql
    {
        public List<V_HIS_SERVICE_RETY_CAT> GetServiceRetyCat()
        {
            List<V_HIS_SERVICE_RETY_CAT> result = null;
            try
            {
                string query = "";
                query += "SELECT * FROM V_HIS_SERVICE_RETY_CAT \n";
                query += "WHERE 1=1 \n";
                query += "AND REPORT_TYPE_CODE = 'MRS00823' \n";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_SERVICE_RETY_CAT>(query);
            }
            catch(Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }
}
