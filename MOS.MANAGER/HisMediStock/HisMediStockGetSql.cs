using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStock
{
    partial class HisMediStockGet : GetBase
    {
        internal List<D_HIS_MEDI_STOCK_1> GetDHisMediStock1(DHisMediStock1Filter filter)
        {
            try
            {
                string query = this.AddInClause(filter.MEDI_STOCK_IDs, "SELECT * FROM D_HIS_MEDI_STOCK_1 WHERE {IN_CLAUSE} ", "MEDI_STOCK_ID");
                return DAOWorker.SqlDAO.GetSql<D_HIS_MEDI_STOCK_1>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
