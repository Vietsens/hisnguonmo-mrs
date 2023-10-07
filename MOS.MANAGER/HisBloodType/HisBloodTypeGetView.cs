using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisBloodType
{
    partial class HisBloodTypeGet : BusinessBase
    {
        internal List<V_HIS_BLOOD_TYPE> GetView(HisBloodTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodTypeDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BLOOD_TYPE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisBloodTypeViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_BLOOD_TYPE GetViewById(long id, HisBloodTypeViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisBloodTypeDAO.GetViewById(id, filter.Query());
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
