using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediStockExty
{
    partial class HisMediStockExtyGet : BusinessBase
    {
        internal List<V_HIS_MEDI_STOCK_EXTY> GetView(HisMediStockExtyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediStockExtyDAO.GetView(filter.Query(), param);
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
