using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAccidentHurt
{
    partial class HisAccidentHurtGet : BusinessBase
    {
        internal List<V_HIS_ACCIDENT_HURT> GetView(HisAccidentHurtViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHurtDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ACCIDENT_HURT GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisAccidentHurtViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ACCIDENT_HURT GetViewById(long id, HisAccidentHurtViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAccidentHurtDAO.GetViewById(id, filter.Query());
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
