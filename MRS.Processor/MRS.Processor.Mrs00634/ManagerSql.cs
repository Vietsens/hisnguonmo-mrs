using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00634
{
    class ManagerSql
    {
        public static List<long> GetServiceId()
        {
            List<long> result = new List<long>();
            try
            {
                string query = "SELECT SERVICE_ID FROM HIS_MEDICINE_TYPE WHERE IS_VACCINE = 1";

                Inventec.Common.Logging.LogSystem.Info("query:" + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<long>(query);
            }
            catch (Exception ex)
            {
                result = new List<long>();
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
