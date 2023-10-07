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
using MRS.Processor.Mrs00687;

namespace MRS.Processor.Mrs00687
{
    public partial class ManagerSql : BusinessBase
    {
        public DataTable Get(Mrs00687Filter filter, string query)
        {
            DataTable result = null;
            try
            {
                query = string.Format(query, (filter.TIME_TO != null) ? filter.TIME_TO.ToString() : "''"
, (filter.TIME_FROM != null) ? filter.TIME_FROM.ToString() : "''"

, (filter.MEDI_STOCK_ID != null) ? filter.MEDI_STOCK_ID.ToString() : "''"

, (filter.TRUE_FALSE ==true) ? "'1'" : "''"

);
                List<string> error = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query,ref error);
                Inventec.Common.Logging.LogSystem.Info(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public List<string> error { get; set; }
    }
}
