using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateMaty
{
    partial class HisAnticipateMatyGet : BusinessBase
    {
        internal List<V_HIS_ANTICIPATE_MATY> GetView(HisAnticipateMatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateMatyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ANTICIPATE_MATY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisAnticipateMatyViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ANTICIPATE_MATY GetViewById(long id, HisAnticipateMatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateMatyDAO.GetViewById(id, filter.Query());
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
