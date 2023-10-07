using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MRS.Processor.Mrs00666
{
    public class ManagerSql
    {
        public System.Data.DataTable GetExmm(Mrs00666Filter filter, string query)
        {
            System.Data.DataTable result = null;
            try
            {

                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"
, (filter.MEDI_STOCK_ID != null) ? filter.MEDI_STOCK_ID.ToString() : "''"
, (filter.MEDI_STOCK_IDs != null) ? string.Join(",",filter.MEDI_STOCK_IDs).ToString() : "''"
, (filter.MEDICINE_TYPE_IDs != null) ? string.Join(",", filter.MEDICINE_TYPE_IDs).ToString() : "''"
, (filter.ACTIVE_INGR_BHYT != null) ? "'"+filter.ACTIVE_INGR_BHYT.ToString()+"'" : "''");
                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
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
