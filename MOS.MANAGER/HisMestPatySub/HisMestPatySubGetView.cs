using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMestPatySub
{
    partial class HisMestPatySubGet : BusinessBase
    {
        internal List<V_HIS_MEST_PATY_SUB> GetView(HisMestPatySubViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPatySubDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEST_PATY_SUB GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMestPatySubViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEST_PATY_SUB GetViewById(long id, HisMestPatySubViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMestPatySubDAO.GetViewById(id, filter.Query());
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
