using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBlood
{
    partial class HisBloodGet : BusinessBase
    {
        internal List<V_HIS_BLOOD> GetView(HisBloodViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BLOOD GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisBloodViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BLOOD GetViewById(long id, HisBloodViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodDAO.GetViewById(id, filter.Query());
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
