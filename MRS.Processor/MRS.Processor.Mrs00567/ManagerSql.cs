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
using MRS.Processor.Mrs00567;

namespace MRS.Processor.Mrs00567
{
    public partial class ManagerSql : BusinessBase
    {

        public System.Data.DataTable GetSum(Mrs00567Filter filter, string query)
        {
            System.Data.DataTable result = null;
            try
            {
                query = string.Format(query, (filter.DIAGNOSIS_TIME_TO!= null) ? filter.DIAGNOSIS_TIME_TO.ToString() : "''"
, (filter.DIAGNOSIS_TIME_FROM != null) ? filter.DIAGNOSIS_TIME_FROM.ToString() : "''"

, (filter.ICD_CODE != null) ? filter.ICD_CODE.ToString() : "''"
, (filter.ICD_CODE_INFUSIONs != null) ? filter.ICD_CODE_INFUSIONs.ToString() : "''"

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
    }
}
