using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateMety
{
    partial class HisAnticipateMetyGet : BusinessBase
    {
        internal List<V_HIS_ANTICIPATE_METY> GetView(HisAnticipateMetyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateMetyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ANTICIPATE_METY GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisAnticipateMetyViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_ANTICIPATE_METY GetViewById(long id, HisAnticipateMetyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateMetyDAO.GetViewById(id, filter.Query());
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
