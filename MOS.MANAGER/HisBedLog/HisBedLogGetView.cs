using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBedLog
{
    partial class HisBedLogGet : BusinessBase
    {
        internal List<V_HIS_BED_LOG> GetView(HisBedLogViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedLogDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BED_LOG GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisBedLogViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BED_LOG GetViewById(long id, HisBedLogViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBedLogDAO.GetViewById(id, filter.Query());
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
