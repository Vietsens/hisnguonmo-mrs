using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRemuneration
{
    partial class HisRemunerationGet : BusinessBase
    {
        internal List<V_HIS_REMUNERATION> GetView(HisRemunerationViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRemunerationDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_REMUNERATION GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisRemunerationViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_REMUNERATION GetViewById(long id, HisRemunerationViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisRemunerationDAO.GetViewById(id, filter.Query());
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
