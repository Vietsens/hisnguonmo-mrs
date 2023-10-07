using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBaby
{
    partial class HisBabyGet : BusinessBase
    {
        internal List<V_HIS_BABY> GetView(HisBabyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBabyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BABY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisBabyViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BABY GetViewById(long id, HisBabyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBabyDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_BABY> GetViewByIds(List<long> ids)
        {
            try
            {
                HisBabyViewFilterQuery filter = new HisBabyViewFilterQuery();
                filter.IDs = ids;
                return this.GetView(filter);
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
