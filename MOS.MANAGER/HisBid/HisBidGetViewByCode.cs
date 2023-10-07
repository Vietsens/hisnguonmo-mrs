using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBid
{
    partial class HisBidGet : BusinessBase
    {
        internal V_HIS_BID GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisBidViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BID GetViewByCode(string code, HisBidViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidDAO.GetViewByCode(code, filter.Query());
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
