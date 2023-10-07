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
using System.Data;
using MRS.Processor.Mrs00269;

namespace MRS.Processor.Mrs00269
{
    public partial class ManagerSql : BusinessBase
    {

        public System.Data.DataTable GetSum(Mrs00269Filter filter, string query)
        {
            System.Data.DataTable result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"

, (filter.DEPARTMENT_ID != null) ? filter.DEPARTMENT_ID.ToString() : "''"

);
                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            if (ContainsSelectClause(query) && NullOrEmptyRow(result))
            {
                result = new DataTable();
                DataRow row = result.NewRow();
                result.Rows.Add(row);
            }
            return result;
        }

        private bool NullOrEmptyRow(DataTable result)
        {
            return (result == null || result.Rows == null || result.Rows.Count == 0);
        }

        private bool ContainsSelectClause(string query)
        {
            return string.IsNullOrWhiteSpace(query) == false && query.ToUpper().Contains("SELECT");
        }

        public List<HIS_TRAN_PATI_TECH> GetTranTech(Mrs00269Filter filter)
        {
            List<HIS_TRAN_PATI_TECH> result = null;
            try
            {
                string query = "select * from HIS_TRAN_PATI_TECH";
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRAN_PATI_TECH>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }
    }
}
