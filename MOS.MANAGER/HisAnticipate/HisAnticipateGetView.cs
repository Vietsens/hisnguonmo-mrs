using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipate
{
    partial class HisAnticipateGet : BusinessBase
    {
        internal List<V_HIS_ANTICIPATE> GetView(HisAnticipateViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ANTICIPATE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisAnticipateViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ANTICIPATE GetViewById(long id, HisAnticipateViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateDAO.GetViewById(id, filter.Query());
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
