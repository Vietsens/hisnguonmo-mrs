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
        internal List<V_HIS_BID> GetView(HisBidViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BID GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisBidViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BID GetViewById(long id, HisBidViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBidDAO.GetViewById(id, filter.Query());
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
